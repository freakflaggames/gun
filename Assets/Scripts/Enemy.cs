using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{

    GameManager gameManager;

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
    GameObject TelegraphGraphic;

    [SerializeField]
    float TelegraphLength;

    [SerializeField]
    float TimeBetweenShots;

    [SerializeField]
    GameObject BulletPrefab;

    Player player;
    FPSController playerMovement;

    public bool canSeePlayer;
    bool isTelegraphing;
    public bool isWaiting;

    [SerializeField]
    LayerMask TargetMask;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        playerMovement = FindAnyObjectByType<FPSController>();

        gameManager = GameManager.Instance;

        health = MaxHealth;
    }

    private void Update()
    {
        Vector3 playerDir = (player.transform.position - LookPoint.position).normalized;

        canSeePlayer = false;

        if (Physics.Raycast(LookPoint.position, playerDir, out RaycastHit hit, AggroDistance, TargetMask))
        {
<<<<<<< Updated upstream
            seesPlayer = hit.collider.tag == "Player";
=======
            canSeePlayer = hit.collider.tag == "Player" && !player.isDead;
>>>>>>> Stashed changes

            if (willShoot && isTelegraphing)
            {
                print("shot");

                GameObject Bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
<<<<<<< Updated upstream
                StartCoroutine(SpawnBullet(Bullet, hit.point));
=======

                bool hitPlayer = canSeePlayer && !playerMovement.isDashing;
>>>>>>> Stashed changes

                StartCoroutine(SpawnBullet(Bullet, hit.point, hitPlayer));

                StartCoroutine(WaitToShoot());

                print("ended telegraph");
                willShoot = false;
                isTelegraphing = false;
            }

            Debug.DrawLine(LookPoint.position, hit.point, canSeePlayer ? Color.green : Color.red);
        }

        if (canSeePlayer && !isTelegraphing && !isWaiting)
        {
            StartCoroutine(TelegraphShot());
            isTelegraphing = true;
        }
<<<<<<< Updated upstream

        canSeePlayer = seesPlayer;
    }

    private void LateUpdate()
    {
        var lookDir = player.transform.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(-lookDir);
    }

    void OnSawPlayer()
=======
    }

    IEnumerator TelegraphShot()
>>>>>>> Stashed changes
    {
        print("started telegraph");
        TelegraphAnimation();

        yield return new WaitForSeconds(TelegraphLength);

        willShoot = true;
    }

<<<<<<< Updated upstream
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
=======
    void TelegraphAnimation()
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                TelegraphGraphic.transform.localScale = Vector3.one * startScale;

                willShoot = true;

                TelegraphGraphic.SetActive(false);
            });
    }

    private IEnumerator SpawnBullet(GameObject Bullet, Vector3 HitPoint)
=======
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
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        StartCoroutine(WaitToShoot());
=======
        if (hitPlayer)
        {
            player.Die();
        }
>>>>>>> Stashed changes

        Destroy(Bullet);
    }

    private IEnumerator WaitToShoot()
    {
        isWaiting = true;

        yield return new WaitForSeconds(TimeBetweenShots);
<<<<<<< Updated upstream
        if (canSeePlayer)
        {
            TelegraphShot();
        }
=======

        isWaiting = false;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        TelegraphGraphic.transform.DOKill();

>>>>>>> Stashed changes
        AudioManager.Instance.PlaySound("enemyKill");
        Destroy(gameObject);
    }
}
