using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
<<<<<<< Updated upstream
=======

    AudioManager audioManager;

    Player player;

>>>>>>> Stashed changes
    public float TimeLeft { get; private set; }

    [SerializeField]
    float MaxTime;

<<<<<<< Updated upstream
    private void Awake()
    {
        Instance = this;
=======
    [SerializeField]
    float SlowTimeLength;

    [SerializeField]
    float SlowTimeFactor;

    bool timerActive;

    private void Awake()
    {
        Instance = this;

        player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
>>>>>>> Stashed changes

        TimeLeft = MaxTime;

        StartCoroutine(Tick());
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
    }

    IEnumerator Tick()
    {
<<<<<<< Updated upstream
        AudioManager.Instance.PlaySound("tick");
=======
        //audioManager.PlaySound("tick");
>>>>>>> Stashed changes

        yield return new WaitForSeconds(1);

        StartCoroutine(Tick());
    }

    public void StartSlowTime()
    {
        StartCoroutine(SlowTime());
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = SlowTimeFactor;

        audioManager.PauseMusic();

        yield return new WaitForSecondsRealtime(SlowTimeLength);

        audioManager.PlayMusic();

        Time.timeScale = 1;
    }
}
