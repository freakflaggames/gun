using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float MaxHealth;

    float health;

    [Header("Shooting")]

    [SerializeField]
    Transform LookPoint;

    [SerializeField]
    Transform BulletSpawnPoint;

    [SerializeField]
    float BulletSpeed;

    [SerializeField]
    float TimeBetweenShots;

    [SerializeField]
    GameObject BulletPrefab;

    bool willShoot;

    [Header("Detection")]

    [SerializeField]
    float AggroDistance;

    [SerializeField]
    GameObject TelegraphGraphic;

    [SerializeField]
    float TelegraphLength;

    Player player;

    public bool canSeePlayer;
    bool isTelegraphing;
    bool isWaiting;

    [SerializeField]
    LayerMask TargetMask;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        health = MaxHealth;
    }

    private void Update()
    {
        LookForPlayer();
    }

    private void LateUpdate()
    {
        var lookDir = player.transform.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(-lookDir);
    }
    void LookForPlayer()
    {
        Vector3 playerDir = (player.transform.position - LookPoint.position).normalized;

        canSeePlayer = false;

        if (Physics.Raycast(LookPoint.position, playerDir, out RaycastHit hit, AggroDistance, TargetMask))
        {
            canSeePlayer = hit.collider.tag == "Player" && !player.isDead;

            if (willShoot)
            {
                Shoot(hit.point);
            }

            Debug.DrawLine(LookPoint.position, hit.point, canSeePlayer ? Color.green : Color.red);
        }

        if (canSeePlayer && !isTelegraphing && !isWaiting)
        {
            StartCoroutine(StartTelegraph());
        }
    }

    void Shoot(Vector3 hitPoint)
    {
        FPSController playerMovement = player.gameObject.GetComponent<FPSController>();

        bool hitPlayer = canSeePlayer && !playerMovement.isDashing;

        GameObject Bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);

        StartCoroutine(SpawnBullet(Bullet, hitPoint, hitPlayer));

        StartCoroutine(WaitToShoot());

        willShoot = false;
    }

    IEnumerator StartTelegraph()
    {
        isTelegraphing = true;

        TelegraphAnimation();

        yield return new WaitForSeconds(TelegraphLength);

        willShoot = true;
        isTelegraphing = false;
    }

    void TelegraphAnimation()
    {
        TelegraphGraphic.SetActive(true);

        float startScale = TelegraphGraphic.transform.localScale.x;
        Quaternion startRot = TelegraphGraphic.transform.localRotation;

        //telegraph zoom in animation

        TelegraphGraphic.transform.DOLocalRotate(new Vector3(0, 0, 360), TelegraphLength, RotateMode.FastBeyond360)
            .SetEase(Ease.OutSine);

        TelegraphGraphic.transform.DOScale(0, TelegraphLength)
            .SetEase(Ease.OutSine)
            .OnKill(() =>
            {
                ResetTelegraph(startScale, startRot);
            })
            .OnComplete(() =>
            {
                ResetTelegraph(startScale, startRot);
            });
    }

    void ResetTelegraph(float startScale, Quaternion startRot)
    {
        TelegraphGraphic.transform.localRotation = startRot;
        TelegraphGraphic.transform.localScale = Vector3.one * startScale;

        TelegraphGraphic.SetActive(false);
    }

    private IEnumerator SpawnBullet(GameObject Bullet, Vector3 HitPoint, bool hitPlayer)
    {
        Vector3 startPosition = Bullet.transform.position;

        float distance = Vector3.Distance(Bullet.transform.position, HitPoint);
        float startingDistance = distance;

        AudioManager.Instance.PlaySound("gunshot");

        while (distance > 0)
        {
            Bullet.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * BulletSpeed;

            yield return null;
        }

        Bullet.transform.position = HitPoint;

        if (hitPlayer)
        {
            player.Die();
        }

        Destroy(Bullet);
    }

    private IEnumerator WaitToShoot()
    {
        isWaiting = true;

        yield return new WaitForSeconds(TimeBetweenShots);

        isWaiting = false;
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
        TelegraphGraphic.transform.DOKill();

        AudioManager.Instance.PlaySound("enemyKill");

        Destroy(gameObject);
    }
}
