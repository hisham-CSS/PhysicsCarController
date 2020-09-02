using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Suspension[] suspensionPoints;

    [SerializeField, Header("Car Specs")]
    float wheelBase; //in meters
    [SerializeField]
    float rearTrack; //in meters
    [SerializeField]
    float turnRadius;//in meters

    [SerializeField, Header("Inputs")]
    float steerInput;

    private float ackermannAngleLeft;
    private float ackermannAngleRight;

    // Update is called once per frame
    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");

        if (steerInput > 0) // turning right
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        }
        else if (steerInput < 0) // turning left
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        else //no input
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }
        
        foreach(Suspension s in suspensionPoints)
        {
            switch (s.wheelLocation)
            {
                case Suspension.SuspensionLocation.BACKLEFT:
                    break;
                case Suspension.SuspensionLocation.BACKRIGHT:
                    break;
                case Suspension.SuspensionLocation.FRONTLEFT:
                    s.steerAngle = ackermannAngleLeft;
                    break;
                case Suspension.SuspensionLocation.FRONTRIGHT:
                    s.steerAngle = ackermannAngleRight;
                    break;

            }
        }
    }
}
