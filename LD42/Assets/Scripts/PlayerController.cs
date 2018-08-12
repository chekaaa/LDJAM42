using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{


    void Start()
    {
        Debug.Log("Subscribing to the event");
        //Register to event
        GameManager.instance.OnVehicleSelected += OnVehicleClicked;

    }

    void OnDestroy()
    {
        GameManager.instance.OnVehicleSelected -= OnVehicleClicked;
    }

    void OnMouseDown()
    {
        //If the vehicle is clicked, Gamemanager check all the vehicles , if the vehicle checked is not the one
        //clicked it disables the controls
        Debug.Log("@OnMouseDown - " + transform.name);
        GameManager.instance.SwitchVehicleControl(transform.name);
    }

    public void OnVehicleClicked(string vehicleName)
    {
        Debug.Log("@OnVehicleClicked - " + transform.name);
        //If this is the vehicle that caleed the event, it will become the active vehicle
        //if not this vehicle will become inactive
        isSelected = (transform.name == vehicleName);
    }


    protected override void ProcessInputs()
    {
        accelerationDirection = Input.GetAxis("Vertical");

        torqueDirection = Input.GetAxis("Horizontal");
    }
}