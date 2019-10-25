using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellDown : MonoBehaviour
{
    public delegate void FellDownDelegate();
    public static event FellDownDelegate Died;

    BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Died();
    }

}
