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
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
    }
}
