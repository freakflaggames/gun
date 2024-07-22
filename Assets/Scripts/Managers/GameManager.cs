using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool levelComplete { get; private set; }

    AudioManager audioManager;
    public float TimeLeft { get; private set; }

    [SerializeField]
    float MaxTime;

    public bool resetBestTime;

    bool timerActive;

    bool audibleTick;

    public delegate void OnMissionComplete();
    public static event OnMissionComplete onMissionComplete;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        audioManager = AudioManager.Instance;

        TimeLeft = MaxTime;
        timerActive = true;

        if (resetBestTime)
        {
            PlayerPrefs.SetFloat("bestTime", 0);
        }

        StartCoroutine(Tick());
    }

    private void OnEnable()
    {
        Boss.onActivated += OnBossActivated;
        Player.onPlayerDeath += PlayerDied;
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

    void Score()
    {
        if (TimeLeft > PlayerPrefs.GetFloat("bestTime"))
        {
            PlayerPrefs.SetFloat("bestTime", TimeLeft);
        }
    }

    public void CompleteMission()
    {
        StopTimer();
        Score();

        onMissionComplete?.Invoke();

        levelComplete = true;
    }

    void PlayerDied()
    {
        StopTimer();
    }

    void OnBossActivated()
    {
        audioManager.StopMusic();

        audibleTick = true;
    }

    IEnumerator Tick()
    {
        if (audibleTick)
        {
            audioManager.PlaySound("tick");
        }

        yield return new WaitForSeconds(1);

        if (timerActive)
        {
            StartCoroutine(Tick());
        }
    }

    private void OnDisable()
    {
        Boss.onActivated -= OnBossActivated;
        Player.onPlayerDeath -= PlayerDied;
    }
}
