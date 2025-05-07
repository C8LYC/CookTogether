using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderDeliveryStation : Block
{
    [SerializeField] private List<Order> orders = new List<Order>();
    [SerializeField] private DirtyPlateReturnArea dirtyPlateReturn;
    [SerializeField] private float orderCompletionDelay = 15.0f; // Delay before dirty plates return
    [System.Serializable]
    public class Order
    {
        public Recipe recipe; // ScriptableObject containing recipe info
        public float orderTime; // When the order was created
        public float timeLimit = 120.0f; // Time limit in seconds
        public bool isCompleted = false;
    
        // Calculate remaining time
        public float RemainingTime(float currentTime)
        {
            return (orderTime + timeLimit) - currentTime;
        }
    
        // Check if order has expired
        public bool IsExpired(float currentTime)
        {
            return RemainingTime(currentTime) <= 0;
        }
    }

    private Order currentOrder;
    public List<Order> Orders 
    {
        get { return orders; }
    }
    // Add a new order to the queue
    public void AddOrder(Order newOrder)
    {
        newOrder.orderTime = Time.time;
        orders.Add(newOrder);
        
        // If no current order, move to the next one
        if (currentOrder == null)
        {
            MoveToNextOrder();
        }
    }
    
    public override void Interact(Player player)
    {
        if (player.itemHold != null && currentOrder != null && !currentOrder.isCompleted)
        {
            // Check if player's dish matches the recipe result
            if (IsCorrectOrder(player.itemHold.ConvertTo<Ingredient>()))
            {
                CompleteOrder(player);
            }
            else
            {
                Debug.Log("This is not the correct order!");
            }
        }
    }
    
    private bool IsCorrectOrder(Ingredient plate)
    {
        return plate.ingredientData == currentOrder.recipe.resultMeal;
    }
    
    private void CompleteOrder(Player player)
    {
        // Take plate from player
        DestroyImmediate(player.itemHold.gameObject);
        player.SetItemHold(null);
        
        // Mark order as completed and award score
        currentOrder.isCompleted = true;
        orders.RemoveAt(0);
        Debug.Log($"Order completed! Score: {currentOrder.recipe.CalculatePoints(currentOrder.RemainingTime(Time.time))}");
        
        // Schedule dirty plate return after delay
        if (dirtyPlateReturn != null)
        {
            StartCoroutine(ScheduleDirtyPlateReturn());
        }
        
        // Move to next order
        MoveToNextOrder();
    }
    
    private IEnumerator ScheduleDirtyPlateReturn()
    {
        yield return new WaitForSeconds(orderCompletionDelay);
        dirtyPlateReturn.AddDirtyPlate();
    }
    
    private void MoveToNextOrder()
    {
        if (orders.Count > 0)
        {
            // remove first and get first one
            currentOrder = orders[0];
            
        }
        else
        {
            currentOrder = null;
        }
    }
    
    private void Update()
    {
        // Check if current order has expired
        if (currentOrder != null && !currentOrder.isCompleted)
        {
            if (currentOrder.IsExpired(Time.time))
            {
                Debug.Log("Order expired!");
                MoveToNextOrder();
            }
        }
    }

    public void OnValidate()
    {
        if (orders!=null && orders.Count > 0)
        {
            currentOrder = orders[0];
        }
    }
}
