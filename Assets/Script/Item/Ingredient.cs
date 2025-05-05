using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    [SerializeField] IngredientData _ingredientData;
    public IngredientData ingredientData => _ingredientData;
    public override void Interact(Player player)
    {
        // pick up the ingredient
        
    }
    // get ingredient data by just calling the variable
   
   
}
