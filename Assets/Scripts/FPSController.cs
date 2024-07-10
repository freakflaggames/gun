using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [SerializeField]
    Player player;

    // Movement
    public bool canMove;
    public bool canLook;

    [SerializeField]
    Camera PlayerCamera;
    [SerializeField]
    float WalkSpeed = 6f;

    [SerializeField]
    float DashSpeed = 10f;

    [SerializeField]
    float DashDistance = 3f;

    [SerializeField]
    float LookSpeed = .1f;
    [SerializeField]
    float LookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;

    Vector3 dashDirection = Vector3.zero;

    float rotationX = 0;

    // Shooting
    public delegate void Fired();
    public static event Fired onFired;

    public delegate void Reloaded();
    public static event Reloaded onReloaded;

    public delegate void Grappled();
    public static event Grappled onGrapple;

    PlayerInput playerInput;
    InputAction moveAction, lookAction, fireAction, reloadAction, dashAction, grappleAction;
    CharacterController characterController;

    void Awake()
    {
        playerInput = new PlayerInput();
        moveAction = playerInput.Player.Move;
        lookAction = playerInput.Player.Look;
        fireAction = playerInput.Player.Fire;
        reloadAction = playerInput.Player.Reload;
        dashAction = playerInput.Player.Dash;
        grappleAction = playerInput.Player.Grapple;

        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        fireAction.Enable();
        reloadAction.Enable();
        dashAction.Enable();
        grappleAction.Disable();

        fireAction.performed += OnFire;
        reloadAction.performed += OnReload;
        grappleAction.performed += OnGrapple;
        
    }

    void Update()
    {
        OnMove();
        OnLook();
        
        if (canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
            dashAction.performed += OnDash;
        }
    }

    void OnMove()
    {
        Vector2 input = playerInput.Player.Move.ReadValue<Vector2>();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float speedX = WalkSpeed * input.y;
        float speedY = WalkSpeed * input.x;

        moveDirection = (forward * speedX) + (right * speedY);

    }


    void OnDash(InputAction.CallbackContext callbackContext)
    {
        Vector2 direction = playerInput.Player.Move.ReadValue<Vector2>();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float speedX = DashSpeed * direction.y;
        float speedY = DashSpeed * direction.x;

        dashDirection = (forward * speedX) + (right * speedY);

        StartCoroutine(DashCoroutine());
        

    }

    IEnumerator DashCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + DashDistance)
        {
            characterController.Move(dashDirection * DashSpeed * Time.deltaTime);
          
            yield return null; 
        }
    }


    void OnLook()
    {
        if (canLook)
        {
            Vector2 lookInput = lookAction.ReadValue<Vector2>();

            rotationX += -lookInput.y * LookSpeed;

            rotationX = Mathf.Clamp(rotationX, -LookXLimit, LookXLimit);

            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, lookInput.x * LookSpeed, 0);
        }
    }

    void OnGrapple(InputAction.CallbackContext callbackContext)
    {
        onGrapple?.Invoke();
    }

    void OnFire(InputAction.CallbackContext callbackContext)
    {
        if (!player.isDead)
        {
            onFired?.Invoke();
        }
    }

    void OnReload(InputAction.CallbackContext callbackContext)
    {
        if (!player.isDead)
        {
            onReloaded?.Invoke();
        }
    }

  
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        fireAction.Disable();
        reloadAction.Disable();
        dashAction.Disable();
        grappleAction.Disable();
    }
}