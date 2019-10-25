using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    Quaternion targetRotation;
    Rigidbody _rigidBody;
    [SerializeField] float turnspeed = 5;
    [SerializeField] float acceleration = 2;

    Vector3 lastPosition;
    float _sideSkidAmount = 0;
    [SerializeField] float maxSpeed;
    bool airborne;

    public float SideSkidAmount
    {
        get
        {
            return _sideSkidAmount;
        }
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        AirborneScript.WentAirborne += WentAirborne;
        AirborneScript.Landed += Landed;
        FellDown.Died += Died;
    }

    private void OnDisable()
    {
        AirborneScript.WentAirborne -= WentAirborne;
        AirborneScript.Landed -= Landed;
        FellDown.Died -= Died;
    }

    private void Landed()
    {
        airborne = false;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    private void WentAirborne()
    {
        airborne = true;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Died()
    {
        //TODO: you are now dead. Probably move this method to other controller
    }

    private void Update()
    {
        if (!airborne)
        {
            SetRotationPoint();
            SetSideSkid();
        }
    }

    private void FixedUpdate()
    {
        float speed = _rigidBody.velocity.magnitude;

        if (!airborne)
        {
            if (speed > maxSpeed)
                _rigidBody.velocity = _rigidBody.velocity.normalized * maxSpeed;
            else
            {
                float accelerationInput = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
                _rigidBody.AddRelativeForce(Vector3.forward * accelerationInput);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnspeed * Mathf.Clamp(speed / 1000, -1, 1) * Time.fixedDeltaTime);
        }
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

    private void SetSideSkid()
    {
        Vector3 direction = transform.position - lastPosition;
        Vector3 movement = transform.InverseTransformDirection(direction);

        lastPosition = transform.position;

        _sideSkidAmount = movement.x;
    }

}
