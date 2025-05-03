using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float velocityLerpSpeed = 8f;
    // public float rotationLerpSpeed = 8f; // Controls how quickly the player rotates

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 targetMoveInput;

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
        moveInput = Vector3.Lerp(moveInput, targetMoveInput, velocityLerpSpeed * Time.fixedDeltaTime);

        // Move
        Vector3 moveVelocity = moveInput * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

        // Rotate towards movement direction if moving
        // if (moveInput.sqrMagnitude > 0.01f)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(moveInput);
        //     Quaternion smoothRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationLerpSpeed * Time.fixedDeltaTime);
        //     rb.MoveRotation(smoothRotation);
        // }
    }
}
