using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this enemy AI is rather simple since no navmesh agent is involved
public class enemyAI : MonoBehaviour
{
    //how the effect of destroying is created on the robot is the same way we did in the player
    public GameObject wreckedVersion;
    public float robotSpeed;
    public int robotHp; //a count of hits before it is wrecked

    //we want to know where the play is
    Transform player;

    private void Start()
    {
        //get the player transform
        player = GameObject.Find("Tank").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) <= 50.0f)
        {
            transform.LookAt(player);
            transform.Translate(Vector3.forward * Time.deltaTime * robotSpeed);
        }
    }

    void OnCollisionEnter(Collision collidingObj)
    {
        //if the robot object is hit by the projectile form player
        //HP decreases
        if (collidingObj.gameObject.tag == "FireBall")
        {
            robotHp--;
        }

        //when hp is depleted to 0 we destroy robot and create a wrecked version
        if (robotHp == 0)
        {
            Destroy(gameObject);
            Instantiate(wreckedVersion, transform.position, transform.rotation);
        }
    }






}
