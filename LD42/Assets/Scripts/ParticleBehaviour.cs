using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    private float maxLifeTime = 2.5f;
    private float lifeTime = 0f;

    void Update()
    {

        lifeTime += Time.deltaTime;
        //If this gameobject pass his maxLifetime its destroyed
        if (lifeTime > maxLifeTime)
        {
            Destroy(this.gameObject);
        }
    }


}
