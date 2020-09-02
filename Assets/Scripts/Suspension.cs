using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public enum SuspensionLocation
    {
        FRONTLEFT, FRONTRIGHT, BACKLEFT, BACKRIGHT
    }

    public SuspensionLocation wheelLocation;

    [SerializeField, Header("Suspension")]
    float restLength;
    [SerializeField]
    float springTravel;
    [SerializeField]
    float springStiffness;
    [SerializeField]
    float dampingStiffness;

    float maxLength;
    float minLength;
    float lastLength;
    float springLength;
    float springVelocity;
    float springForce;
    float dampingForce;



    Vector3 suspensionForce;
    Vector3 wheelVelocity;
    float forceX;
    float forceY;

    [SerializeField, Header("Wheel")]
    float wheelRadius;
    public float steerAngle;
    public float steerTime;

    private float wheelAngle;

    GameObject car;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Car");
        rb = car.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);

        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.green);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, maxLength + wheelRadius)) 
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            dampingForce = dampingStiffness * springVelocity;

            suspensionForce = (springForce + dampingForce) * transform.up;

            wheelVelocity = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            forceX = Input.GetAxis("Vertical") * 0.5f * springForce;
            forceY = wheelVelocity.x * springForce;


            rb.AddForceAtPosition(suspensionForce + (forceX * transform.forward) + (forceY * -transform.right), hit.point);
        }
    }


}
