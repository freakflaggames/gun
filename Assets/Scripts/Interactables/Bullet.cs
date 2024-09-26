using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles bullet movement and collision.
//It also handles spawning an impact vfx particle when a bullet collides somewhere.
//This class is housed on the Bullet prefab object, which is instantiated by PlayerShooting.

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    
    //How much damage the bullet will do to a target.
    public int damage;
    //How fast the bullet moves over time.
    public float speed;
    public float hitDistance;

    //VFX to spawn when bullet collides with something.
    public GameObject impactPrefab;
    GameObject impactInstance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Set bullet's velocity to move forward at its given speed.
        rb.velocity = transform.forward * speed;
    }
    private void Update()
    {
        //create a raycast hit to store raycast data
        RaycastHit hit;
        //draw a raycast forward, if hit something within hit distance store data in hit
        Physics.Raycast(transform.position, transform.forward, out hit, hitDistance);

        //if the bullet hit something and there isnt already an impact vfx, spawn one
        if (hit.collider && !impactInstance)
        {
            //set impact vfx instance to hit object so it tracks
            impactInstance = Instantiate(impactPrefab, hit.collider.transform);
            VFXBulletImpact vfx = impactInstance.GetComponent<VFXBulletImpact>();
            vfx.Initialize(hit);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //On collision with anything, destroy the bullet and spawn an impact VFX.
        //TODO: implement object pools for shooting
        Destroy(gameObject);
    }
}
