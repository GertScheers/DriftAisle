using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownCamera : MonoBehaviour
{
    [SerializeField]
    Transform observable;
    [SerializeField]
    float aheadSpeed;
    [SerializeField]
    float followDamping;
    [SerializeField]
    float cameraHeight;

    Rigidbody _observableRigidBody;

    private void Start()
    {
        _observableRigidBody = observable.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (observable == null)
            return;

        Vector3 targetPos = observable.position + Vector3.up * cameraHeight + _observableRigidBody.velocity * aheadSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPos, followDamping * Time.deltaTime);
    }
}
