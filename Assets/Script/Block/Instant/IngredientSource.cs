using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSource : Block
{
    [SerializeField] IngredientData _heldIngredient;
    
    public override void Interact(Player player)
    {
        if (player.itemHold == null)
        {
            
            Ingredient newIngredient = Instantiate(_heldIngredient.ingredientPrefab);
            player.SetItemHold(newIngredient);
        }
    }
    
    void Start()
    {
        GetComponent<Renderer>().material.color = _heldIngredient.color;
    }
}
