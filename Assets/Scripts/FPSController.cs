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

    [SerializeField]
    Camera PlayerCamera;
    [SerializeField]
    float WalkSpeed = 6f;

    [SerializeField]
    float LookSpeed = .1f;
    [SerializeField]
    float LookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    // Shooting
    public delegate void Fired();
    public static event Fired onFired;

    public delegate void Reloaded();
    public static event Reloaded onReloaded;

    PlayerInput playerInput;
    InputAction moveAction, lookAction, fireAction, reloadAction;
    CharacterController characterController;

    void Awake()
    {
        playerInput = new PlayerInput();
        moveAction = playerInput.Player.Move;
        lookAction = playerInput.Player.Look;
        fireAction = playerInput.Player.Fire;
        reloadAction = playerInput.Player.Reload;

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

        fireAction.performed += OnFire;
        reloadAction.performed += OnReload;
    }

    void Update()
    {
        OnMove();
        OnLook();

        if (canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
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
    void OnLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        rotationX += -lookInput.y * LookSpeed;
        rotationX = Mathf.Clamp(rotationX, -LookXLimit, LookXLimit);

        PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, lookInput.x * LookSpeed, 0);
    }
    void OnFire(InputAction.CallbackContext callbackContext)
    {
        onFired?.Invoke();
    }
    void OnReload(InputAction.CallbackContext callbackContext)
    {
        onReloaded?.Invoke();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        fireAction.Disable();
        reloadAction.Disable();
    }
}