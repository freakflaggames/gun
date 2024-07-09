using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Railcar : MonoBehaviour
{
    [SerializeField]
    Vector3 StartPoint;

    [SerializeField]
    Vector3 EndPoint;

    [SerializeField]
    float RideTime;

    [SerializeField]
    Ease ease;

    bool end;

    Transform passenger;

    Vector3 seatedPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            passenger = other.transform;
            PassengerEnter();
        }
    }
    private void Update()
    {
        if (passenger)
        {
            seatedPosition = transform.position;
            seatedPosition.y = passenger.transform.position.y;

            passenger.transform.position = seatedPosition;
        }
    }

    void PassengerEnter()
    {
        passenger.GetComponent<FPSController>().canMove = false;
        StartCar();
    }

    void PassengerExit()
    {
        passenger.GetComponent<FPSController>().canMove = true;
        passenger = null;
    }

    void StartCar()
    {
        Vector3 destination = end ? StartPoint : EndPoint;
        transform.parent.DOLocalMove(destination, RideTime)
            .SetEase(ease)
            .OnComplete(() =>
            {
                end = !end;
                PassengerExit();
            });
    }
}
