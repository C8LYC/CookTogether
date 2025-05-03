using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject highlightBoxPrefab;
    public float interactRange = 1.5f;

    private GameObject highlightBox;
    private GameObject[] blocks;
    private GameObject closestBlock;

    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
    }

    void Update()
    {
        FindClosestBlock();

        // Update highlight box position
        if (closestBlock)
        {
            if (!highlightBox)
            {
                highlightBox = Instantiate(highlightBoxPrefab);
            }
            highlightBox.SetActive(true);
            highlightBox.transform.position = closestBlock.transform.position;

            // Interaction input
            if (Input.GetKeyDown(KeyCode.E))
            {
                Block interactable = closestBlock.GetComponent<Block>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
                else
                {
                    Debug.LogWarning("Doesn't have \"Block\" script");
                }
            }
        }
        else
        {
            if (highlightBox)
            {
                highlightBox.SetActive(false);
            }
        }
    }

    void FindClosestBlock()
    {
        closestBlock = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject block in blocks)
        {
            float distance = Vector3.Distance(transform.position, block.transform.position);
            if (distance < interactRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestBlock = block;
            }
        }
    }
}
