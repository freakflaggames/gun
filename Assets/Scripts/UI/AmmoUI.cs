using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI AmmoText;

    [SerializeField]
    Image ReloadBar;

    Player player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnEnable()
    {
        FPSController.onReloaded += Reload;
    }

    void Update()
    {
        Gun playerGun = player.playerGun;
        int shotsLeft = playerGun.shotsLeft;
        int ammo = playerGun.ammo;
        AmmoText.text = shotsLeft + "/" + ammo;
    }

    void Reload()
    {
        StartCoroutine(FillReloadBar());
    }

    IEnumerator FillReloadBar()
    {
        float reloadTime = player.playerGun.ReloadDelay;
        float timer = 0;

        while (timer < reloadTime)
        {
            ReloadBar.fillAmount = timer / reloadTime;
            timer += Time.deltaTime;
            yield return null;
        }

        ReloadBar.fillAmount = 0;
    }
    private void OnDisable()
    {
        FPSController.onReloaded -= Reload;
    }
}
