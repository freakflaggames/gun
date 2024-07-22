using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    Boss boss;
    private void Awake()
    {
        boss = FindAnyObjectByType<Boss>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boss.ActivateBoss();
        }
    }
}
