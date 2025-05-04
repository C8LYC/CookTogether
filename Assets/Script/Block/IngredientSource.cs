using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSource : Block
{
    public override void Interact()
    {
        playerInventory.Add(heldIngredient);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = heldIngredient.color;
    }
}
