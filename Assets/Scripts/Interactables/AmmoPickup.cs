using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    Player player;
    public int AmmoToAdd;



    private void Awake()
    {
        player = FindAnyObjectByType<Player>().GetComponent<Player>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.AddAmmo(AmmoToAdd);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}