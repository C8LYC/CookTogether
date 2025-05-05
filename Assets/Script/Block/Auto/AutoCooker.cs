using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCooker : Block
{
    private enum CookingState { Idle, Cooking, Cooked }
    private CookingState state = CookingState.Idle;
    
    [SerializeField] private float cookTime = 5.0f;
    private float cookTimer = 0f;
    
    [Header("cook")]
    [SerializeField] List<IngredientData> allowedIngredients;
    [SerializeField] List<Ingredients> currentIngredients = new List<Ingredients>();
    [SerializeField] int maxIngredients = 3;
    [SerializeField] List<cooked> cookedIngredients= new List<cooked>();

    [SerializeField] private GameObject infoUIPrefab;
    private BlockInfoUI infoUI;

    [Serializable]
    class cooked
    {
        public IngredientData resultIngredient;
        public List<Ingredients> ingredients;
        
    }

    [Serializable]
    class Ingredients
    {
        public IngredientData ingredient;
        public int amount;
    }

    public override void Interact(Player player)
    {
        switch (state)
        {
            case CookingState.Idle:
                if (player.itemHold != null)
                {
                    // Check if the item is an ingredient
                    Ingredient ingredient = player.itemHold.GetComponent<Ingredient>();
                    if (ingredient != null)
                    {
                        if(!allowedIngredients.Contains(ingredient.ingredientData)) return;
                        DestroyImmediate(player.itemHold.gameObject);
                        player.SetItemHold(null);
                        currentIngredients.Add(new Ingredients()
                        {
                            ingredient = ingredient.ingredientData,
                            amount = 1
                        });
                        StartCoroutine(CookCoroutine());
                    }
                }
                break;
            case CookingState.Cooking:
                if (player.itemHold != null)
                {
                    // Check if the item is an ingredient
                    Ingredient ingredient = player.itemHold.GetComponent<Ingredient>();
                    if (ingredient != null)
                    {
                        DestroyImmediate(player.itemHold.gameObject);
                        player.SetItemHold(null);
                        
                        // Check if the ingredient is allowed
                        if (allowedIngredients.Contains(ingredient.ingredientData))
                        {
                            // Check if we can add more ingredients
                            if (currentIngredients.Count < maxIngredients)
                            {
                                // check whether the ingredient is already in the list
                                bool found = false;
                                foreach (var currentIngredient in currentIngredients)
                                {
                                    if (currentIngredient.ingredient == ingredient.ingredientData)
                                    {
                                        currentIngredient.amount++;
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    currentIngredients.Add(new Ingredients()
                                    {
                                        ingredient = ingredient.ingredientData,
                                        amount = 1
                                    });
                                }
                            }
                            else
                            {
                                Debug.Log("Max ingredients reached.");
                            }
                        }
                        else
                        {
                            Debug.Log("Ingredient not allowed.");
                        }
                    }
                }

                break;
            case CookingState.Cooked :
                if (player.itemHold == null)
                {
                    player.SetItemHold(Instantiate(currentIngredients[0].ingredient.ingredientPrefab));
                    currentIngredients.Clear();
                }
                state = CookingState.Idle;
                break;
        }

        // switch (state)
        // {
        //     case CookingState.Idle:
        //         if (playerIngredient != null && heldIngredient == null)
        //         {
        //             heldIngredient = playerIngredient;
        //             playerInventory.Empty();
        //             StartCoroutine(CookCoroutine());
        //         }
        //         break;
        //
        //     case CookingState.Cooked:
        //         if (playerIngredient == null && heldIngredient != null)
        //         {
        //             playerInventory.Add(heldIngredient);
        //             heldIngredient = null;
        //             state = CookingState.Idle;
        //         }
        //         break;
        // }
    }

    private IEnumerator CookCoroutine()
    {
        if (infoUI == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            infoUI = Instantiate(infoUIPrefab, canvas.transform).GetComponent<BlockInfoUI>();
        }
        infoUI.target3DObject = this.gameObject.transform;
        infoUI.SetProgress(0.0f);

        state = CookingState.Cooking;
        cookTimer = 0f;

        while (cookTimer < cookTime)
        {
            cookTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(cookTimer / cookTime);
            infoUI.SetProgress(progress);

            yield return null;
        }

        Destroy(infoUI.gameObject);
        infoUI = null;

        state = CookingState.Cooked;
        cookedTransform();
    }

    private void cookedTransform()
    {
        // Transform the cooked ingredient into a new item
        // check cooked and examine the ingredients and amount meet require
        foreach (var cookedIngredient in cookedIngredients)
        {
            bool found= true;
            foreach (var ingredient in currentIngredients)
            {
                if (!cookedIngredient.ingredients.Exists((x =>
                        x.ingredient == ingredient.ingredient && x.amount == ingredient.amount)))
                {
                    found = false;
                    break;
                }
            }

            if (found == true)
            {
                currentIngredients.Clear();
                currentIngredients.Add(new Ingredients()
                {
                    ingredient = cookedIngredient.resultIngredient,
                    amount = 1
                });
            }
            else
            {
                // i don't know what it can be ...
            }
        }
    }
    private void OnValidate()
    {
        allowedIngredients.Clear();
        // automatic add all ingredient data from cookedIngredients
        foreach (var cookedIngredient in cookedIngredients)
        {
            foreach (var ingredient in cookedIngredient.ingredients)
            {
                if (!allowedIngredients.Contains(ingredient.ingredient))
                {
                    allowedIngredients.Add(ingredient.ingredient);
                }
            }
        }
        
    }
}

