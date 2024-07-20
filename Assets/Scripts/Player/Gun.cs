using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Damage;

    public int MaxMagazine;

    public int MaxAmmo;

    public int ammo { get; private set; }

    public int shotsLeft { get; private set; }

    public float ShootDelay;

    public float ReloadDelay;

    float lastShootTime;

    public float BulletSpeed;

    public Transform BulletSpawnPoint;

    public GameObject BulletPrefab;

    [SerializeField]
    LayerMask TargetMask;

    [SerializeField]
    ParticleSystem ImpactParticleSystem;
    private void Awake()
    {
        ammo = MaxAmmo;
        shotsLeft = MaxMagazine;
    }

    private void OnEnable()
    {
        FPSController.onFired += Shoot;
        FPSController.onReloaded += Reload;
    }
    private void Update()
    {

    }
    public void AddAmmo(int ammoCount)
    {
        ammo += ammoCount;
    }

    void Shoot()
    {
        bool canShoot = lastShootTime + ShootDelay < Time.time && shotsLeft > 0;

        if (canShoot)
        {
            Transform camera = Camera.main.transform;

            //if (Physics.Raycast(camera, camera.forward, ))

            //SpawnBullet(BulletPrefab, BulletSpawnPoint.transform, lookPos);

            AudioManager.Instance.PlaySound("gunshot");

            shotsLeft--;

            lastShootTime = Time.time;
        }
    }

    void Reload()
    {
        StartCoroutine(WaitToReload());
    }

    IEnumerator WaitToReload()
    {
        yield return new WaitForSeconds(ReloadDelay);

        if (shotsLeft != MaxMagazine && ammo > 0)
        {
            int bulletsToReload = MaxMagazine - shotsLeft;
            ammo -= bulletsToReload;

            if (ammo < 0)
            {
                bulletsToReload += ammo;
                ammo = 0;
            }

            shotsLeft += bulletsToReload;
        }
    }

    void SpawnBullet(GameObject bulletPrefab, Transform bulletSpawnPosition, Vector3 lookPos)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity);

        bulletObject.transform.LookAt(lookPos);

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.BulletSpeed = BulletSpeed;
        bullet.BulletDamage = Damage;
    }

    private void OnDisable()
    {
        FPSController.onFired -= Shoot;
        FPSController.onReloaded -= Reload;
    }

}
