using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera fpsCamera;
    public Camera tpsCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpPower = 5f;
    public float gravity = 12f;
    public float lookSpeed = 3f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 1f;

    private float baseWalkSpeed;
    private float baseRunSpeed;

    public KeyCode pickupKey = KeyCode.E;
    public KeyCode checkInventoryKey = KeyCode.I;
    public KeyCode fireKey = KeyCode.F;

    private InventoryManager inventory;

    public AudioSource audioSource;
    public AudioClip walkingClip;
    public AudioClip runningClip;
    public AudioClip jumpClip;
    public AudioClip crouchClip;
    public AudioClip pickupClip;

    public GameObject campfirePrefab;
    public float spawnDistance = 2f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private bool isFpsMode = true;

    private Animator animator;
    private GameObject currentPickupItem = null;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        baseWalkSpeed = walkSpeed;
        baseRunSpeed = runSpeed;

        inventory = GetComponent<InventoryManager>();
        if (inventory == null)
        {
            Debug.LogError("InventoryManager tidak ditemukan!");
        }

        audioSource = GetComponent<AudioSource>();

        if (animator != null)
        {
            animator.SetBool("isCrouching", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // Kursor diatur oleh PauseManager, tapi inisialisasi awal di sini
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isFpsMode = true;
        fpsCamera.gameObject.SetActive(true);
        tpsCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        // =============================================================
        // INTEGRASI PAUSE: Hentikan semua input jika game sedang Pause
        // =============================================================
        if (PauseManager.isPaused)
        {
            // Matikan suara langkah jika sedang pause
            if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
            return;
        }

        // 1. Switch FPS / TPS mode
        if (Input.GetKeyDown(KeyCode.M))
        {
            isFpsMode = !isFpsMode;
            fpsCamera.gameObject.SetActive(isFpsMode);
            tpsCamera.gameObject.SetActive(!isFpsMode);
        }

        // 2. Tentukan Crouch Status
        bool isCrouching = Input.GetKey(KeyCode.R) && canMove;
        if (animator != null) animator.SetBool("isCrouching", isCrouching);

        // 3. Perhitungan Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        bool isMoving = (verticalInput != 0f || horizontalInput != 0f) && characterController.isGrounded;

        if (isCrouching)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

            if (Input.GetKeyDown(KeyCode.R) && crouchClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(crouchClip);
            }
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = baseWalkSpeed;
            runSpeed = baseRunSpeed;
        }

        // ===== ANIMASI DAN KECEPATAN =====
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && !isCrouching;

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isWalking", isMoving && !isRunning);
            animator.SetBool("isGrounded", characterController.isGrounded);
            animator.SetBool("isFalling", !characterController.isGrounded && moveDirection.y < 0);
        }

        float chosenSpeed = isRunning ? runSpeed : walkSpeed;
        float curSpeedX = canMove ? chosenSpeed * verticalInput : 0;
        float curSpeedY = canMove ? chosenSpeed * horizontalInput : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // 4. Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching)
        {
            moveDirection.y = jumpPower;
            if (animator != null) animator.SetTrigger("Jump");

            if (jumpClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpClip);
            }
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // 5. Gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // 6. Sound Logic
        if (isMoving && !isCrouching)
        {
            AudioClip clipToPlay = isRunning ? runningClip : walkingClip;

            if (audioSource != null && audioSource.clip != clipToPlay)
            {
                audioSource.clip = clipToPlay;
                audioSource.loop = true;
                audioSource.Play();
            }
            else if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // 7. Move Execution
        characterController.Move(moveDirection * Time.deltaTime);

        // 8. Look controls
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            if (isFpsMode)
            {
                fpsCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            }
        }

        // 9. Interaction Logic
        if (Input.GetKeyDown(checkInventoryKey)) inventory?.CheckInventory();

        if (currentPickupItem != null && Input.GetKeyDown(pickupKey))
        {
            ProcessPickup(currentPickupItem);
            currentPickupItem = null;
        }

        if (Input.GetKeyDown(fireKey))
        {
            if (inventory != null && inventory.TryConsumeLogsForFire())
            {
                SpawnCampfire();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupItem")) currentPickupItem = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickupItem") && other.gameObject == currentPickupItem)
        {
            currentPickupItem = null;
        }
    }

    private void ProcessPickup(GameObject item)
    {
        string itemName = item.name.Replace("(Clone)", "").Trim();
        if (inventory != null) inventory.AddItem(itemName);

        if (pickupClip != null && audioSource != null) audioSource.PlayOneShot(pickupClip);
        Destroy(item);
    }

    private void SpawnCampfire()
    {
        if (campfirePrefab == null) return;
        Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
        spawnPosition.y = transform.position.y - (characterController.height / 2f);
        Instantiate(campfirePrefab, spawnPosition, Quaternion.identity);
    }
}