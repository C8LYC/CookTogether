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
    [SerializeField] private float points = 10f;
    [SerializeField] private List<Required> requireds;
    
    [Serializable]
    public class Required
    {
        [SerializeField] private Sprite ingredientIcon;
        [SerializeField] private Sprite operationIcon;
    }
    
    public string Name => recipeName;
    public Sprite Icon => recipeIcon;
    public float TimeLimit => timeLimit;
    public List<Required> Requireds => requireds;
    
    public float CalculatePoints()
    {
        return points;
    }
    
    
}