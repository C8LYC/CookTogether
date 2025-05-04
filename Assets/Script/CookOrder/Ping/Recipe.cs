using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Cooking/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [SerializeField] private string recipeName;
    [SerializeField] private Sprite recipeIcon;
    [SerializeField] private float timeLimit = 60f;
    [SerializeField] private List<Required> requireds;
    
    [Serializable]
    public class Required
    {
        [SerializeField] private IngredientData requiredIngredient;
        [SerializeField] private Operation requiredOperation;
        
        public IngredientData RequiredIngredientData => requiredIngredient;
        public Operation RequiredOperation => requiredOperation;
    }
    
    public string Name => recipeName;
    public Sprite Icon => recipeIcon;
    public float TimeLimit => timeLimit;
    public List<Required> Requireds => requireds;
    
    public float CalculatePoints()
    {
        // This could be expanded to calculate points based on recipe complexity,
        // time taken, etc.
        return 10f;
    }
    
    
}