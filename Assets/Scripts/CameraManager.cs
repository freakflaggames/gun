using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera VirtualCamera;

    [SerializeField]
    float ScreenShakeAmplitude;

    [SerializeField]
    float ScreenShakeFrequency;

    [SerializeField]
    float ScreenShakeTime;

    private void OnEnable()
    {
        FPSController.onFired += ScreenShake;
    }
    public void ScreenShake()
    {
        StartCoroutine(StartScreenShake());
    }

    IEnumerator StartScreenShake()
    {
        var noise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = ScreenShakeAmplitude;
        noise.m_FrequencyGain = ScreenShakeFrequency;

        yield return new WaitForSeconds(ScreenShakeTime);

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    private void OnDisable()
    {
        FPSController.onFired -= ScreenShake;
    }
}
