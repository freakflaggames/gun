using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This class handles player shooting.
//It is housed as a component in the Player prefab game object.
//Ensure that the Player prefab game object also houses a PlayerInput component, otherwise shooting won't work.

[RequireComponent(typeof(PlayerInput))]
public class PlayerShooting : MonoBehaviour
{
    //How many shots the player has.
    public int ammo;
    //The maximum number of shots for this gun.
    public int maxAmmo;

    //The bullet object that will be shot out of the gun.
    //Bullet damage and collision is handled in the bullet script.
    public GameObject bulletPrefab;
    //Where the bullet will spawn from (tip of gun barrel)
    public Transform bulletSpawnPosition;
    //What is considered a target for the bullet?
    public LayerMask bulletTarget;

    //How long it takes the player to shoot again.
    public float shootDelay;

    //The last time the player has shot.
    float lastShootTime;

    public GameObject gunObject;

    private void Awake()
    {
        ammo = maxAmmo;
    }

    //When the player presses the shoot button, PlayerInput calls this function.
    public void Shoot(InputAction.CallbackContext context)
    {
        //If the player has ammo and has waited for shoot delay, allow shooting.
        bool canShoot = ammo > 0 && Time.time > lastShootTime + shootDelay;

        if (canShoot)
        {
            //Spawn a bullet and lose one ammo.
            SpawnBullet();
            ammo--;
            //Set last shoot time to current time so player has to wait for shoot delay.
            lastShootTime = Time.time;
        }
    }
    public void SpawnBullet()
    {
        //Check if the camera is looking at a target.
        Transform camera = Camera.main.transform;

        //Create a raycast hit instance to store collision data.
        RaycastHit hit;

        //Create a rotation variable for the bullet.
        //Make its rotation face parallel to the gun barrel.
        Quaternion bulletRotation = bulletSpawnPosition.rotation;

        //Draw a raycast at the camera position that goes forward and looks for target layer.
        if (Physics.Raycast(camera.position, camera.forward, out hit, float.MaxValue, bulletTarget))
        {
            //Make the bullet shoot from the bullet spawn point to the raycast hit point
            Vector3 relativePos = hit.point - bulletSpawnPosition.position;
            //Set the bullet to rotate to face the raycast hit point
            bulletRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }

        //Spawn a bullet at the tip of the barrel and rotate it
        Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletRotation);
    }
}