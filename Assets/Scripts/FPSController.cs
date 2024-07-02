using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    PlayerInput playerInput;
    InputAction MoveAction, LookAction;
    CharacterController characterController;
    void Awake()
    {
        playerInput = new PlayerInput();
        MoveAction = playerInput.Player.Move;
        LookAction = playerInput.Player.Look;

        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        MoveAction.Enable();
        LookAction.Enable();
    }

    void Update()
    {
        OnMove();
        OnLook();

        characterController.Move(moveDirection * Time.deltaTime);
    }
    void OnMove()
    {
        Vector2 input = playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = walkSpeed * input.y;
        float curSpeedY = walkSpeed * input.x;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

    }
    void OnLook()
    {
        Vector2 lookInput = LookAction.ReadValue<Vector2>();
        rotationX += -lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);
    }
    private void OnDisable()
    {
        MoveAction.Disable();
        LookAction.Disable();
    }
}