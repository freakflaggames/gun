using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles data for the player's melee weapon.
//The enemy will take this data when calculating collision.
//This class should be housed in the Knife object which is a child of the Main Camera object.

public class MeleeWeapon : MonoBehaviour
{
    //How much damage the melee weapon does.
    public int damage;
}
