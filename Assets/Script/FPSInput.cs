using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour
{
    private float speed = 9.0f;
    private float gravity = -9.8f;
    private CharacterController charController;
    private float pushForce = 5.0f;

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0, vertInput);

        movement = Vector3.ClampMagnitude(movement, 1.0f);

        movement.y = gravity;

        movement *= speed;

        movement *= Time.deltaTime;

        movement = transform.TransformDirection(movement);

        charController.Move(movement);
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        // does it have a rigidbody and is Physics enabled?
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

}
