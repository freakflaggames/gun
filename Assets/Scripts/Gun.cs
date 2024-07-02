using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Damage;

    public int MaxAmmo;

    int shotsLeft;

    public float ShootDelay;

    public float BulletSpeed;

    float lastShootTime;

    public Transform BulletSpawnPoint;

    public GameObject BulletPrefab;

    [SerializeField]
    LayerMask TargetMask;

    [SerializeField]
    ParticleSystem ImpactParticleSystem;
    private void Awake()
    {
        shotsLeft = MaxAmmo;
    }

    private void OnEnable()
    {
        FPSController.onShot += Shoot;
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

            StartCoroutine(SpawnBullet(Bullet, hit.point, hit.normal, hitTarget));

            shotsLeft--;

            lastShootTime = Time.time;
        }
    }
    private IEnumerator SpawnBullet(GameObject Bullet, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
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

        Destroy(Bullet);

        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint + HitNormal * 0.1f, Quaternion.LookRotation(HitNormal));
        }
    }
    private void OnDisable()
    {
        FPSController.onShot -= Shoot;
    }
}
