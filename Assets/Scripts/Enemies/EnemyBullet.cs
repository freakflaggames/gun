using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBullet : MonoBehaviour
{
    public float BulletSpeed;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.velocity = transform.forward * BulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool persistent = false;

        if (other.gameObject.CompareTag("Player") && other.transform.parent)
        {
            GameObject parentObject = other.transform.parent.gameObject;
            Player playerBehavior = parentObject.GetComponent<Player>();

            playerBehavior.Die();

            persistent = true;
        }
        if (!persistent)
        {
            Destroy(gameObject);
        }
    }
}
