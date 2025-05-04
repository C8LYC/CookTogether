using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected IngredientData heldIngredient;
    protected GameObject player;
    protected PlayerInventory playerInventory;

    [SerializeField] private GameObject ingredientIndicatorPrefab;
    private GameObject ingredientIndicator;

    public virtual void Interact()
    {
        Debug.Log($"{name} was interacted with.");
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    void Update()
    {
        // Temporary indicator for contained items
        if (heldIngredient)
        {
            if (ingredientIndicator == null && ingredientIndicatorPrefab != null)
            {
                ingredientIndicator = Instantiate(
                    ingredientIndicatorPrefab,
                    transform.position + Vector3.up * 0.7f,
                    Quaternion.identity,
                    transform
                );
                ingredientIndicator.GetComponent<Renderer>().material.color = heldIngredient.color;
            }
        }
        else
        {
            if (ingredientIndicator != null)
            {
                Destroy(ingredientIndicator);
                ingredientIndicator = null;
            }
        }
    }
}
