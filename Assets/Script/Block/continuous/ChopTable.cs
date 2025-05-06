using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTable : Block
{
    private enum ChoppingState { Idle, Paused, Chopping, Chopped }
    private ChoppingState state = ChoppingState.Idle;
    
    [SerializeField] private float chopTime = 5.0f;
    private float chopTimer = 0f;
    private float savedProgress = 0f;
    
    [Header("Chop")]
    [SerializeField] List<IngredientData> allowedIngredients;
    [SerializeField] List<Ingredients> currentIngredients = new List<Ingredients>();
    [SerializeField] int maxIngredients = 3;
    [SerializeField] List<Chopped> choppedIngredients = new List<Chopped>();
    
    private Coroutine chopCoroutine;
    // private bool isChoppingPaused = false;
    
    [Serializable]
    class Chopped
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
            case ChoppingState.Idle:
                if (player.itemHold != null)
                {
                   
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
                        
                        player.RestrictMovement(true);
                        
                        
                        StartChopping(player);
                    }
                }

                break;

            case ChoppingState.Paused:
                player.RestrictMovement(true);
                StartChopping(player);
                break;
                
            case ChoppingState.Chopping:
                player.RestrictMovement(false);
                PauseChopping();
                break;
                
            case ChoppingState.Chopped:
                if (player.itemHold == null)
                {
                    player.SetItemHold(Instantiate(currentIngredients[0].ingredient.ingredientPrefab));
                    currentIngredients.Clear();
                    state = ChoppingState.Idle;
                    savedProgress = 0f;
                    player.RestrictMovement(false);
                }
                break;
        }
    }
    
    private void StartChopping(Player player)
    {
        chopTimer = state == ChoppingState.Paused ? savedProgress : 0f;
        state = ChoppingState.Chopping;
        chopCoroutine = StartCoroutine(ChopCoroutine(player));
    }
    
    private void PauseChopping()
    {
        if (chopCoroutine != null)
        {
            StopCoroutine(chopCoroutine);
            chopCoroutine = null;
            savedProgress = chopTimer;
            state = ChoppingState.Paused;
        }
    }
    
    private IEnumerator ChopCoroutine(Player player)
    {
        SetInfoUIActive(true);
        infoUI.SetProgress(chopTimer / chopTime);
        
        Debug.Log($"Starting chopping from progress: {chopTimer}/{chopTime}");
        
        while (chopTimer < chopTime)
        {
            chopTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(chopTimer / chopTime);
            infoUI.SetProgress(progress);
            
            yield return null;
        }
        
        player.RestrictMovement(false);
        state = ChoppingState.Chopped;
        savedProgress = 0f;
        SetInfoUIActive(false);
        ChoppedTransform();
    }
    
    private void ChoppedTransform()
    {
        
        foreach (var choppedIngredient in choppedIngredients)
        {
            bool found = true;
            foreach (var ingredient in currentIngredients)
            {
                if (!choppedIngredient.ingredients.Exists(x => 
                    x.ingredient == ingredient.ingredient && x.amount == ingredient.amount))
                {
                    found = false;
                    break;
                }
            }
            
            if (found)
            {
                currentIngredients.Clear();
                currentIngredients.Add(new Ingredients()
                {
                    ingredient = choppedIngredient.resultIngredient,
                    amount = 1
                });
                break;
            }
        }
    }
    
    private void OnValidate()
    {
        allowedIngredients.Clear();
        
        foreach (var choppedIngredient in choppedIngredients)
        {
            foreach (var ingredient in choppedIngredient.ingredients)
            {
                if (!allowedIngredients.Contains(ingredient.ingredient))
                {
                    allowedIngredients.Add(ingredient.ingredient);
                }
            }
        }
    }
}
