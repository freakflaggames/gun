using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.CompleteMission();
        }
    }
}
