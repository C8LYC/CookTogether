using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private IngredientData ingredient;

    public bool Add(IngredientData ing)
    {
        if (!ingredient) {
            ingredient = ing;
            Debug.Log("You got " + ing.Name);
            return true;
        }
        return false;
    }

    public IngredientData Get()
    {
        return ingredient;
    }

    public void Empty()
    {
        Debug.Log("Emptied!");
        ingredient = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        ingredient = null;
    }
}
