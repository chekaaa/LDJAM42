using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Cars in the scene will be registered in this event
    // This event will be used for switching which vehicle the player controls
    public delegate void VehicleSelected(string vehicleName);
    public event VehicleSelected OnVehicleSelected;

    public List<Transform> vehicleList = new List<Transform>();
    public TMP_Text timerGUI, parkedGUI, bestTimeGUI, yourTimeGUI;
    public GameObject gameOverPanel, winPanel;

    [SerializeField] private LayerMask shipLayermask;
    private float timer = 0f;
    private bool isGameOver;

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
        if (!isGameOver)
        {
            UpdateTime();
            UpdateParkedUI();
            if (areAllParked())
            {
                //WOn game
                Win();
                Debug.Log("All parked");
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                 new Vector3(0, 0, 1), 500f, shipLayermask);
                if (hit.collider != null)
                {
                    SwitchVehicleControl(hit.transform.name);
                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
            }
        }


    }

    public void Win()
    {
        isGameOver = true;

        DeselectallShips();

        gameOverPanel.SetActive(false);
        winPanel.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("bestTime", -1);
        if (bestTime == -1)
        {
            PlayerPrefs.SetFloat("bestTime", timer);
        }
        else if (bestTime > timer)
        {
            PlayerPrefs.SetFloat("bestTime", timer);
        }
        bestTime = PlayerPrefs.GetFloat("bestTime", -1);
        string formatedBestTime = FormatTime(bestTime);
        string formatedTimer = FormatTime(timer);
        bestTimeGUI.text = formatedBestTime;
        yourTimeGUI.text = formatedTimer;

    }

    public void GameOver()
    {
        // this.enabled = false;
        isGameOver = true;
        //Disable all vehicles
        DeselectallShips();

        //Show GameOver Panel
        winPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Debug.Log("GameOver");
    }

    void DeselectallShips()
    {
        foreach (Transform t in vehicleList)
        {
            t.transform.GetComponent<PlayerController>().isSelected = false;
        }
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

        timerGUI.text = FormatTime(timer);

    }

    string FormatTime(float t)
    {
        float minutes = Mathf.Floor(t / 60);
        float seconds = Mathf.RoundToInt(t % 60);
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
        return sMinutes + ":" + sSeconds;
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
