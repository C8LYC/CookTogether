using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPosition : MonoBehaviour
{
    public float height = 1.0f;
    void Start()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);
        position.y = height * 0.5f;

        transform.position = position;
    }
    
}
