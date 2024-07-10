using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DeathUI : MonoBehaviour
{
    [SerializeField]
    Image DamageFlash;

    [SerializeField]
    Color StartColor;

    [SerializeField]
    Color EndColor;

    [SerializeField]
    float FlashTime;

    [SerializeField]
    GameObject DeathPanel;

    private void OnEnable()
    {
        Player.onPlayerDeath += DeathScreen;
    }

    private void Awake()
    {
        foreach(Transform child in DeathPanel.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void DeathScreen()
    {
        DamageFlash.gameObject.SetActive(true);

        DamageFlash.color = StartColor;

        DamageFlash.DOColor(EndColor, FlashTime)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                RevealDeathPanel();
            });
    }

    void RevealDeathPanel()
    {
        DeathPanel.gameObject.SetActive(true);

        foreach (Transform child in DeathPanel.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        Player.onPlayerDeath -= DeathScreen;
    }
}
