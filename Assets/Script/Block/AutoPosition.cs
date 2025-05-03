using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPosition : MonoBehaviour
{
    public float height = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);
        position.y = height * 0.5f;

        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
