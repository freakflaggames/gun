using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(Collider))]
public class Railcar : MonoBehaviour
{
    //where and how far does the car go
    public Vector3 Distance;

    //where did the car start
    Vector3 startPosition;

    //where will the car end up
    Vector3 endPosition;

    //how long is the player riding for
    public float RideLength;

    //current ride time
    float rideTime;

    //time until player can remount
    public float remountTime;

    //store the passenger
    [HideInInspector] public Player passenger;

    Collider trigger;

    private void Awake()
    {
        trigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            //on colliding with player, bring them in as a passenger
            Player player = other.GetComponent<Player>();
            PassengerEnter(player);
        }
    }


    void PassengerEnter(Player player)
    {
        //store the player as a passenger
        passenger = player;

        //anchor passenger's position to the car
        passenger.transform.SetParent(transform);

        //stop passenger from moving while riding 
        passenger.movement.canMove = false;

        //set start and end position
        startPosition = transform.position;
        endPosition = transform.position + Distance;

        //start the ride timer
        rideTime = RideLength;
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
        else
        {
            if (passenger)
            {
                PassengerExit(passenger);
            }
        }
    }

    public void PassengerExit(Player passenger)
    {
        //remove anchor from passenger position
        passenger.transform.SetParent(null);

        //allow the passenger to move
        passenger.movement.canMove = true;

        //remove player as passenger
        passenger = null;

        //disable collision to prevent reentry
        trigger.enabled = false;

        //start coroutine to allow player to remount
        StartCoroutine(RemountTimer());
            
    }


    public IEnumerator RemountTimer()
    {
        //wait a fixed amount of time
        yield return new WaitForSeconds(remountTime);

        //renable collision for reentry
        trigger.enabled = true;
    }
}
