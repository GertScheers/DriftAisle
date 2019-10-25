using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneScript : MonoBehaviour
{
    //Event system
    public delegate void AirBorneDelegate();
    public static event AirBorneDelegate WentAirborne;
    public static event AirBorneDelegate Landed;

    BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: send event to car to tell airborne ==> acceleration disallowed
        WentAirborne();
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO: send event to car to tell no longer airbone => acceleration allowed 
        Landed();
    }
}
