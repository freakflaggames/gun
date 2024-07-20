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

    void Shoot()
    {
        bool canShoot = lastShootTime + ShootDelay < Time.time && shotsLeft > 0;

        if (canShoot)
        {
            Transform camera = Camera.main.transform;

            Vector3 direction = camera.forward;
            float maxDistance = float.MaxValue;

            GameObject Bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);

            bool hitTarget = Physics.Raycast(camera.position, direction, out RaycastHit hit, maxDistance, TargetMask);

            if (hitTarget)
            {
                StartCoroutine(SpawnBullet(Bullet, hit.point, hit.normal, hit.collider.gameObject, hitTarget));
            }
            else
            {
                StartCoroutine(SpawnBullet(Bullet, camera.forward * Camera.main.farClipPlane, Vector3.zero, null, hitTarget));
            }

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

    private IEnumerator SpawnBullet(GameObject Bullet, Vector3 HitPoint, Vector3 HitNormal, GameObject other, bool MadeImpact)
    {
        Vector3 startPosition = Bullet.transform.position;

        float distance = Vector3.Distance(Bullet.transform.position, HitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            Bullet.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * BulletSpeed;

            yield return null;
        }

        Bullet.transform.position = HitPoint;

        if (MadeImpact && other)
        {
            if (other.transform.parent.tag == "Enemy")
            {
                Enemy enemy = other.transform.parent.GetComponent<Enemy>();

                bool headshot = other.tag == "Head";
                float damage = Damage * (headshot ? 2 : 1);

                enemy.Damage(damage);
            }

            var impactInstance = Instantiate(ImpactParticleSystem, HitPoint + HitNormal * 0.1f, Quaternion.LookRotation(HitNormal));
            impactInstance.transform.parent = other.transform.parent;
        }

        Destroy(Bullet);
    }

    private void OnDisable()
    {
        FPSController.onFired -= Shoot;
        FPSController.onReloaded -= Reload;
    }

}
