using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float MaxHealth;

    public float health { get; private set; }

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

    Vector3 hitPoint;
    bool willShoot;

    [Header("Detection")]

    [SerializeField]
    float AggroDistance;

    [SerializeField]
    GameObject TelegraphGraphic;

    [SerializeField]
    float TelegraphLength;

    public Player player;

    public bool canSeePlayer;
    public bool isTelegraphing;
    public bool isWaiting;

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
        CheckForShoot();
    }

    private void LateUpdate()
    {
        LookAtPlayer();
    }
    void LookForPlayer()
    {
        Vector3 playerDir = (player.transform.position - LookPoint.position).normalized;

        canSeePlayer = false;

        if (Physics.Raycast(LookPoint.position, playerDir, out RaycastHit hit, AggroDistance, TargetMask))
        {
            canSeePlayer = hit.collider.tag == "Player" && !player.isDead;

            hitPoint = hit.point;

            Debug.DrawLine(LookPoint.position, hit.point, canSeePlayer ? Color.green : Color.red);
        }

        if (canSeePlayer && !isTelegraphing && !isWaiting)
        {
            StartCoroutine(StartTelegraph());
        }
    }

    Vector3 PlayerDir()
    {
        Vector3 dir = player.transform.position - transform.position;

        if (transform.position.y <= player.transform.position.y)
        {
            dir.y = 0;
        }

        return dir;
    }
    
    void LookAtPlayer()
    {
        transform.rotation = Quaternion.LookRotation(-PlayerDir());
    }

    void CheckForShoot()
    {
        if (willShoot)
        {
            Shoot();

            StartCoroutine(WaitToShoot());

            willShoot = false;
        }
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

    void Shoot()
    {
        FPSController playerMovement = player.gameObject.GetComponent<FPSController>();

        Quaternion bulletRotation = Quaternion.LookRotation(PlayerDir());

        GameObject bulletObject = Instantiate(BulletPrefab, BulletSpawnPoint.position, bulletRotation);

        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
        bullet.BulletSpeed = BulletSpeed;

        willShoot = false;
        isTelegraphing = false;
    }

    IEnumerator StartTelegraph()
    {
        isTelegraphing = true;

        TelegraphAnimation();

        yield return new WaitForSeconds(TelegraphLength);

        willShoot = true;
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
