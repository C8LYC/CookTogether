using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum OperationType
    {
        Auto,       // Operates automatically once started (cooking) : time limited 
        Continuous, // Requires continuous player interaction (chopping) : time limited and need to be held
        Instant,    // Completes immediately (picking up) : instantaneous
        Control     // Controls a mechanism (levers, buttons): no time limited 
    }

    [SerializeField] protected GameObject infoUIPrefab;
    protected BlockInfoUI infoUI;

    protected void SetInfoUIActive(bool active) 
    {
        if (infoUI == null)
        {
            if (infoUIPrefab == null)
            {
                Debug.Log("infoUIPrefab is not assigned.");
                return;
            }

            Canvas canvas = FindFirstObjectByType<Canvas>();
            infoUI = Instantiate(infoUIPrefab, canvas.transform).GetComponent<BlockInfoUI>();
            infoUI.target3DObject = this.gameObject.transform;
            return;
        }
        infoUI.gameObject.SetActive(active);
    }
    
    
    public virtual void Interact(Player player)
    {
        Debug.Log($"{name} was interacted with.");
    }
}
