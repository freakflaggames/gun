using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles enemy detection of player and shooting bullets at the player.
//It also handles the warning telegraph before shooting.
//This class should be housed on the Enemy prefab object.

public class EnemyShooting : MonoBehaviour
{
    //The enemy is going to use a player class to find player position
    public PlayerController player;
    //This is how far the enemy can see
    public float visibilityRange;
    //This is how long it takes for the enemy to shoot
    public float shootDelay;
    //This is how much time until the enemy shoots
    public float shootTimer;
    //This is where the bullet comes from, it should be assigned to "Gun" child object
    public Transform bulletSpawnPosition;
    public GameObject bulletPrefab;

    private void Awake()
    {
        //Locate a player class to use to find player position
        player = FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();

        shootTimer = shootDelay;
    }

    private void Update()
    {
        //if shoot timer is not 0, count it down over time
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        //if shoot timer hits 0 and enemy can see player, shoot
        else 
        {
            if (CanSeePlayer())
            {
                Shoot();
            }

            //reset the shoot timer 
            shootTimer = shootDelay;
        }
    }

    public void Shoot()
    {
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        //Spawn a bullet at the enemys hip facing outwards
        Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
    }

    //This is true when the player enters the enemy's visibility range uncovered
    public bool CanSeePlayer()
    {
        //Get difference betwen player (destination) and enemy (origin) to find direction
        Vector3 relativePos = player.transform.position - transform.position;
        //Create a raycast hit to store raycast data
        RaycastHit hit;
        //Draw a raycast towards the player and store the data in hit
        Physics.Raycast(transform.position, relativePos, out hit, visibilityRange);
        //The enemy can see the player if raycast hits player object
        if (hit.collider)
        {
            return hit.collider.gameObject.CompareTag("Player");
        }
        else
        {
            return false;
        }
    }
}
