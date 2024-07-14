using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    AudioManager audioManager;
    public float TimeLeft { get; private set; }

    [SerializeField]
    float MaxTime;

    bool timerActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;

        TimeLeft = MaxTime;
        timerActive = true;

        //StartCoroutine(Tick());
    }

    private void Update()
    {
        if (timerActive)
        {
            TimeLeft -= Time.deltaTime;
        }
    }

    public void StopTimer()
    {
        timerActive = false;
        audioManager.StopMusic();
    }

    IEnumerator Tick()
    {
        audioManager.PlaySound("tick");

        yield return new WaitForSeconds(1);

        if (timerActive)
        {
            StartCoroutine(Tick());
        }
    }
}
