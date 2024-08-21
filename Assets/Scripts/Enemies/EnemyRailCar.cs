using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRailCar : MonoBehaviour
{
    //where and how far does the car go
    public Vector3 Distance;

    //where did the car start
    Vector3 startPosition;

    //where will the car end up
    Vector3 endPosition;

    //how long is the enemy riding for
    public float RideLength;

    //current ride time
    float rideTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rideTime > 0)
        {
            //move car towards destination over time
            transform.position = Vector3.Lerp(endPosition, startPosition, rideTime / RideLength);

            //count down timer
            rideTime -= Time.deltaTime;
        }

    }


    public void StartRide()
    {
        //set start and end position
        startPosition = transform.position;
        endPosition = transform.position + Distance;

        //start the ride timer
        rideTime = RideLength;
    }
}
