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
    Gun playerGun;

    int shotsLeft;
    int ammo;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnEnable()
    {
        FPSController.onReloaded += Reload;
        FPSController.onFired += Fired;
    }

    void Update()
    {
        playerGun = player.playerGun;
        shotsLeft = playerGun.shotsLeft;
        ammo = playerGun.ammo;

        AmmoText.text = shotsLeft + "/" + ammo;
    }

    void Reload()
    {
        StartCoroutine(FillReloadBar());
    }

    void Fired()
    {
        //Check if the gun has enough ammo left to fire
        //If not, show indicator 
        //if(shotsLeft == 0)
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
        FPSController.onFired -= Fired;
    }
}
