using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    //a public varialbe taking the prefab of the wrecked version of tank
    //the idea is to instantiate this prefab as soon as the player is destroyed
    //so that a destroy effect can be created
    public GameObject wreckedVersion;
    public Rigidbody projectile;
    public float projectileSpeed = 4.0f;
    public float in_fireCD = 0.33f;

    //a variable controling how fast does the tank rotates
    //you can make it public to be accessed from the unity side
    float hAngle = 0;

    //define the game state which controls the game process
    enum state {alive, dead, win};
    //set the initial state
    state playerStat = state.alive;

    //Fire Cool Down
    float temp_fireCD;

    //the position to instantiate projectiles
    Transform projectileSpawn;



    private void Start()
    {
        initialization();
    }

    //to assign values to some variables
    private void initialization()
    {
        temp_fireCD = 0.0f;

        projectileSpawn = transform.GetChild(2).GetChild(0).GetChild(0);

        GameObject canvas = GameObject.Find("Canvas");
        Destroy(canvas);
    }

    void Update()
    {
        //process cooldown timer of projectile firing
        processTimer(ref temp_fireCD);

        //constantly check state in the update()
        if(playerStat == state.alive)
        {
            //if the game state is "alive" activate user input
            processInput();
        }

    }


    //OnColiissionEnter gets called whenever the colliding box of the object that this script attaching overlaps with another
    void OnCollisionEnter(Collision target)
    {
        //if the attached objected is detected a collision
        //then we check what is the tag of the object that the current object is colliding with
        //OnCollisionEnter is extremely handy because the other object that collides with the current one automatically passed in through the argument "Collision target"
        //which means the "(Collision target)" is the reference to the other object that collides with current one
        //we can easily access the other objects' tag by putting target.gameObject.tag
        //note that tag is a string type
        //in case you might wandering what a tag is, go back to your unity, look for the field right below the name of the object
        //we can use this "tag" system to mark what object it is for the convenience of ourselves, this is very handy when we have thousands of assets
        if (target.gameObject.tag == "Enemy")
        {
            //if the tag of the object that the current object collides with is "Enemy"
            //then we set the game state to "dead"
            playerStat = state.dead;
            //to create a destroy effect we can simply destroy the current object and replace with a wrecked version
            //that's why we call Destroy() and Instantiate();
            Destroy(gameObject);
            Instantiate(wreckedVersion, transform.position, transform.rotation);
        }
    }

    //we don't want the tank to have an infinite consecutiveness to fire the projectile
    //so let's set up a cooldown timer and allow the player to fire only when the cooldown is ready
    private void processTimer(ref float timerRef)
    {
        timerRef -= Time.deltaTime;
        if (timerRef <= 0.0f) timerRef = 0.0f;
    }

    //after the projectile is fired we call this function to reset cooldown
    private void resetTimer(ref float timerRef, float in_value)
    {
        timerRef = in_value;
    }

    private void processInput()
    {
        //in case you're wondering what is Input.GetAxis(), it is a built-in feature of unity
        //we used get KeyCode() to detect user input before, but GetAxis() can return a much smoother effect
        //how do settle GetAxis from Unity is through the main menu -> Edit -> Project Setting -> Input Manager -> Axes
        //unfold Axes and unfold "Horizontal" and "Vertical" you can see more paraters that you can twich with
        //Btw don't ask me why is it called "Axes", I have no idea
        //In here I use "Horizontal" input to control the rotation of the tank
        //and "Vertical" input to the forward and backward movement
        //[IMPOTANT]: duely noted that Input.GetAxis returns a float from -1.0f to 1.0f
        //and we use this float as an coefficient to control the movement
        float h = Input.GetAxis("Horizontal");
        hAngle += h;
        transform.localRotation = Quaternion.Euler(0, hAngle, 0);

        float v = Input.GetAxis("Vertical");
        transform.Translate(Vector3.back * Time.deltaTime * v * 2.0f);

        //when cooldown equals to 0 which means it is ready
        if(temp_fireCD == 0.0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //instantiate projectile
                Rigidbody temp = Instantiate(projectile, projectileSpawn.position, projectile.rotation);
                //set the velocity
                temp.velocity = projectileSpawn.forward * projectileSpeed;
                //reset cooldown
                resetTimer(ref temp_fireCD, in_fireCD);
            }
        }
    }
}

//a trick:
//don't be afraid to see a lot of script, there's always a way to figure out a connection between them in a project
//go to the asset tab select a script -> right click -> find reference in scene, you'll see to which object it is attached
//but you may not always find the whereabout of the script in the scene necessarily this way, since the script could be hold by prefabs that have yet to be instantiated

//The structure of the project:
//we have a player tank, and is holding the player script
//the tank has a empty game object child called launcher, it is where the projectiles are instantiated
//the fireball has a script called to_projectile_prefab, it says whenever the fireball has a collision it instantiates an explosion particle effect
//the explosion script is attached to the particle effect, it gives force to all the rigidbody in a certain range