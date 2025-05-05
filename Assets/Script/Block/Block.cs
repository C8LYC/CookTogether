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
    
    
    public virtual void Interact(Player player)
    {
        Debug.Log($"{name} was interacted with.");
    }
}
