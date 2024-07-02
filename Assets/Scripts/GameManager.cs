using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float TimeLeft { get; private set; }

    [SerializeField]
    float MaxTime;

    private void Awake()
    {
        Instance = this;

        TimeLeft = MaxTime;

        StartCoroutine(Tick());
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
    }

    IEnumerator Tick()
    {
        AudioManager.Instance.PlaySound("tick");

        yield return new WaitForSeconds(1);

        StartCoroutine(Tick());
    }
}
