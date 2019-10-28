using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    float totalScore = 0;
    float currentScore = 0;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;

    public float SideSkidAmount
    {
        get
        {
            return _sideSkidAmount;
        }
    }

    #region basics
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
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
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (collision.impulse.magnitude > 10f)
            {
                Debug.Log("Game Over");
                FindObjectOfType<GameManager>().GameOver();
            }
            else
            {
                if(collision.impulse.magnitude < 2f)
                {
                    Debug.Log("Wall tap");
                    //TODO: Give walltap bonus. Needs to have a cooldown! (1 second should be sufficient)
                    currentScore += 500;
                }
                else
                {
                    Debug.Log("Hit wall");
                    //TODO: Hit wall too hard, reset score
                    currentScore = 0;
                    currentScoreText.text = "Hit a wall!";
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Deadzone")
        {
            //TODO: dead / You've crashed
            FindObjectOfType<GameManager>().GameOver();
            return;
        }
        else if (other.gameObject.tag == "Airborne")
        {
            airborne = true;
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Airborne")
        {
            airborne = false;
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
    }

    #region specificMethods

    private void SetRotationPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;

        if (plane.Raycast(ray, out distance))
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

    public void CalculateScore(float intensity)
    {
        currentScore += (intensity * _rigidBody.velocity.magnitude) / 3;
        currentScoreText.text = "DRIFT: \n" + "+" + currentScore.ToString("0");
    }

    #endregion
}
