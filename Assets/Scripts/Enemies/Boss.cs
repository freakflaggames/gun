using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(CharacterController))]
public class Boss : MonoBehaviour
{
    public Enemy enemyBehavior;
    CharacterController controller;

    Player player;

    public string bossName;

    [SerializeField]
    float ChaseSpeed;

    public bool activated { get; private set; }
    bool chasing;

    public delegate void OnActivated();
    public static event OnActivated onActivated;

    private void Awake()
    {
        enemyBehavior = GetComponent<Enemy>();
        controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        player = enemyBehavior.player;
    }
    private void Update()
    {
        if (enemyBehavior.canSeePlayer && !chasing)
        {
            chasing = true;
        }

        ChasePlayer();
    }
    void ChasePlayer()
    {
        if (chasing)
        {
            controller.Move(-transform.forward * ChaseSpeed * Time.deltaTime);
        }
    }

    public void ActivateBoss()
    {
        activated = true;
        onActivated?.Invoke();
    }
}
