using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputActions : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float sprintSpeed = 12f;
    public float mouseSensitivity = 0.1f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;
    public float damage = 15f;
    public float range = 8f;
    public ParticleSystem muzzleFlash;

    bool isGrounded;
    Vector3 velocity;

    public CharacterController characterController;
    private Camera playerCamera;

    private Vector2 movement;
    private Vector2 look;
    private bool isSprinting;
    private bool isJumping;
    private float xRotation;
    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Looking();
        Moving();
    }
    public void OnMovement(InputAction.CallbackContext cntxt)
    {
        movement=cntxt.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext cntxt)
    {
        look = cntxt.ReadValue<Vector2>();

    }
    public void OnShoot(InputAction.CallbackContext cntxt)
    {
        if (cntxt.started)
        {
            Shoot();
        }
    }
    public void OnJump(InputAction.CallbackContext cntxt)
    {
        if (cntxt.started)
        {
            isJumping = true;
        }
    }
    public void OnSprint(InputAction.CallbackContext cntxt)
    {
        isSprinting = cntxt.performed;
    }
    private void Looking()
    {
        if (!PauseMenu.isPaused)
        {
            float x = look.x * mouseSensitivity; float y = look.y * mouseSensitivity;
            xRotation -= y;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * x);
        }
    }

    private void Moving()
    {
        float speed = movementSpeed;
        if (isSprinting)
        {
            speed = sprintSpeed;
        }

        isGrounded=Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 moveDirections = transform.right * movement.x + transform.forward * movement.y;

        characterController.Move(moveDirections * speed * Time.deltaTime);

        if (isJumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        isJumping = false;

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    private void Shoot()
    {
        RaycastHit hit;
        if (!PauseMenu.isPaused)
        {
            muzzleFlash.Play();
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
