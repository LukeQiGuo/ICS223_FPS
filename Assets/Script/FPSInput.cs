using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour
{
    private float normalSpeed = 9.0f;
    private float runSpeed = 18.0f;
    private float gravity = -9.8f;
    private float jumpHeight = 1.0f;
    private float crouchHeight = 1.0f;
    private float standHeight = 2.0f;
    private float crouchSpeed = 4.5f;
    private Vector3 cameraStandPosition;
    private Vector3 cameraCrouchPosition;
    private Vector3 velocity;
    private CharacterController charController;
    private bool isCrouching = false;
    private float pushForce = 5.0f;
    private bool isJumping = false;
    [SerializeField] private AudioClip movementClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip hitGroundClip;
    private AudioSource audioSource;
    [SerializeField] private Animator animator;
    private float originalVolume; // 新增

    void Start()
    {
        charController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            originalVolume = audioSource.volume; // 保存原始音量
        }
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.MUTE_ALL_SOUNDS, OnMuteAllSounds);
        Messenger.AddListener(GameEvent.UNMUTE_ALL_SOUNDS, OnUnmuteAllSounds); // 新增
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.MUTE_ALL_SOUNDS, OnMuteAllSounds);
        Messenger.RemoveListener(GameEvent.UNMUTE_ALL_SOUNDS, OnUnmuteAllSounds); // 新增
    }

    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0, vertInput);
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        bool isMoving = movement.magnitude > 0;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && charController.isGrounded)
        {
            movement *= runSpeed;
            audioSource.pitch = 2.0f;
        }
        else if (isCrouching && charController.isGrounded)
        {
            movement *= crouchSpeed;
            audioSource.pitch = 1.0f;
        }
        else if (charController.isGrounded)
        {
            movement *= normalSpeed;
            audioSource.pitch = 1.0f;
        }

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
            velocity.y = -2f;

            if (isJumping)
            {
                Debug.Log("Landed, playing landing sound");
                PlayJumpSound(hitGroundClip);
                isJumping = false;
            }

            if (Input.GetButtonDown("Jump") && !isCrouching)
            {
                Debug.Log("Jumping, playing jump sound");
                velocity.y = Mathf.Sqrt(2 * -gravity * jumpHeight);
                isJumping = true;
                PlayJumpSound(jumpClip);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMovement = transform.TransformDirection(movement) * Time.deltaTime;
        finalMovement.y = velocity.y * Time.deltaTime;

        charController.Move(finalMovement);

        animator.SetFloat("moveSpeed", finalMovement.magnitude / Time.deltaTime);

        if (isMoving && charController.isGrounded && !isJumping)
        {
            if (!audioSource.isPlaying || audioSource.clip != movementClip)
            {
                PlayMovementSound(movementClip);
                Debug.Log("Playing movement sound");
            }
        }
        else if (!isMoving && charController.isGrounded && !isJumping)
        {
            if (audioSource.isPlaying && audioSource.clip == movementClip)
            {
                audioSource.Stop();
                Debug.Log("Stopping movement sound");
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    private void Crouch()
    {
        charController.height = crouchHeight;
        isCrouching = true;
    }

    private void StandUp()
    {
        charController.height = standHeight;
        isCrouching = false;
    }

    private void PlayMovementSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void PlayJumpSound(AudioClip clip)
    {
        Debug.Log("Playing jump sound: " + clip.name);
        audioSource.PlayOneShot(clip);
    }

    private void OnMuteAllSounds()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0; // 将音量设置为0
        }
    }

    private void OnUnmuteAllSounds()
    {
        if (audioSource != null)
        {
            audioSource.volume = originalVolume; // 恢复原始音量
        }
    }
}
