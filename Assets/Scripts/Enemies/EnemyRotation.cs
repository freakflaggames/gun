using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a script that deals with rotating the enemy towards the player.
//It should be housed on the Enemy prefab object.

public class EnemyRotation : MonoBehaviour
{
    PlayerController player;
    private void Awake()
    {
        //Locate a player class that is housed on the player object
        player = FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (player)
        {
            LookTowardsPlayer();
        }
    }

    void LookTowardsPlayer()
    {
        //Use player transform to store player position
        Vector3 playerPos = player.transform.position;

        //Use difference of destination playerpos and origin transform position 
        //to determine angle for enemy object to rotate
        Vector3 relativePos = playerPos - transform.position;

        //Make the rotation face towards the relative pos
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        transform.rotation = rotation;
    }
}
