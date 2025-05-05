using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanPlateStorage : Block
{
    [SerializeField] private int plateCount = 0;
    [SerializeField] private Ingredient cleanPlatePrefab;
    
    [Header("Visual")]
    [SerializeField] private Transform plateDisplayPoint;
    [SerializeField] private float plateStackHeight = 0.05f;
    [SerializeField] private int maxPlateCapacity = 5; // Maximum number of plates the storage can hold
    private List<Ingredient> displayedPlates = new List<Ingredient>();
    
    private void Start()
    {
        // Initialize the plate object pool
        InitializePlatePool();
        UpdateVisuals();
    }
    
    private void InitializePlatePool()
    {
        // Create the maximum possible number of plates in the pool
        displayedPlates = new List<Ingredient>(maxPlateCapacity);
        
        for (int i = 0; i < maxPlateCapacity; i++)
        {
            Ingredient plateVisual = Instantiate(cleanPlatePrefab, plateDisplayPoint);
            plateVisual.transform.localPosition = new Vector3(0, i * plateStackHeight, 0);
            
            
            
            
            
            // Initially hide all plates
            plateVisual.gameObject.SetActive(false);
            displayedPlates.Add(plateVisual);
        }
    }
    
    public void AddCleanPlate()
    {
        if (plateCount < maxPlateCapacity)
        {
            plateCount++;
            UpdateVisuals();
        }
        else
        {
            Debug.LogWarning("Clean plate storage is at maximum capacity");
        }
    }
    
    public override void Interact(Player player)
    {
        if (player.itemHold == null && plateCount > 0)
        {
            // Give player a clean plate (create a new instance, not from the pool)
            Ingredient cleanPlate = Instantiate(cleanPlatePrefab);
            player.SetItemHold(cleanPlate);
            
            plateCount--;
            UpdateVisuals();
        }
    }
    
    private void UpdateVisuals()
    {
        // Activate/deactivate plates based on current count
        for (int i = 0; i < displayedPlates.Count; i++)
        {
            displayedPlates[i].gameObject.SetActive(i < plateCount);
            
            if (i < plateCount)
            {
                // Ensure correct position
                displayedPlates[i].transform.localPosition = new Vector3(0, i * plateStackHeight, 0);
            }
        }
    }
}