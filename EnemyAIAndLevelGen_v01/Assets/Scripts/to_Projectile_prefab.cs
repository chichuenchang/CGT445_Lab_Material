using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class to_Projectile_prefab : MonoBehaviour
{
    //This script is attached separately to the projectile prefab itself
    //whenever the project tile is instantiated, this script is generated as well
    //controling how should the projectile perform after it is fired
    //the explosion gameobject here is taking a particle effect
    //we make an explosion effect to appear the same way we instantiate an gameObject
    public GameObject explosion;

    void Start()
    {
         // The projectile is deleted after 10 seconds, whether
         // or not it collided with anything (to prevent lost
         // instances sticking around in the scene forever)
         Destroy(gameObject,10);
    }

    //it is legal to put OnCollisionEnter() without an argument
    //it means it need no Collision variable passing in and gets called whenever a collision is detected
    void OnCollisionEnter()
    {
        // it hit something: create an explosion, and remove the projectile
        Instantiate(explosion,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
