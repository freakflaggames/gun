using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossUI : MonoBehaviour
{
    [SerializeField]
    GameObject BossPanel;

    [SerializeField]
    Image BossBarFill;

    [SerializeField]
    float FillTime;

    bool bossBarFilled;

    Boss boss;

    float health;
    float maxHealth;

    private void Awake()
    {
        boss = FindAnyObjectByType<Boss>();
    }

    private void Start()
    {
        if (boss)
        {
            maxHealth = boss.enemyBehavior.MaxHealth;
        }
    }

    private void Update()
    {
        if (boss)
        {
            health = boss.enemyBehavior.health;

            UpdateBossBar();
        }
    }

    private void OnEnable()
    {
        Boss.onActivated += RevealBossPanel;
        Player.onPlayerDeath += HideBossPanel;
    }

    void RevealBossPanel()
    {
        BossPanel.SetActive(true);

        StartCoroutine(FillBossBar());
    }

    void HideBossPanel()
    {
        BossPanel.SetActive(false);
    }

    IEnumerator FillBossBar()
    {
        BossBarFill.DOFillAmount(1, FillTime);

        yield return new WaitForSeconds(FillTime);

        bossBarFilled = true;
    }

    void UpdateBossBar()
    {
        if (bossBarFilled)
        {
            BossBarFill.fillAmount = health / maxHealth;
        }
    }

    private void OnDisable()
    {
        Boss.onActivated -= RevealBossPanel;
        Player.onPlayerDeath += HideBossPanel;
    }
}
