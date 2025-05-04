using UnityEngine;

public interface IOperation
{
    bool Execute(IngredientData ingredientData);
    string OperationName { get; }
    float OperationTime { get; }
    Sprite OperationIcon { get; }
    OperationType Type { get; }
}

public enum OperationType
{
    Auto,       // Operates automatically once started (cooking) : time limited 
    Continuous, // Requires continuous player interaction (chopping) : time limited and need to be held
    Instant,    // Completes immediately (picking up) : instantaneous
    Control     // Controls a mechanism (levers, buttons): no time limited 
}

public abstract class Operation : ScriptableObject, IOperation
{
    [SerializeField] private string operationName;
    [SerializeField] private float operationTime;
    [SerializeField] private Sprite operationIcon;
    [SerializeField] private OperationType operationType;

    public string OperationName => operationName;
    public float OperationTime => operationTime;
    public Sprite OperationIcon => operationIcon;
    public OperationType Type => operationType;

    public abstract bool Execute(IngredientData ingredientData);
}

[CreateAssetMenu(fileName = "NewAutoOperation", menuName = "Cooking/Operations/Auto", order = 1)]
public class AutoOperation : Operation
{
    private float currentTimer = 0f;
    private IngredientData _currentIngredientData;
    private bool isProcessing = false;

    public override bool Execute(IngredientData ingredientData)
    {
        // If we're not currently processing anything, start processing
        if (!isProcessing)
        {
            _currentIngredientData = ingredientData;
            isProcessing = true;
            currentTimer = 0f;
            return true;
        }
        // If we already have an ingredient, check if we can combine it
        else if (_currentIngredientData != null && ingredientData != null)
        {
            // Logic for combining ingredients would go here
            // This is a placeholder - you'd need to implement your combination logic
            return false;
        }
        
        return false;
    }

    // This would be called from a MonoBehaviour Update method
    public void UpdateOperation(float deltaTime)
    {
        if (isProcessing && _currentIngredientData != null)
        {
            currentTimer += deltaTime;
            
            if (currentTimer >= OperationTime)
            {
                // Process is complete, transform the ingredient
                IngredientData result = _currentIngredientData.GetTransformationResult(this);
                if (result != null)
                {
                    _currentIngredientData = result;
                }
                
                // Reset timer and processing state
                currentTimer = 0f;
                isProcessing = false;
            }
        }
    }
}

[CreateAssetMenu(fileName = "NewContinuousOperation", menuName = "Cooking/Operations/Continuous", order = 2)]
public class ContinuousOperation : Operation
{
    private float currentProgress = 0f;
    private IngredientData _currentIngredientData;
    
    public override bool Execute(IngredientData ingredientData)
    {
        // Start or continue the operation
        if (_currentIngredientData == null)
        {
            _currentIngredientData = ingredientData;
            currentProgress = 0f;
        }
        
        // Increment progress (this would be called repeatedly while player is interacting)
        currentProgress += Time.deltaTime;
        
        // Check if operation is complete
        if (currentProgress >= OperationTime)
        {
            // Complete the operation
            IngredientData result = _currentIngredientData.GetTransformationResult(this);
            _currentIngredientData = null;
            currentProgress = 0f;
            return true;
        }
        
        return false;
    }
    
    public float GetProgress()
    {
        return currentProgress / OperationTime;
    }
}

[CreateAssetMenu(fileName = "NewInstantOperation", menuName = "Cooking/Operations/Instant", order = 3)]
public class InstantOperation : Operation
{
    public override bool Execute(IngredientData ingredientData)
    {
        // Instant operations complete immediately
        return true;
    }
}
