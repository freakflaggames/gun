using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This class handles player melee attacks.
//It is housed as a component in the Player prefab game object.
//Ensure that the Player prefab game object also houses a PlayerInput component, otherwise the script won't work.

[RequireComponent(typeof(PlayerInput))]
public class PlayerMelee : MonoBehaviour
{
    //The object that deals melee damage to enemies.
    public GameObject meleeWeapon;

    //How long the player's melee weapon is active for.
    public float swingTime;

    //How long it takes to do another melee attack.
    public float meleeDelay;

    //The last time the player has done a melee attack.
    float lastMeleeTime;

    public void Melee(InputAction.CallbackContext context)
    {
        //If the player has waited for delay, allow attacking.
        bool canMelee = Time.time > lastMeleeTime + meleeDelay;

        if (canMelee)
        {
            //If can melee and melee button is pressed, swing weapon
            StartCoroutine(SwingWeapon());
            //Set last melee time to current time so player has to wait for melee delay.
            lastMeleeTime = Time.time;
        }
    }

    IEnumerator SwingWeapon()
    {
        //Activate the melee weapon, which will handle collision and damage to enemy
        meleeWeapon.gameObject.SetActive(true);

        yield return new WaitForSeconds(swingTime);

        //Deactivate the melee weapon after swing time has passed
        meleeWeapon.gameObject.SetActive(false);
    }
}
