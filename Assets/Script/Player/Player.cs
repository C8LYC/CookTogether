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
    private  Item _itemHold;
    public Item itemHold=> _itemHold;
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

    void Update()
    {
        // Get WASD / Arrow input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Calculate target speed vector
        targetMoveInput = new Vector3(x, 0f, z).normalized;
        
    }

    void FixedUpdate()
    {
        moveInput = Vector3.Lerp(moveInput, targetMoveInput, movementLerpSpeed * Time.fixedDeltaTime);

        // Move
        Vector3 moveVelocity = moveInput * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }
}
