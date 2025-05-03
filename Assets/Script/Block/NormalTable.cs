using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTable : Block
{
    public override void Interact()
    {
        PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
        IngredientData playerIngredient = playerInventory.Get();

        if (playerIngredient && !heldIngredient)
        {
            heldIngredient = playerIngredient;
            playerInventory.Empty();
        }
        else if (!playerIngredient && heldIngredient)
        {
            playerInventory.Add(heldIngredient);
            heldIngredient = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        heldIngredient = null;
    }
}
