using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(FPSController))]
public class Player : MonoBehaviour
{
    GameManager gameManager;

    public Gun playerGun;

    public bool isDead { get; private set; }

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDeath;

    FPSController movement;

    private void Awake()
    {
        movement = GetComponent<FPSController>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Die()
    {
        isDead = true;
        onPlayerDeath?.Invoke();

        gameManager.StopTimer();

        transform.DOLocalRotate(new Vector3(0, 0, -45), 1.5f, RotateMode.Fast).SetEase(Ease.OutBounce);
        
        movement.canMove = false;
        movement.canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
