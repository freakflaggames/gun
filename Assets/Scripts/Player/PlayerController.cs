using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

//This class handles player rotation and movement.
//It is housed as a component in the Player prefab game object.
//Ensure that the Player prefab game object also houses a PlayerInput component, otherwise controller can't do anything.

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    //The player looks around by rotating the camera in the Y axis and the game object in the X axis.
    [Header("Rotation")]
    public Vector2 lookInput;
    public bool canLook;
    public float lookSpeed = .1f;
    public float lookXLimit = 45f;
    float rotationX = 0;
    CinemachineVirtualCamera virtualCamera;

    //The player moves using the CharacterController component.
    [Header("Movement")]
    public Vector2 moveInput;
    public bool canMove;
    public float walkSpeed = 6f;
    Vector3 moveDirection = Vector3.zero;
    CharacterController characterController;

    //Dashing is a short burst of movement in the direction the player is moving.
    [Header("Dashing")]
    public bool isDashing;
    public float dashSpeed = 20f;
    public float dashTime = .2f;
    public float dashCooldown = 1f;
    public Vector3 dashDirection = Vector3.zero;
    //This c# event messages all classes listening that a successful dash happened
    public delegate void OnDashed(Vector3 direction);
    public static OnDashed onDashed;

    void Awake()
    {

        //Player object should also house a CharacterController component for use in movement.
        characterController = GetComponent<CharacterController>();
        
        //Locate the virtual camera, which should be a child of the Player game object.
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

        //Lock and hide the cursor to allow for first person rotation.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Certain outside factors, like the player dying, entering a railcar, or completing a level, will lock movement and rotation.

        if (canLook)
        {
            Look();
        }

        if (canMove)
        {
            Move();
        }
    }
    void Look()
    {
        //lookInput is provided by PlayerInput.
        rotationX += -lookInput.y * lookSpeed;

        //Limit X axis (Y axis to player) rotation.
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        //Rotate the camera's X axis (Y axis to player)
        virtualCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        //Rotate player object's Y axis (X axis to player)
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);
    }

    void Move()
    {
        //moveInput is provided by PlayerInput. 
        Vector2 speed = moveInput * walkSpeed;

        //Player moves forward using Y axis of move input and side to side using X axis of move input.
        moveDirection = (transform.forward * speed.y) + (transform.right * speed.x);

        //Move character controller by move speed and direction over time.
        //SimpleMove applies gravity automatically.
        characterController.SimpleMove(moveDirection);
    }


    public void Dash(InputAction.CallbackContext context)
    {
        //Dash is triggered by PlayerInput.
        //Check to see if player is already dashing to prevent infinite dashes.
        if (moveInput != Vector2.zero && canMove && !isDashing)
        {
            StartCoroutine(DashCoroutine(moveInput));
        }
    }

    //DashCoroutine starts a timer and moves player by dash speed and direction until timer is up.
    IEnumerator DashCoroutine(Vector3 direction)
    {
        //message other classes that a dash event has occurred
        //pass through direction so other classes know what direction its going
        onDashed?.Invoke(direction);

        isDashing = true;

        //Make dash direction relative to player's transform.
        dashDirection = (transform.forward * direction.y) + (transform.right * direction.x);

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            //Move character controller by dash speed and direction over time until time reaches end of dash time.
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }

        //Start cooldown for dash. 
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        //Wait for the dashcooldown time before allowing a player to dash again
        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
    }
}