using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Cars in the scene will be registered in this event
    // This event will be used for switching which vehicle the player controls
    public delegate void VehicleSelected(string vehicleName);
    public event VehicleSelected OnVehicleSelected;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SwitchVehicleControl(string name)
    {
        Debug.Log("@SwitchVehicleControl");
        if (OnVehicleSelected != null)
        {
            //Execute OnVehicleClicked on all vehicles checking for who is the active
            OnVehicleSelected(name);
            Debug.Log("Calling event");
        }
    }

}
