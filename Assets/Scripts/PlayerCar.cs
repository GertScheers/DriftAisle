using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    Quaternion targetRotation;
    Rigidbody _rigidBody;
    [SerializeField] float turnspeed = 5;
    [SerializeField] float acceleration = 2;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SetRotationPoint();
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnspeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {

        float accelerationInput = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
        _rigidBody.AddRelativeForce(Vector3.forward * accelerationInput);
    }

    private void SetRotationPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        
        if(plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }

}
