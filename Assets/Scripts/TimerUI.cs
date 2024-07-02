using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI SecondsText;

    [SerializeField]
    TextMeshProUGUI MillisecondsText;

    private void Update()
    {
        string time = GameManager.Instance.TimeLeft.ToString("F2");
        string[] times = time.Split('.');

        print(time);

        SecondsText.text = times[0];
        MillisecondsText.text = times[1];
    }
}
