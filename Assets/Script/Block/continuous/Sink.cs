using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sink : Block
{
    private enum WashingState { Idle, Washing }
    private WashingState state = WashingState.Idle;
    [SerializeField] private float washTime = 5.0f;
    private float washTimer = 0f;
    private float savedProgress = 0f;
    private Coroutine washCoroutine;
    private bool isWashingPaused = false;

    [Header("Wash")]
    [SerializeField] private IngredientData allowedPlate; // Single item type for dirty plates
    [SerializeField] private int dirtyPlateCount = 0; // Count of plates being washed

    

    [Header("Output")]
    [SerializeField] private CleanPlateStorage cleanPlateStorage;

    public override void Interact(Player player)
    {
        switch (state)
        {
            case WashingState.Idle:
                if (player.itemHold != null)
                {
                    // Check if holding a dirty plate
                    if(player.itemHold.ConvertTo<Ingredient>().ingredientData == allowedPlate)
                    {
                        // Take the plate from player
                        DestroyImmediate(player.itemHold.gameObject);
                        player.SetItemHold(null);
                        // Increment dirty plate count
                        dirtyPlateCount++;
                        // Start washing
                        StartWashing(player);
                    }
                }
                else if (dirtyPlateCount>0)
                {
                    // If player interacts without holding anything, resume washing
                    StartWashing(player);
                }
                break;
            case WashingState.Washing:
                // If player interacts during washing, pause it
                PauseWashing();
                // Allow player to move again
                player.RestrictMovement(false);
                break;
        }
    }

    private void StartWashing(Player player)
    {
        state = WashingState.Washing;
        player.RestrictMovement(true);
        isWashingPaused = false;
        washCoroutine = StartCoroutine(WashCoroutine(player));
    }

    private void PauseWashing()
    {
        if (washCoroutine != null)
        {
            StopCoroutine(washCoroutine);
            washCoroutine = null;
            savedProgress = washTimer;
            isWashingPaused = true;
            state = WashingState.Idle;
            
        }
    }

    

    private IEnumerator WashCoroutine(Player player)
    {
        // Initialize and show progress UI
        SetInfoUIActive(true);
        infoUI.SetProgress(washTimer / washTime);

        
        
        while (washTimer < washTime)
        {
            washTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(washTimer / washTime);
            
            // Update progress bar with current completion percentage
            infoUI.SetProgress(progress);
            
            yield return null;
        }

        // Hide progress UI when washing is completed
        SetInfoUIActive(false);

        // Reset progress tracking variables
        washTimer = 0f;
        savedProgress = 0f;
        player.RestrictMovement(false);
        
        // Washing complete - transfer to storage
        TransferToStorage();
        isWashingPaused = false;
        
        // Reset state
        state = WashingState.Idle;
    }

    private void TransferToStorage()
    {
        if (cleanPlateStorage != null && dirtyPlateCount > 0)
        {
            // Add clean plate to storage
            cleanPlateStorage.AddCleanPlate();
            // Decrement dirty plate count
            dirtyPlateCount--;
        }
        else
        {
            Debug.LogWarning("Clean plate storage not assigned or no dirty plates in sink");
        }
    }
}
