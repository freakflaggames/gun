using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRailTrigger : MonoBehaviour
{
    public EnemyRailCar railcar1;

    public EnemyRailCar railcar2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //if player enters start trigger, start railcar movement
        if (other.GetComponent<Player>())
        {
           railcar1.StartRide();
           railcar2.StartRide();
        }
    }
}
