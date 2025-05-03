using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCooker : Block
{
    private enum CookingState { Idle, Cooking, Cooked }
    private CookingState state = CookingState.Idle;

    [SerializeField] private float cookTime = 5.0f;
    [SerializeField] private Transform progressBar;

    private float cookTimer = 0f;

    public override void Interact()
    {
        PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
        IngredientData playerIngredient = playerInventory.Get();

        switch (state)
        {
            case CookingState.Idle:
                if (playerIngredient != null && heldIngredient == null)
                {
                    heldIngredient = playerIngredient;
                    playerInventory.Empty();
                    StartCoroutine(CookCoroutine());
                }
                break;

            case CookingState.Cooked:
                if (playerIngredient == null && heldIngredient != null)
                {
                    playerInventory.Add(heldIngredient);
                    heldIngredient = null;
                    state = CookingState.Idle;
                    ResetProgressBar();
                }
                break;
        }
    }

    private IEnumerator CookCoroutine()
    {
        state = CookingState.Cooking;
        cookTimer = 0f;

        while (cookTimer < cookTime)
        {
            cookTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(cookTimer / cookTime);

            if (progressBar != null)
            {
                progressBar.localScale = new Vector3(progress, 1f, 1f);
            }

            yield return null;
        }

        state = CookingState.Cooked;
        TransformIngredients();
    }

    private void ResetProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    private void TransformIngredients()
    {
        // Change ingredient here. (raw chicken -> fried chicken, etc.)
    }
}

