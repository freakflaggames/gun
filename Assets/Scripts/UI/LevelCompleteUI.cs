using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField]
    GameObject WinPanel;

    [SerializeField]
    TextMeshProUGUI ScoreText;

    private void OnEnable()
    {
        GameManager.onMissionComplete += RevealWinPanel;
    }

    void RevealWinPanel()
    {
        string score = PlayerPrefs.GetFloat("bestTime").ToString("F2");

        ScoreText.text = score;

        WinPanel.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.onMissionComplete -= RevealWinPanel;
    }
}
