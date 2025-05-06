using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlateReturnArea : Block
{
    [SerializeField] private int dirtyPlateCount = 0;
    [SerializeField] private Ingredient dirtyPlatePrefab;
    
    [Header("Visual")]
    [SerializeField] private Transform plateDisplayPoint;
    [SerializeField] private float plateStackHeight = 0.05f;
    [SerializeField] private int maxPlateCapacity = 10;
    
    private List<Ingredient> displayedPlates = new List<Ingredient>();
    
    private void Start()
    {
        InitializePlatePool();
        UpdateVisuals();
    }
    
    private void InitializePlatePool()
    {
        // Create plate pool up to max capacity
        displayedPlates = new List<Ingredient>(maxPlateCapacity);
        for (int i = 0; i < maxPlateCapacity; i++)
        {
            Ingredient plateVisual = Instantiate(dirtyPlatePrefab, plateDisplayPoint);
            plateVisual.transform.localPosition = new Vector3(0, i * plateStackHeight, 0);
            plateVisual.gameObject.SetActive(false);
            displayedPlates.Add(plateVisual);
        }
    }
    
    public void AddDirtyPlate()
    {
        if (dirtyPlateCount < maxPlateCapacity)
        {
            dirtyPlateCount++;
            UpdateVisuals();
        }
        else
        {
            Debug.LogWarning("Dirty plate area is at maximum capacity");
        }
    }
    
    public override void Interact(Player player)
    {
        if (player.itemHold == null && dirtyPlateCount > 0)
        {
            // Give player a dirty plate
            Ingredient dirtyPlate = Instantiate(dirtyPlatePrefab);
            player.SetItemHold(dirtyPlate);
            dirtyPlateCount--;
            UpdateVisuals();
        }
    }
    
    private void UpdateVisuals()
    {
        for (int i = 0; i < displayedPlates.Count; i++)
        {
            displayedPlates[i].gameObject.SetActive(i < dirtyPlateCount);
            if (i < dirtyPlateCount)
            {
                displayedPlates[i].transform.localPosition = new Vector3(0, i * plateStackHeight, 0);
            }
        }
    }
}
