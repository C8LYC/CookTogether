using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
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

            // Interaction input , interact with the block and item first, if failed then interact with the player item, else skip
            if (Input.GetKeyDown(KeyCode.E))
            {
                Block interactable = closestBlock.GetComponent<Block>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject.GetComponent<Player>());
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
