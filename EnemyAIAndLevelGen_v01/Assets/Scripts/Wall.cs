using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is attached to an empty game object that instantiate a vertical wall consisting of cubes
public class Wall : MonoBehaviour
{
    //this takes a prefab of the cube
    public GameObject block;
    //this specifies the height of the wall
    public int width = 10;
    //width
    public int height = 4;
    void Start()
    {
        //two for loops to instantiate a wall
        //we've done it multiple times
        for (int y=0; y<height; ++y)
        {
            for (int x=0; x<width; ++x)
            {
                Vector3 offset = new Vector3(x, y, 0);
                Instantiate(block, transform.position + offset, Quaternion.identity);
            }
        }        
    }
}
