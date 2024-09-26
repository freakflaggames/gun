using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles what happens when something collides with an enemy
//It should be housed in the Head and Body child objects of the Enemy prefab object

public class EnemyCollision : MonoBehaviour
{
    //This class communicates with EnemyHealth to communicate damage.
    //EnemyHealth is assigned to the parent Enemy prefab object
    //Assign it in the inspector.
    public EnemyHealth health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>())
        {
            //if the enemy collides with a bullet, take the bullet's damage
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            health.TakeDamage(bullet.damage);
        }
        if (other.gameObject.GetComponent<MeleeWeapon>())
        {
            //if the enemy collides with a melee weapon, take the weapon's damage
            MeleeWeapon weapon = other.gameObject.GetComponent<MeleeWeapon>();
            health.TakeDamage(weapon.damage);
        }
    }
}
