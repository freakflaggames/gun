using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //[SerializeField]
    //CinemachineVirtualCamera VirtualCamera;

    //[SerializeField]
    //float ScreenShakeAmplitude;

    //[SerializeField]
    //float ScreenShakeTime;

    //float screenShakeTimer;

    //Player player;

    //private void OnEnable()
    //{
    //    FPSController.onFired += StartScreenShake;
    //}
    //private void Awake()
    //{
    //    player = FindAnyObjectByType<Player>();
    //}
    //private void Update()
    //{
    //    ScreenShake();
    //}
    //void StartScreenShake()
    //{
    //    if (player.playerGun.shotsLeft > 0)
    //    {
    //        screenShakeTimer = ScreenShakeTime;
    //    }
    //}
    //public void ScreenShake()
    //{
    //    var noise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    //    if (screenShakeTimer > 0)
    //    {
    //        screenShakeTimer -= Time.deltaTime;
    //    }

    //    noise.m_AmplitudeGain = Mathf.Lerp(0, ScreenShakeAmplitude, screenShakeTimer / ScreenShakeTime);
    //}

    //private void OnDisable()
    //{
    //    FPSController.onFired -= StartScreenShake;
    //}
}
