using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    // Reference to the order delivery station that manages orders
    [SerializeField] private OrderDeliveryStation orderDeliveryStation;
    
    // Main UI container
    [SerializeField] private GameObject orderPanelTemplate; // Template for order panels
    [SerializeField] private Transform orderContainer; // Container for all order panels
    
    [Header("UI Components")]
    [SerializeField] private float foodIconSize = 50f;     // Size of food icons
    [SerializeField] private float componentIconSize = 30f; // Size of component icons
    
    // Dictionary to track which orders are being displayed and their UI elements
    private Dictionary<OrderDeliveryStation.Order, GameObject> activeOrderPanels = new Dictionary<OrderDeliveryStation.Order, GameObject>();
    
    private void Start()
    {
        // Auto-find OrderDeliveryStation if not assigned
        if (orderDeliveryStation == null)
        {
            orderDeliveryStation = FindObjectOfType<OrderDeliveryStation>();
            
            if (orderDeliveryStation != null)
            {
                Debug.Log("OrderDeliveryStation automatically assigned.");
            }
            else
            {
                Debug.LogError("No OrderDeliveryStation found in the scene!");
            }
        }
        
        // Hide template panel if it exists
        if (orderPanelTemplate != null)
        {
            orderPanelTemplate.SetActive(false);
        }
        else
        {
            Debug.LogError("Order panel template is not assigned!");
        }
        
        // Make sure the container exists
        if (orderContainer == null)
        {
            orderContainer = transform;
            Debug.Log("Using self as order container.");
        }
    }
    
    private void Update()
    {
        if (orderDeliveryStation != null)
        {
            UpdateOrdersDisplay(orderDeliveryStation.Orders);
        }
    }
    
    // Update the UI to display all current orders
    private void UpdateOrdersDisplay(List<OrderDeliveryStation.Order> currentOrders)
    {
        // Safety check
        if (currentOrders == null)
        {
            Debug.LogError("Current orders list is null!");
            return;
        }
        
        // First, identify orders that are no longer active and remove their panels
        List<OrderDeliveryStation.Order> ordersToRemove = new List<OrderDeliveryStation.Order>();
        
        foreach (var orderPanel in activeOrderPanels)
        {
            if (!currentOrders.Contains(orderPanel.Key))
            {
                ordersToRemove.Add(orderPanel.Key);
            }
        }
        
        // Remove panels for completed/expired orders
        foreach (var order in ordersToRemove)
        {
            if (activeOrderPanels[order] != null)
            {
                Destroy(activeOrderPanels[order]);
            }
            activeOrderPanels.Remove(order);
        }
        
        // Create or update panels for all current orders
        for (int i = 0; i < currentOrders.Count; i++)
        {
            OrderDeliveryStation.Order order = currentOrders[i];
            
            // Create new panel if this order doesn't have one
            if (!activeOrderPanels.ContainsKey(order))
            {
                CreateOrderPanel(order);
            }
            else if (activeOrderPanels[order] != null)
            {
                // Update time remaining for this order
                UpdateOrderTimer(order);
            }
            else
            {
                // Panel reference is null but exists in dictionary - recreate it
                activeOrderPanels.Remove(order);
                CreateOrderPanel(order);
            }
        }
    }
    
    // Create a new panel for a specific order
    private void CreateOrderPanel(OrderDeliveryStation.Order order)
    {
        // Safety checks
        if (orderPanelTemplate == null)
        {
            Debug.LogError("Order panel template is null!");
            return;
        }
        
        if (orderContainer == null)
        {
            Debug.LogError("Order container is null!");
            return;
        }
        
        if (order == null || order.recipe == null || order.recipe.resultMeal == null)
        {
            Debug.LogError("Order or its recipe is invalid!");
            return;
        }
        
        // Instantiate a new order panel from the template
        GameObject panelObj = Instantiate(orderPanelTemplate, orderContainer);
        if (panelObj == null)
        {
            Debug.LogError("Failed to instantiate order panel!");
            return;
        }
        
        panelObj.SetActive(true); // Make it visible
        
        // Add to tracking dictionary
        activeOrderPanels.Add(order, panelObj);
        
        Debug.Log($"Creating panel for order with recipe: {order.recipe.name}");
        
        // Find UI elements using the correct paths
        Slider timeSlider = panelObj.transform.Find("Progress").GetComponent<Slider>();
        Image foodImg = null;
        Transform foodIconTrans = panelObj.transform.Find("FoodBackground/FoodIcon");
        if (foodIconTrans != null)
        {
            foodImg = foodIconTrans.GetComponent<Image>();
        }
        
        Transform componentsTransform = panelObj.transform.Find("Component");
        
        // Log found/missing components
        if (timeSlider == null) Debug.LogError("Time slider not found in panel!");
        if (foodImg == null) Debug.LogError("Food icon not found in panel!");
        if (componentsTransform == null) Debug.LogError("Components transform not found in panel!");
        
        if (timeSlider == null || foodImg == null || componentsTransform == null)
        {
            Debug.LogError("Order panel template missing required components!");
            return;
        }
        
        // Set food icon
        foodImg.sprite = order.recipe.resultMeal.sprite;
        if (foodImg.sprite == null)
        {
            Debug.LogError("Recipe resultMeal sprite is null!");
        }
        else
        {
            foodImg.SetNativeSize(); // First set native size to preserve aspect ratio
            RectTransform foodIconRect = foodImg.GetComponent<RectTransform>();
            foodIconRect.sizeDelta = new Vector2(foodIconSize, foodIconSize); // Then force to consistent size
            foodImg.preserveAspect = true;
        }
        
        // Create recipe components display
        DisplayRecipeComponents(order.recipe, componentsTransform);
        
        // Enable all layout groups to ensure proper arrangement
        EnableAllLayoutGroups(panelObj);
        
        // Force layout rebuild
        StartCoroutine(DelayedLayoutRebuild(panelObj));
    }
    
    // Display recipe components (ingredients and tools) in the given container
    private void DisplayRecipeComponents(Recipe recipe, Transform componentsContainer)
    {
        if (recipe == null || recipe.Requireds == null)
        {
            Debug.LogError("Recipe or its required components are null!");
            return;
        }
        
        // Find the template objects first
        Transform ingredientToolGroupTemplate = null;
        foreach (Transform child in componentsContainer)
        {
            if (child.name.Contains("IngredientToolGroup"))
            {
                ingredientToolGroupTemplate = child;
                break;
            }
        }
        
        // If we didn't find the template in the hierarchy, log error
        if (ingredientToolGroupTemplate == null)
        {
            Debug.LogError("IngredientToolGroup template not found in components container!");
            return;
        }
        
        // Temporarily enable the template
        ingredientToolGroupTemplate.gameObject.SetActive(true);
        
        // Find the ingredient and tool groups in the template
        Transform ingredientGroupTrans = ingredientToolGroupTemplate.transform.Find("Ingredients");
        Transform toolGroupTrans = ingredientToolGroupTemplate.transform.Find("Tools");
        
        if (ingredientGroupTrans == null || toolGroupTrans == null)
        {
            Debug.LogError("Required child groups not found in the IngredientToolGroup!");
            return;
        }
        
        // Find template icons
        Transform ingredientIconTemplate = null;
        if (ingredientGroupTrans.childCount > 0)
        {
            ingredientIconTemplate = ingredientGroupTrans.GetChild(0);
            ingredientIconTemplate.gameObject.SetActive(true);
        }
        
        Transform toolIconTemplate = null;
        if (toolGroupTrans.childCount > 0)
        {
            toolIconTemplate = toolGroupTrans.GetChild(0);
            toolIconTemplate.gameObject.SetActive(true);
        }
        
        // Create a clone for each recipe step
        for (int i = 0; i < recipe.Requireds.Count; i++)
        {
            var required = recipe.Requireds[i];
            
            // Safety check
            if (required.ingredientIcon == null || required.operationIcon == null)
            {
                Debug.LogError($"Required component at index {i} has null icons!");
                continue;
            }
            
            // Instantiate a new ingredient-tool group
            GameObject groupObj = Instantiate(ingredientToolGroupTemplate.gameObject, componentsContainer);
            groupObj.name = $"IngredientToolGroup_{i}";
            groupObj.SetActive(true); // Make it visible
            
            // Find the ingredient and tool groups in the new instance
            Transform newIngredientGroupTrans = groupObj.transform.Find("Ingredients");
            Transform newToolGroupTrans = groupObj.transform.Find("Tools");
            
            // Clear existing template icons from the new instance
            if (newIngredientGroupTrans != null)
            {
                foreach (Transform child in newIngredientGroupTrans)
                {
                    Destroy(child.gameObject);
                }
            }
            
            if (newToolGroupTrans != null)
            {
                foreach (Transform child in newToolGroupTrans)
                {
                    Destroy(child.gameObject);
                }
            }
            
            // Add ingredient icons
            for (int j = 0; j < required.ingredientIcon.Count; j++)
            {
                var ingredientSprite = required.ingredientIcon[j];
                if (ingredientSprite == null) continue;
                
                GameObject iconObj = Instantiate(ingredientIconTemplate.gameObject, newIngredientGroupTrans);
                iconObj.name = $"IngredientIcon_{j}";
                iconObj.SetActive(true);
                
                Image imgComponent = iconObj.GetComponent<Image>();
                if (imgComponent != null)
                {
                    imgComponent.sprite = ingredientSprite;
                    imgComponent.SetNativeSize();
                    
                    
                    RectTransform rect = iconObj.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        rect.sizeDelta = new Vector2(componentIconSize, componentIconSize);
                    }
                }
            }
            
            // Add tool icons
            for (int j = 0; j < required.operationIcon.Count; j++)
            {
                var toolSprite = required.operationIcon[j];
                if (toolSprite == null) continue;
                
                GameObject iconObj = Instantiate(toolIconTemplate.gameObject, newToolGroupTrans);
                iconObj.name = $"ToolIcon_{j}";
                iconObj.SetActive(true);
                
                Image imgComponent = iconObj.GetComponent<Image>();
                if (imgComponent != null)
                {
                    imgComponent.sprite = toolSprite;
                    imgComponent.SetNativeSize();
                    
                    
                    RectTransform rect = iconObj.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        rect.sizeDelta = new Vector2(componentIconSize, componentIconSize);
                    }
                }
            }
        }
        
        // Important: Hide the template objects after creating all clones
        ingredientToolGroupTemplate.gameObject.SetActive(false);
        if (ingredientIconTemplate != null) ingredientIconTemplate.gameObject.SetActive(false);
        if (toolIconTemplate != null) toolIconTemplate.gameObject.SetActive(false);
    }
    
    // Update the time slider for a specific order
    private void UpdateOrderTimer(OrderDeliveryStation.Order order)
    {
        if (activeOrderPanels.TryGetValue(order, out GameObject panel) && panel != null)
        {
            Slider timeSlider = panel.transform.Find("Progress").GetComponent<Slider>();
            if (timeSlider != null)
            {
                float timeRemaining = order.RemainingTime(Time.time);
                float timeFraction = Mathf.Clamp01(timeRemaining / order.timeLimit);
                timeSlider.value = timeFraction;
            }
        }
    }
    
    // Helper method to enable all layout groups
    private void EnableAllLayoutGroups(GameObject root)
    {
        LayoutGroup[] layoutGroups = root.GetComponentsInChildren<LayoutGroup>(true);
        foreach (var group in layoutGroups)
        {
            group.enabled = true;
        }
    }
    
    // Force layout rebuild after a small delay to ensure proper display
    private IEnumerator DelayedLayoutRebuild(GameObject panel)
    {
        // Wait for end of frame
        yield return new WaitForEndOfFrame();
        
        if (panel == null) yield break;
        
        // Enable all layout groups
        EnableAllLayoutGroups(panel);
        
        // Force canvas update
        Canvas.ForceUpdateCanvases();
        
        // Rebuild layouts
        RectTransform rect = panel.GetComponent<RectTransform>();
        if (rect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
        
        // Additional delay to ensure everything is properly set up
        yield return new WaitForEndOfFrame();
        
        // One final rebuild for good measure
        if (rect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}
