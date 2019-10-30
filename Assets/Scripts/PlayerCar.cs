using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : MonoBehaviour
{
    //Game related
    Quaternion targetRotation;
    Rigidbody _rigidBody;
    GameManager game;

    //Utility
    Vector3 lastPosition;
    float _sideSkidAmount = 0;
    bool airborne;
    float totalScore = 0;
    float currentScore = 0;
    float walltapCD = 0;
    float driftTime = 0;

    //Serializable fields
    [SerializeField] float turnspeed = 5;
    [SerializeField] float acceleration = 2;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] float maxSpeed;

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
        game = GameManager.Instance;
    }

    private void Update()
    {
        if (game.Playing)
        {
            if (!airborne)
            {
                SetRotationPoint();
                SetSideSkid();
            }

            if (driftTime <= Time.time)
            {
                totalScore += currentScore;
                UpdateTotalScore();
                currentScore = 0;
                currentScoreText.text = "";
            }
        }
    }

    private void FixedUpdate()
    {
        if (game.Playing)
        {
            float speed = _rigidBody.velocity.magnitude;

            if (!airborne)
            {
                if (speed > maxSpeed)
                    _rigidBody.velocity = _rigidBody.velocity.normalized * maxSpeed;
                else
                {
                    float accelerationInput = acceleration * Time.fixedDeltaTime;
                    _rigidBody.AddRelativeForce(Vector3.forward * accelerationInput);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnspeed * Mathf.Clamp(speed / 1000, -1, 1) * Time.fixedDeltaTime);
            }
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
                if (collision.impulse.magnitude < 2f)
                {
                    Debug.Log("Wall tap");
                    if (walltapCD < Time.time && _rigidBody.velocity.magnitude > 30)
                    {
                        Debug.Log("Walltap");
                        currentScore += 2000;
                        //Adds a cooldown of 2 seconds
                        walltapCD = Time.time + 2;
                    }
                    else
                        Debug.Log("Walltap on CD");
                }
                else
                {
                    Debug.Log("Hit wall");
                    currentScore = 0;
                    currentScoreText.text = "Hit a wall!";
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            //TODO: Game over / Crash
            case "Deadzone":
                _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                FindObjectOfType<GameManager>().GameOver();
                break;
            case "ScorePlus":
                Debug.Log("ScorePlus");
                totalScore += 200;
                UpdateTotalScore();
                break;
            case "Airborne":
                airborne = true;
                _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                break;
            case "Finish":
                totalScore += currentScore;
                UpdateTotalScore();
                FindObjectOfType<GameManager>().Finished();
                break;
            default:
                break;
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
        driftTime = Time.time + 0.5f;
        currentScore += (intensity * _rigidBody.velocity.magnitude) / 3;
        currentScoreText.text = "DRIFT: \n" + "+" + currentScore.ToString("0");
    }

    private void UpdateTotalScore()
    {
        totalScoreText.text = "Score: \n" + totalScore.ToString("0");
    }
    #endregion
}
