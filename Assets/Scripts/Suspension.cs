using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
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

    [SerializeField, Header("Wheel")]
    float wheelRadius;
    


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

            rb.AddForceAtPosition(suspensionForce, hit.point);
        }
    }
}
