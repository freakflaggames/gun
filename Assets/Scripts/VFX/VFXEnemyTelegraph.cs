using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles animating the enemy's telegraph graphic.
//It should be housed on the Enemy prefab object
//This class requires communication with enemyshooting

[RequireComponent(typeof(EnemyShooting))]
public class VFXEnemyTelegraph : MonoBehaviour
{
    //This class uses the shoot system's timer to determine when the enemy will shoot
    public EnemyShooting shootSystem;
    public Transform telegraphGraphic;
    //How large the telegraph will appear at start
    public float telegraphSize;

    private void Awake()
    {
        shootSystem = GetComponent<EnemyShooting>();
    }
    private void Update()
    {
        if (shootSystem)
        {
            //Telegraph is only active when the enemy can see the player
            bool isActive = shootSystem.CanSeePlayer();
            telegraphGraphic.gameObject.SetActive(isActive);
            //Make the telegraph graphic smaller as the shoot system timer reaches 0.
            float shootPercentage = shootSystem.shootTimer / shootSystem.shootDelay;
            Vector3 size = Vector3.one * telegraphSize * shootPercentage;
            //Set scale to dynamic size
            telegraphGraphic.localScale = size;
        }
    }
}
