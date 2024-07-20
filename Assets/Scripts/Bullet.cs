using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    public float BulletSpeed;
    public float BulletDamage;

    [SerializeField]
    GameObject ImpactPrefab;

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
        print("collided with " + other.gameObject.name);

        if (other.transform.parent)
        {
            GameObject otherParent = other.transform.parent.gameObject;

            Enemy enemyBehavior = otherParent.GetComponent<Enemy>();

            if (enemyBehavior)
            {
                float damageModifier = 1;

                if (other.CompareTag("Head"))
                {
                    damageModifier = 2;
                }

                enemyBehavior.Damage(BulletDamage * damageModifier);
            }
        }

        Instantiate(ImpactPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
