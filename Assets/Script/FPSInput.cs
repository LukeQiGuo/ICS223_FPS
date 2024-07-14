using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour
{
    private float normalSpeed = 9.0f;
    private float runSpeed = 18.0f; // 奔跑速度
    private float gravity = -9.8f;
    private float jumpHeight = 1.0f; // 跳跃高度
    private float crouchHeight = 1.0f; // 下蹲时的高度
    private float standHeight = 2.0f; // 站立时的高度
    private float crouchSpeed = 4.5f; // 下蹲时的移动速度
    private Camera playerCamera;
    private Vector3 cameraStandPosition;
    private Vector3 cameraCrouchPosition;
    private Vector3 velocity;
    private CharacterController charController;
    private bool isCrouching = false;
    private float pushForce = 5.0f;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        cameraStandPosition = playerCamera.transform.localPosition;
        cameraCrouchPosition = new Vector3(cameraStandPosition.x, cameraStandPosition.y - 0.5f, cameraStandPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0, vertInput);
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        // 检查是否按下奔跑键 (左Shift键)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement *= runSpeed;
        }
        else if (isCrouching)
        {
            movement *= crouchSpeed;
        }
        else
        {
            movement *= normalSpeed;
        }

        // 检查是否按下下蹲键 (左Ctrl键)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }


        if (charController.isGrounded)
        {
            Debug.Log("Grounded");
            // 如果在地面上，重置y方向速度为一个小的负数
            velocity.y = -2f;

            if (Input.GetButtonDown("Jump") && !isCrouching) // 检查是否按下跳跃键 (默认是 Space 键)
            {
                Debug.Log("Jumping");
                // 使用公式计算跳跃速度
                velocity.y = Mathf.Sqrt(2 * -gravity * jumpHeight);
            }
        }
        else
        {
            // 如果不在地面上，应用重力
            velocity.y += gravity * Time.deltaTime;
        }

        // 将水平运动应用到角色控制器
        Vector3 finalMovement = transform.TransformDirection(movement) * Time.deltaTime;
        // 将垂直运动应用到角色控制器
        finalMovement.y = velocity.y * Time.deltaTime;

        charController.Move(finalMovement);
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

    private void Crouch()
    {
        charController.height = crouchHeight;
        playerCamera.transform.localPosition = cameraCrouchPosition;
        isCrouching = true;
    }

    private void StandUp()
    {
        charController.height = standHeight;
        playerCamera.transform.localPosition = cameraStandPosition;
        isCrouching = false;
    }
}
