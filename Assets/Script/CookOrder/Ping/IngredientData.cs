using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Cooking/Ingredient", order = 1)]
public class IngredientData : ScriptableObject
{
    [SerializeField] private string ingredientName;
    [SerializeField] private Sprite icon;
    [SerializeField] private List<IngredientTransformation> possibleTransformations;

    [System.Serializable]
    public class IngredientTransformation
    {
        [SerializeField] private Operation operation;
        [FormerlySerializedAs("resultIngredient")] [SerializeField] private IngredientData resultIngredientData;

        public Operation Operation => operation;
        public IngredientData ResultIngredientData => resultIngredientData;
    }

    public string Name => ingredientName;
    public Sprite Icon => icon;
    public List<IngredientTransformation> PossibleTransformations => possibleTransformations;

    public IngredientData GetTransformationResult(Operation operation)
    {
        foreach (var transformation in possibleTransformations)
        {
            if (transformation.Operation == operation)
            {
                return transformation.ResultIngredientData;
            }
        }
        return null;
    }
}