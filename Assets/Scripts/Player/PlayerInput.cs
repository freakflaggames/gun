using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This class handles player input.
//It is housed as a component in the Player prefab game object.

public class PlayerInput : MonoBehaviour
{
    //This class utilizes input actions from the new Unity input system.
    //You can review the bindings for each action by going to Assets/Scripts/Player/PlayerInputActions.inputactions
    PlayerInputActions input;

    //Multiple-axis inputs, like moving or looking, are represented as Vector2 values
    InputAction moveAction, lookAction;

    //Reference Player classes to communicate input to them
    PlayerController controller;
    PlayerShooting shooting;
    PlayerMelee melee;
    PlayerGrapple grapple;

    //Button inputs, like firing or reloading, are represented by input actions
    InputAction fireAction, meleeAction, dashAction, grappleAction;

    private void Awake()
    {
        //Player object should house these components for PlayerInput to communicate with.
        controller = GetComponent<PlayerController>();
        shooting = GetComponent<PlayerShooting>();
        melee = GetComponent<PlayerMelee>();
        grapple = GetComponent<PlayerGrapple>();

        //Initialize Input Action action map
        input = new PlayerInputActions();

        //Initialize Input Actions
        moveAction = input.Player.Move;
        lookAction = input.Player.Look;
        fireAction = input.Player.Fire;
        meleeAction = input.Player.Melee;
        dashAction = input.Player.Dash;
        grappleAction = input.Player.Grapple;
    }

    //Subscribe to input action events when active
    private void OnEnable()
    {
        //Start listening to movement and rotation inputs
        moveAction.Enable();
        lookAction.Enable();

        //When the fire button is pressed, tell player to shoot
        fireAction.Enable();
        if (shooting)
        {
            fireAction.performed += shooting.Shoot;
        }

        //When the melee button is pressed, tell player to melee
        meleeAction.Enable();
        if (melee)
        {
            meleeAction.performed += melee.Melee;
        }

        grappleAction.Enable();
        if (grapple)
        {
            grappleAction.performed += grapple.StartGrapple;
            grappleAction.canceled += grapple.EndGrapple;
        }

        //When the dash button is pressed, tell controller to dash
        dashAction.Enable();
        if (controller)
        {
            dashAction.performed += controller.Dash;
        }
    }

    private void Update()
    {
        //Update move and rotation input every frame for use by PlayerController
        if (controller)
        {
            controller.moveInput = moveAction.ReadValue<Vector2>();
            controller.lookInput = lookAction.ReadValue<Vector2>();
        }
    }

    //Unsubscribe to input action events when inactive
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        fireAction.Disable();
        meleeAction.Disable();
        dashAction.Disable();
        grappleAction.Disable();
    }
}
