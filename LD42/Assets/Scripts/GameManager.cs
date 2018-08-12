using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Cars in the scene will be registered in this event
    // This event will be used for switching which vehicle the player controls
    public delegate void VehicleSelected(string vehicleName);
    public event VehicleSelected OnVehicleSelected;

    public List<Transform> vehicleList = new List<Transform>();
    public TMP_Text timerGUI;

    [SerializeField] private float timeLimit = 30;
    private float timePassed = 0f;

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

    void Start()
    {
        timePassed = timeLimit;
    }

    void Update()
    {
        if (!isTimeUp())
        {
            if (areAllParked())
            {
                //WOn game
                Debug.Log("All parked");
            }
        }
        else
        {
            //Game Over
            GameOver();
            Debug.Log("Game Over");
        }
    }

    void GameOver()
    {
        this.enabled = false;

        //Disable all vehicles
        foreach (Transform t in vehicleList)
        {
            t.transform.GetComponent<PlayerController>().isSelected = false;
        }

        //Show GameOver Panel
        Debug.Log("GameOver");
    }

    //Return if the the time passed the time limit
    bool isTimeUp()
    {
        timePassed -= Time.deltaTime;
        timerGUI.text = (int)timePassed + "";
        if (timePassed <= 0)
        {
            return true;
        }
        return false;
    }

    //Return if all vehicles are inside the hangar
    bool areAllParked()
    {
        bool value = true;
        foreach (Transform v in vehicleList)
        {
            if (!v.GetComponent<VehicleBehaviour>().isParked)
            {
                value = false;
            }
        }

        return value;
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
