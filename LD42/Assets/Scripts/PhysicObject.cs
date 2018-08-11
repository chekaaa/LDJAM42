using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] private float accelerationForce = 5f;
    [SerializeField] protected float torqueForce = 2f;
    protected float torqueDirection;
    protected float accelerationDirection;
    protected float stopThreshold = .7f;

    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Request player inputs
        ProcessInputs();
    }

    protected virtual void ProcessInputs() { }

    void FixedUpdate()
    {
        //Add the forces and torque to the RigidBody2D
        Movement();
    }


    void Movement()
    {
        rb.velocity = ForwardVelocity();
        //Impulse the car forward
        rb.AddForce(accelerationDirection * accelerationForce * transform.up);
        //If the car velocity is not between the threshold it will start turning
        //but if it is the car will smoothly stop turning
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        if (localVelocity.y > stopThreshold)
        {
            rb.angularVelocity = (torqueForce * -torqueDirection);
        }
        //invert the turn if backing
        else if (localVelocity.y < -stopThreshold)
        {
            rb.angularVelocity = (torqueForce * torqueDirection);
        }
        else
        {
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, 2f * Time.deltaTime);
        }



    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(rb.velocity, transform.up);
    }

    Vector2 SideDirection()
    {
        return transform.right * Vector2.Dot(rb.velocity, transform.right);
    }

}
