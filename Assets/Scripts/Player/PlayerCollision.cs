using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles what happens when the player collides with objects
//It should be housed in the Body child object of the Player prefab object

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>())
        {
            print("shot!");
        }
    }
}
