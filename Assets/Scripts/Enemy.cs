using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float MaxHealth;

    float health;

    //Shooting

    [SerializeField]
    Transform LookPoint;

    [SerializeField]
    Transform BulletSpawnPoint;

    [SerializeField]
    float BulletSpeed;

    bool willShoot;

    //Viewing + Displaying

    [SerializeField]
    float AggroDistance;

    [SerializeField]
    GameObject AlertGraphic;

    [SerializeField]
    float AlertLength;

    [SerializeField]
    GameObject TelegraphGraphic;

    [SerializeField]
    float TelegraphLength;

    [SerializeField]
    float TimeBetweenShots;

    [SerializeField]
    GameObject BulletPrefab;

    Player player;

    public bool canSeePlayer;

    [SerializeField]
    LayerMask TargetMask;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        health = MaxHealth;
    }

    private void Update()
    {
        Vector3 playerDir = (player.transform.position - LookPoint.position).normalized;

        bool seesPlayer = false;

        if (Physics.Raycast(LookPoint.position, playerDir, out RaycastHit hit, AggroDistance, TargetMask))
        {
            seesPlayer = hit.collider.tag == "Player";

            if (willShoot)
            {
                GameObject Bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnBullet(Bullet, hit.point));

                willShoot = false;
            }

            Debug.DrawLine(LookPoint.position, hit.point, seesPlayer ? Color.green : Color.red);
        }

        if (!canSeePlayer && seesPlayer)
        {
            OnSawPlayer();
        }

        canSeePlayer = seesPlayer;
    }

    private void LateUpdate()
    {
        var lookDir = player.transform.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(-lookDir);
    }

    void OnSawPlayer()
    {
        AlertPlayer();
    }

    void AlertPlayer()
    {
        AlertGraphic.SetActive(true);

        float alertBounceAmount = 0.2f;

        Vector3 startPos = AlertGraphic.transform.localPosition;
        Vector3 newPos = startPos + (transform.up * alertBounceAmount);

        AlertGraphic.transform.DOLocalMoveY(newPos.y, AlertLength)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                AlertGraphic.transform.localPosition = startPos;

                if (canSeePlayer)
                {
                    TelegraphShot();
                }

                AlertGraphic.SetActive(false);
            });
    }

    void TelegraphShot()
    {
        TelegraphGraphic.SetActive(true);

        float startScale = TelegraphGraphic.transform.localScale.x;

        Quaternion startRot = TelegraphGraphic.transform.localRotation;

        TelegraphGraphic.transform.DOLocalRotate(new Vector3(0, 0, 360), TelegraphLength, RotateMode.FastBeyond360)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                TelegraphGraphic.transform.localRotation = startRot;
            });

        TelegraphGraphic.transform.DOScale(0, TelegraphLength)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                TelegraphGraphic.transform.localScale = Vector3.one * startScale;

                willShoot = true;

                TelegraphGraphic.SetActive(false);
            });
    }

    private IEnumerator SpawnBullet(GameObject Bullet, Vector3 HitPoint)
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

        StartCoroutine(WaitToShoot());

        Destroy(Bullet);
    }

    private IEnumerator WaitToShoot()
    {
        yield return new WaitForSeconds(TimeBetweenShots);
        if (canSeePlayer)
        {
            TelegraphShot();
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioManager.Instance.PlaySound("enemyHit");
        }
    }

    void Die()
    {
        AudioManager.Instance.PlaySound("enemyKill");
        Destroy(gameObject);
    }
}
