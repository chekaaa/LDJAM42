using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleBehaviour : MonoBehaviour
{
    //Layer Names
    private const string OUTSIDE_TAG = "outside";
    private const string WALL_TAG = "wall";
    private const string SHIP_TAG = "ship";

    //Components of this gameobject
    private RaycastHit2D[] hits;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask triggerMask;

    //Hp stats and representation
    public float shipMaxHp = 100f;
    public float shipHp;
    [SerializeField] private float crashDamage = 20f;
    [SerializeField] private GameObject hpBarCanvas;
    [SerializeField] private Image hpBarImage;
    private float hpBarDisplayTime = 1f;

    //FX
    private AudioSource audioSource;
    [SerializeField] private AudioClip crashClip;
    [SerializeField] private GameObject explosionParticle;

    public bool isParked;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shipHp = shipMaxHp;
        audioSource = GetComponent<AudioSource>();
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
        Collider2D trigger = Physics2D.OverlapCircle(transform.position, .3f, triggerMask);

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

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, .5f);
    // }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.tag == WALL_TAG || collider.transform.tag == SHIP_TAG)
        {
            //Reduce ship HP
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        //Reduce ship hp
        shipHp -= crashDamage;
        //Stop all coroutines just to prevent the bar is disabled by another coroutin too fast
        StopAllCoroutines();
        StartCoroutine(DisplayHPBar(hpBarDisplayTime));
        //Play FXs
        PlayCrashFx();
        if (shipHp <= 0)
        {
            GameManager.instance.GameOver();
            //TODO: Destroy ship
            DestroyShip();
        }
    }

    void DestroyShip()
    {
        //Instantiate explosion particles
        PlayExplosionFx();
        //Destroy Ship Gameobject
        Destroy(this.gameObject);

    }

    void PlayExplosionFx()
    {
        //TODO: PLay explosion sound

        //Instantiate the explosion prefab that plays particles
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
    }

    void PlayCrashFx()
    {
        //Play sound a visual FXs

        //Set the crash sound to the audioSource
        audioSource.clip = crashClip;
        //Play the clip
        audioSource.Play();

    }

    IEnumerator DisplayHPBar(float seconds)
    {
        hpBarCanvas.SetActive(true);

        float fillPercent = shipHp / shipMaxHp;
        hpBarImage.fillAmount = fillPercent;

        yield return new WaitForSeconds(seconds);

        hpBarCanvas.SetActive(false);

    }

}
