using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownCamera : MonoBehaviour
{
    [SerializeField]
    Transform observable;

    Rigidbody _observableRigidBody;
    private Vector3 currentSpeed;

    private void Start()
    {
        _observableRigidBody = observable.GetComponent<Rigidbody>();
        currentSpeed = _observableRigidBody.velocity;
    }

    private void LateUpdate()
    {
        if (observable == null)
            return;

        var newPos = new Vector3(_observableRigidBody.position.x + 25, 50, 0);
        var smoothPosition = Vector3.SmoothDamp(transform.position, newPos, ref currentSpeed, 0.1f);
        transform.position = smoothPosition;
    }
}
