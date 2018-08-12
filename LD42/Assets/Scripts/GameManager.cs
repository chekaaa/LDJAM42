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
    public TMP_Text timerGUI, parkedGUI;


    private float timer = 0f;

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
        timer = 0;
    }

    void Update()
    {
        UpdateTime();
        UpdateParkedUI();
        if (areAllParked())
        {
            //WOn game
            Debug.Log("All parked");
        }

    }

    public void GameOver()
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

    //Display in the screen the amount of ships parked
    void UpdateParkedUI()
    {
        int parked = 0;
        int maxParked = vehicleList.Count;

        foreach (Transform t in vehicleList)
        {
            if (t.GetComponent<VehicleBehaviour>().isParked)
            {
                parked++;
            }
        }

        parkedGUI.text = "Ships parked: \n " + parked + "/" + maxParked;
    }

    //Return if the the time passed the time limit
    void UpdateTime()
    {
        timer += Time.deltaTime;
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);
        string sMinutes = minutes.ToString();
        string sSeconds = seconds.ToString();

        if (minutes < 10)
        {
            sMinutes = "0" + minutes.ToString();
        }
        if (seconds < 10)
        {
            sSeconds = "0" + Mathf.RoundToInt(seconds).ToString();
        }

        timerGUI.text = sMinutes + ":" + sSeconds;

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
