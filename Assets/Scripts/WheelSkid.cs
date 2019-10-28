using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSkid : MonoBehaviour
{
    [SerializeField] float intensityModifier = 1.5f;

    Skidmarks skidmarkController;
    PlayerCar playerCar;
    ParticleSystem TireSmoke;

    int lastSkidId = -1;

    // Start is called before the first frame update
    void Start()
    {
        skidmarkController = FindObjectOfType<Skidmarks>();
        playerCar = GetComponentInParent<PlayerCar>();
        TireSmoke = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        float intensity = playerCar.SideSkidAmount;

        if (intensity < 0)
            intensity = -intensity;


        //If intensity is below certain value, don't skid.
        if (intensity > 0.2f)
        {
            lastSkidId = skidmarkController.AddSkidMark(transform.position, transform.up, intensity * intensityModifier, lastSkidId);
            FindObjectOfType<PlayerCar>().CalculateScore(intensity);
        }
        else
            lastSkidId = -1;

        if(intensity > 0.2f
            && TireSmoke != null 
            && !TireSmoke.isPlaying)
        {
                TireSmoke.Play();
        }
        else
        {
            if (TireSmoke != null && TireSmoke.isPlaying)
                TireSmoke.Stop();
        }
    }
}
