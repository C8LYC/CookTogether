using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float movementLerpSpeed = 8f;
    // public float rotationLerpSpeed = 8f; // Controls how quickly the player rotates

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 targetMoveInput;
    
    [Header("Item")]
    private Item _itemHold;
    public Item itemHold => _itemHold;
    [SerializeField] Transform itemHoldPoint;

    public void SetItemHold(Item item)
    {
        if (item != null)
        {
            item.transform.SetParent(itemHoldPoint);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            _itemHold = item;
        }
        else
        {
            _itemHold = null;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | 
                         RigidbodyConstraints.FreezeRotationX | 
                         RigidbodyConstraints.FreezeRotationZ;
    }
    private bool movementRestricted = false;

    public void RestrictMovement(bool restrict)
    {
        movementRestricted = restrict;
        if (restrict)
        {
           
            rb.velocity = Vector3.zero;
            moveInput = Vector3.zero;
            targetMoveInput = Vector3.zero;
        }
    }
    void Update()
    {
        if (!movementRestricted)
        {
            
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            
            targetMoveInput = new Vector3(x, 0f, z).normalized;
        }
        else
        {
            
            targetMoveInput = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (!movementRestricted)
        {
            moveInput = Vector3.Lerp(moveInput, targetMoveInput, movementLerpSpeed * Time.fixedDeltaTime);
            
            Vector3 moveVelocity = moveInput * moveSpeed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
           
            if (moveInput != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
                rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, movementLerpSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
}
