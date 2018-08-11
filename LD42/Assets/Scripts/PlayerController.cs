using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    protected override void ProcessInputs()
    {
        accelerationDirection = Input.GetAxis("Vertical");

        torqueDirection = Input.GetAxis("Horizontal");
    }
}