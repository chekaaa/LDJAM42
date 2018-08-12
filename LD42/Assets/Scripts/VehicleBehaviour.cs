using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{
    private const string OUTSIDE_TAG = "outside";

    private RaycastHit2D[] hits;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask triggerMask;

    public bool isParked;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameManager.instance.vehicleList.Add(this.transform);
    }

    void FixedUpdate()
    {
        CheckIfParked();
    }

    void CheckIfParked()
    {
        Collider2D trigger = Physics2D.OverlapCircle(transform.position, .5f, triggerMask);

        //Cast a overlap circle from the center of the ship , if it detects the outside trigger collision
        //that means its not parked, if it doesnt detect the outside trigger then check if the ships is moving
        //if its not moving then set this ship as parked
        if (trigger != null)
        {
            isParked = false;
        }
        else
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            if (localVelocity.y < .1f)
            {
                isParked = true;
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
    }

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     Debug.Log("@OnTriggerEnter2D - " + collider.tag);
    //     if (collider.tag == OUTSIDE_TAG)
    //     {
    //         isParked = false;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collider)
    // {
    //     Debug.Log("@OnTriggerExit2D - " + collider.tag);

    //     if (collider.tag == OUTSIDE_TAG)
    //     {
    //         isParked = true;
    //     }
    // }


}
