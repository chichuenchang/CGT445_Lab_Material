using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Player_control : MonoBehaviour
{ 
    //for player rigidbody 
    Rigidbody rigidbody_choppy;

    //for player audio
    AudioSource Audio_choppy;
    //these are the public variable that hold the sound track
    //click and drag the sound track to this field in the inspector tab
    //soundtracks can be downloaded from https://freesound.org/
    [SerializeField] AudioClip engine_sound;
    [SerializeField] AudioClip explosion_sound;
    [SerializeField] AudioClip finish_sound;
    [SerializeField] AudioClip choppy_ambient_noise;
    
    //particle
    //here I got all the pre-designed particles from unity asset store
    [SerializeField] GameObject in_explodeParticl;
    [SerializeField] ParticleSystem finish_particle;

    //parameters for control
    [SerializeField] float turnSpd = 100.0f;
    [SerializeField] float boostPower = 100.0f;

    //for load next level
    bool addLevelToggle = false;

    //[VERY IMPORTANT]
    //enumeration of game state can be defined by this syntax
    //game state is a very handy tool to let us control everything
    //here's how we control the game
    //1) set the game state prompted by certain conditions
    //2) constantly check the game state in update() and do whatever accordingly to each state
    //if the state is "alive" then everything is running
    //if the state is set to "die" then we stop the user input and play the explosion particle effect
    //if the state is "win" we put down some words on the screen and say you win and then load the next level
    enum State {Alive, Dying, Win};
    //by default set the state to alive
    State gameState =  State.Alive;

    //this is for debug, whenever this is set to false we don't want choppy react to collision
    bool collisionRespond = true;

    // Start is called before the first frame update
    void Start()
    {
        //variable initializing
        //get the rigid body of the choppy object and assign it to a handler
        rigidbody_choppy = gameObject.GetComponent<Rigidbody>();
        rigidbody_choppy.mass = 0.06f;

        //get the audio component and assign it to a handler
        Audio_choppy = gameObject.GetComponent<AudioSource>();

        //play the audio 
        Audio_choppy.PlayOneShot(choppy_ambient_noise);
    }

    // Update is called once per frame
    void Update()
    {
        //check game state in update()
        if (gameState == State.Alive)
        {
            //if the state is alive we use a function to get user input and apply it to the game
            processUserInput();
        }

        //propeller spinning animation of choppy game object
        propRotate();
    }

    //[VERY IMPORTANT]
    //just like start() or update(), OnCollisionEnter() gets called automatically
    //start() is called at the beginning of the game
    //update() is called every frame
    //OnCollisionEnter() is called whenever the colliding box component of this gameObject touches another one
    //in this case, whenever the choppy collides with another object we want to do something like destroy the choppy
    //duely note that the parameter here "Collision collidingObj"
    //this variable has a type of Collision and is refering to any object that is colliding with "choppy"
    //if the choppy touches the "wall" then the collidingObj is going to pass the reference of the Collision component of "wall"
    //same thing happens to "floor" or "projectile" or anything that "choppy" could possibly hit
    private void OnCollisionEnter(Collision collidingObj)
    {
        //we make choppy invincible either when state is not alive or debugstate is false 
        //
        if (gameState != State.Alive|| !collisionRespond) 
        {
            //if return is put in a function
            //the compiler will no longer execute the lines of code below return
            return;
        }

        //another advantage that we can take from OnCollisionEnter() is the argument "collidingObj" that passed in 
        //we can use this to check what kind of obj that choppy is colliding with
        //we can access the tag variable of collidingObj by collidingObj.gameObject.tag
        //what is tag?  go back to unity editor and select any game object 
        //check the inspector tab, somewhere right below the name of the object you'll find tag
        //tag is a very handy tool that we can use to mark all the objects in the scene
        //we can mark the player object as "player"
        //mark the scene as "state level object"
        //mark the enemy as "enemy"

        //here we are checking the tag of the object we collide
        switch (collidingObj.gameObject.tag)
        {
            //if the tag of the colliding object is "friendly" we do nothing 
            case "Friendly":
                break;
            //if the tag is "finish" meaning that we win we call a function to do something if we win
            case "Finish":
                ReactToHitDestination();
                break;
            //if collide to tag that didn't specify we want to destroy the choppy
            default:
                ReactToHitDeadly();
                break;
        }
    }

    //the function that specifies what to do when choppy collides to deadly object
    private void ReactToHitDeadly()
    {
        gameState = State.Dying;//set game state to dying
        Audio_choppy.Stop(); //audio stop playing whenever player is dying

        //instantiate a explosive particle prefab and play it when player dies
        GameObject explodeParticle = Instantiate(in_explodeParticl, transform.position, Quaternion.identity);
        //play the particle effect
        explodeParticle.GetComponent<ParticleSystem>().Play();
        //play the explosive sound track
        Audio_choppy.PlayOneShot(explosion_sound);

        //when the choppy explode we can also use some method to have it break into pieces
        Demolish(gameObject);

        //use Invoke() to call the load level function 1 second later when the deadly collision happens
        Invoke("LoadCurrLevel", 1f);

        //you can also do something else like put on a "you died" on the screen here
    }

    //the way with which we break a gameOjbect into pieces is pretty easy
    //the trick is to simply add rigidbody component to every child of the object
    //so, whenever the choppy hits deadly object the transform of each child will be taken over by the unity physical engine
    private void Demolish(GameObject in_gameObj)
    {
        //use gameobject.transform.childCount to get how many child the gameobject has
        //traverse all the children
        for(int i = 0; i < in_gameObj.transform.childCount - 1; i++)
        {
            //add a rigidbody component to each child
            in_gameObj.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
        }

    }

    //specify what do we do when we hit destination
    private void ReactToHitDestination()
    {
        //set the state to win
        gameState = State.Win;//set state
        Audio_choppy.Stop();
        //play the sound track of winning
        Audio_choppy.PlayOneShot(finish_sound);
        //play the particle effect of winning
        finish_particle.Play();

        //you may do something like putting " you win" on the screen

        if (!addLevelToggle)
        {
            addLevelToggle = true;
            //use Invoke() to load the next level 1 second later
            Invoke("LoadNextLevel", 1f);
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void LoadCurrLevel()
    {
        SceneManager.LoadScene(0);
    }

    //this is called in the update()
    private void processUserInput()
    {
        //what to do when user press the button that boost the choppy
        //we pass in which key to press to trigger the boost
        //and pass in which sound track to be played when the boost is triggered
        RespondToBoostInput(KeyCode.Space, engine_sound);

        //what to do when user orient the choppy
        //pass in the key that we want to make the player rotate clock-wise or counter clock-wise
        RespondToRotateInput(KeyCode.A, KeyCode.D);

        //for debug build
        if (Debug.isDebugBuild)
        {
            RespondToDebugInput();
        }
    }

    //this is for debug purpose, you may ignore it
    //whenever c is pressed the player is invincible
    //convinient when we need to test our game
    private void RespondToDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionRespond = !collisionRespond;
            print("collisionRespond = " + collisionRespond);
        }
    }


    private void RespondToBoostInput(KeyCode in_boostButton, AudioClip in_boostSoundTrack)
    {
        //to check whenever the key we passed in is pressed
        if (Input.GetKey(in_boostButton))
        {
            //if that key is pressed we add a upward force to the rigidbody component of choppy
            rigidbody_choppy.AddRelativeForce(Vector3.up* boostPower);

            if (!Audio_choppy.isPlaying)
            {//play the boost sound track whenever it is not being played
                Audio_choppy.PlayOneShot(in_boostSoundTrack);
            }
        }
        else//whenever that key is not pressed, stop the sound track
        {
            Audio_choppy.Stop();
        }

    }

    //basically the same idea of the function above
    private void RespondToRotateInput(KeyCode keyForCCW, KeyCode keyForCW)
    {
        //the reason why I put 2 bool right here is to solve the problem occurs when A and D are pressed in the same time
        //so I declare 2 bools to hold the state of whether or not either A or D is pressed
        bool CCWPressed = false;
        bool CWPressed = false;
        
        //use input.getkeyup() to detect if user release the key
        //if the key is released we set the corresponding state to false
        if (Input.GetKeyUp(keyForCCW)) CCWPressed = false;
        if (Input.GetKeyUp(keyForCW)) CWPressed = false;

        //when the key for counter clock-wise is pressed we first check if the state for another direction is true
        //if it is not true means the key to rotate the object in another direction is not pressed then it's good to process the input
        //if it is true means the key of another direction is being pressed so we do nothing
        if (Input.GetKey(keyForCCW))
        {
            if (!CWPressed)
            {
                //call the function of rotating the choppy and pass in how fast we want it to rotate
                RotateCounterCW(turnSpd);
                CCWPressed = true;
            }
        }
        if (Input.GetKey(keyForCW))
        {
            if (!CCWPressed)
            {
                //pass in the negative speed value so that it rotates other way
                RotateCounterCW(-turnSpd);
                CWPressed = true;
            }
        }
      

    }

    //function we call to rotate the choppy
    private void RotateCounterCW(float rotateSpd)
    {
        //since our object has its own rigidbody component meaning that the momentum will often affect the user rotating input
        //making the user control less accurate, and we don't want that
        //what we want is that the user take full control of rotation whenever the key is pressed and get rid of the influence from physics
        rigidbody_choppy.freezeRotation = true;
        //familiar?
        gameObject.transform.Rotate(0.0f, 0.0f, rotateSpd * Time.deltaTime, Space.World);
        //after the player release the key let physics take over the rotation
        rigidbody_choppy.freezeRotation = false;
    }

    //propeller rotating animation
    //this is called in the update()
    private void propRotate()
    {
        gameObject.transform.GetChild(0).Rotate(0.0f, 30.0f, 0.0f, Space.Self);
        gameObject.transform.GetChild(1).Rotate(0.0f, 0.0f, 30.0f, Space.Self);
    }
}


