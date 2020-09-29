using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenLevel : MonoBehaviour
{
    //this project generally works the same way as the grid-based city generation

    //GameObject array that holds your different prefabs
    //whenever you declare a public data array, it can be seen in the inspector tab in Unity
    //note that you should specify the number of elements before you drag your object into the variable
    public GameObject[] in_empPrefab;

    //a integer that controls the number of rows of the grid
    public int numRow = 10;
    //integer for the column
    public int numCol = 10;
    //how far each grid is away from each other
    public float offsetDist = 1.0f;
    //a variable controling the percentage of an "flat tile" object to be instantiated in the grid
    //it's a percentage, which means it varies from 0.0f to 1.0f
    public float tilePercentage;

    //a private 2D GameObject array that holds all the object instantiated in the for loop
    private GameObject[,] gridObj;

    void Start()
    {
        //this variable represents for a percentage
        //therefore it only makes sense when user input a value from 0.0f to 1.0f;
        //but sometimes useres might have a typo and put a number outside of 0.0f -1.0f;
        //so we use Mathf.Clamp() function to force the value to be within 0.0f - 1.0f;
        //clamp is a useful function since it is widely used
        tilePercentage = Mathf.Clamp(tilePercentage, 0.0f, 1.0f);
        //initialize the 2D GameObject array
        gridObj = new GameObject[numRow, numCol];

        //the function to create the grid 
        createGrid(in_empPrefab, gridObj, tilePercentage);
    }

    //we define our own function to create the grid, we don't have to do it this way, it's the style of coding to improve readability
    //the function is taking 3 arguments that we need for the grid-base level generation
    //1) a 1D GameObject array that holds all the prefabs that we wish to instantiate in the grid
    //the reason why I put "in_" as the surfix in the name of the variable is that this data array is given before the function is excecuted
    //2) a 2D GameObject array that holds all the object on the grid
    //the reason of the "out_" in the name of the variable is that this data array is an output of the function 
    //3) we need the percentage of the flat tile to define our grid
    private void createGrid(GameObject[] in_prefabArray, GameObject[,] out_gridObjArray, float percentFlatTile)
    {
        //traverse through the z direction
        for (int i = 0; i < numRow; i++)
        {
            //traverse through the x direction
            for (int j = 0; j < numCol; j++)
            {
                //find the corner of the grid
                if ((i == 0 && j == 0) || (i == 0 && j == numCol - 1) || (i == numRow - 1 && j == 0) || (i == numRow - 1 && j == numCol - 1))
                {
                    //we want nothing in corner, but we need something to put in the array
                    out_gridObjArray[i, j] = new GameObject("emptyCorner");
                }
                //find the edge of the grid on the negative y direction
                else if (i == 0)
                {
                    //instantiate walls and rotate them 90 degree about x axis
                    //why "in_prefabArray[5]" is wall? 
                    //because I click and drag a wall to the 6th element in the inspector tab
                    //in fact, a preferred way to do this is to name another independent public GameObject and drag your wall to that one
                    out_gridObjArray[i, j] = Instantiate(in_prefabArray[5], new Vector3(-(numCol - 1) * offsetDist / 2.0f + j * offsetDist, 0.0f, -(numRow - 1) * offsetDist / 2.0f + i * offsetDist + offsetDist / 2.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));
                }
                //find the edge of the grid on the positive y direction
                else if (i == numRow - 1)
                {
                    //instantiate walls and rotate them 90 degree about x axis
                    out_gridObjArray[i, j] = Instantiate(in_prefabArray[5], new Vector3(-(numCol - 1) * offsetDist / 2.0f + j * offsetDist, 0.0f, -(numRow - 1) * offsetDist / 2.0f + i * offsetDist - offsetDist / 2.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));
                }
                //find the edge of the grid on the negative x direction
                else if (j == 0)
                {
                    //instantiate walls and rotate them 90 degree about z axis
                    out_gridObjArray[i, j] = Instantiate(in_prefabArray[5], new Vector3(-(numCol - 1) * offsetDist / 2.0f + j * offsetDist + offsetDist / 2.0f, 0.0f, -(numRow - 1) * offsetDist / 2.0f + i * offsetDist), Quaternion.Euler(0.0f, 0.0f, 90.0f));
                }
                //find the edge of the grid on the positive x direction
                else if (j == numCol - 1)
                {
                    //instantiate walls and rotate them 90 degree about z axis
                    out_gridObjArray[i, j] = Instantiate(in_prefabArray[5], new Vector3(-(numCol - 1) * offsetDist / 2.0f + j * offsetDist - offsetDist / 2.0f, 0.0f, -(numRow - 1) * offsetDist / 2.0f + i * offsetDist), Quaternion.Euler(0.0f, 0.0f, 90.0f));
                }
                //generate grid that is not corners or edges
                else
                {

                    out_gridObjArray[i, j] = Instantiate(in_prefabArray[WalkableTilePercentage(percentFlatTile, in_prefabArray.Length)], new Vector3(-(numCol - 1) * offsetDist / 2.0f + j * offsetDist, 0.0f, -(numRow - 1) * offsetDist / 2.0f + i * offsetDist), Quaternion.identity);
                    //out_gridObjArray[i, j] = Instantiate(in_empPrefab[Random.Range(0,5)], new Vector3 (- (numCol-1)*offsetDist/ 2.0f + j * offsetDist, 0.0f, -(numRow-1)*offsetDist/2.0f + i*offsetDist), Quaternion.identity);
                }
                //set the name of all the elements in the 2D array
                //it's not necessay but it makes us easier to identify an object in the scene
                //if we don't put this, everything will be just named as clone
                out_gridObjArray[i, j].name = "gridObj " + i + ", " + j;
            }

        }
    }

    //this function is a little bit of tricky
    //if you want to know how it works, come to my office hour
    //It's perfectly fine if you don't know
    //but what you do need to know is what it does
    //what it does is it acts very similar to a random function
    //it randomly returns an integer from 0 to "numDiffObj-1"
    //the thing is, the possiblity for each value returned by Random.Range() is the same 
    //but this function returns 0 with a possibility that equals to "tileAearPercent"
    int WalkableTilePercentage(float tileAreaPercent, int numDiffObj)
    {
        int output = 0;
        float tempRand = 0f;
        float flatTilePerc = tileAreaPercent * (numDiffObj - 1) / (1 - tileAreaPercent);

        tempRand = Random.Range(0.0f, flatTilePerc+ (numDiffObj-1.0f));

        for (int i = 0; i < numDiffObj; i++)
        {
            if(i == 0)
            {
                if (tempRand >= 0.0f && tempRand <= flatTilePerc) output = 0;
                continue;
            }
            else
            {
                if (tempRand > (flatTilePerc + i - 1) && tempRand <= (flatTilePerc + i)) output = i;
            }
        }
        return output;
    }
}
