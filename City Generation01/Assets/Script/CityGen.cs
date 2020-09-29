using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGen : MonoBehaviour
{
    [SerializeField] float appearRange;
    [SerializeField] int numberObj;

    //numOfRoad doesn't change the number of roads
    int numOfRoad= 6;

    public Vector3 center;

    private GameObject[] poppingCube;
    private GameObject[] roadCube;
    
    void Start()
    {
        poppingCube = new GameObject[numberObj];
        roadCube = new GameObject[numOfRoad];

        createBuildingBlocks(poppingCube);
        createRoadCollider(roadCube);
    }
   
    void Update()
    {
        //*****************************************
        //1) run the scene and check the FPS
        //2) comment out Repositioning_Deprecated() and un-comment respositioning_Optimized()
        //3) check the FPS again, notice something?
        //4) take a look at these two funcitons, why the sudden change in performance?
        //*****************************************

        //Repositioning_Deprecated(poppingCube, roadCube);
        Repositioning_Optimized(poppingCube, roadCube);
    }

    private void Repositioning_Optimized(GameObject[] buildingArray, GameObject[] roadArray)
    {
        Vector3 tempPos;
        float dist;
        float randH;

        for (int j = 0; j < numberObj; j++)
        {
            tempPos = new Vector3(Random.Range(-appearRange, appearRange), -0.05f, Random.Range(-appearRange, appearRange));
            dist = (tempPos - center).magnitude;
            randH = Mathf.Clamp(Random.Range(2.5f - 0.5f * dist, 3.0f - 0.5f * dist), 0.1f, 3.0f);

            for (int i = j + 1; i < numberObj; i++)
            {
                if (buildingArray[j].GetComponent<Collider>().bounds.Intersects(buildingArray[i].GetComponent<Collider>().bounds))
                {
                    buildingArray[j].transform.localScale = new Vector3(Random.Range(0.3f, 0.6f), randH, Random.Range(0.3f, 0.6f));
                    buildingArray[j].transform.position = tempPos;
                }
            }

            for (int k = 0; k < numOfRoad; k++)
            {
                if (buildingArray[j].GetComponent<Collider>().bounds.Intersects(roadArray[k].GetComponent<Collider>().bounds))
                {
                    buildingArray[j].transform.localScale = new Vector3(Random.Range(0.3f, 0.6f), randH, Random.Range(0.3f, 0.6f));
                    buildingArray[j].transform.position = tempPos;
                }
            }

            if (buildingArray[j].GetComponent<Collider>().bounds.Intersects(gameObject.GetComponent<Collider>().bounds))
            {
                buildingArray[j].transform.Translate(0.0f, Time.deltaTime*0.1f, 0.0f);
            }
        }
    }

    private void Repositioning_Deprecated(GameObject[] buildingArray, GameObject[] roadArray)
    {
        for (int j = 0; j < numberObj; j++)
        {
            for (int i = j + 1; i < numberObj; i++)
            {
                for (int k = 0; k < numOfRoad; k++)
                {
                    if (buildingArray[j].GetComponent<Collider>().bounds.Intersects(buildingArray[i].GetComponent<Collider>().bounds)
                        || buildingArray[j].GetComponent<Collider>().bounds.Intersects(roadArray[k].GetComponent<Collider>().bounds))
                    {

                        Vector3 tempPos = new Vector3(Random.Range(-appearRange, appearRange), 0.0f, Random.Range(-appearRange, appearRange));

                        buildingArray[j].transform.position = tempPos;

                        float dist = (tempPos - center).magnitude;

                        buildingArray[j].transform.localScale = new Vector3(Random.Range(0.4f, 0.6f), Random.Range(2.5f - 0.5f * dist, 3.0f - 0.5f * dist), Random.Range(0.4f, 0.6f));

                    }
                }
            }

            if (buildingArray[j].GetComponent<Collider>().bounds.Intersects(gameObject.GetComponent<Collider>().bounds))
            {
                buildingArray[j].transform.Translate(0.0f, 0.1f * Time.deltaTime, 0.0f);
            }
        }
    }

    private void createBuildingBlocks(GameObject[] outObjArray)
    {
        Color genRandomCol;
        Vector3 randPosition;
        float dist;
        Vector3 randScale;

        for (int i = 0; i < numberObj; i++)
        {

            genRandomCol = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            randPosition = new Vector3(0.0f, 0.0f, 0.0f);

            dist = (randPosition - center).magnitude;
            randScale = new Vector3(Random.Range(0.4f, 0.6f), Mathf.Clamp(Random.Range(2.5f - 0.5f * dist, 3.0f - 0.5f * dist), 0.1f, 3.0f), Random.Range(0.4f, 0.6f));

            outObjArray[i] = GeneratePrimitiveMesh(PrimitiveType.Cube, "Building Block" +i, randPosition, randScale, genRandomCol, true);
        }
    }

    private void createRoadCollider(GameObject[] roadArray)
    {
        Color noColor = new Color(0.0f, 0.0f, 0.0f);
        Vector3 randScale = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 randPosition = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 3; i++)
        {



            //east-west dir road
            randPosition = new Vector3(-appearRange + Random.Range(0.5f, 1.5f)+ i* Random.Range(2.5f, 3.5f), 0.0f, 0.0f);
            randScale = new Vector3(Random.Range(0.4f, 0.5f), 0.1f, appearRange*2 );
            roadArray[i] = GeneratePrimitiveMesh(PrimitiveType.Cube, "Road Block" + i, randPosition, randScale, noColor, true);

            //south-north dir road
            randPosition = new Vector3(0.0f, 0.0f, -appearRange + Random.Range(0.5f, 1.5f) + i * Random.Range(2.5f, 3.5f));
            randScale = new Vector3(appearRange * 2, 0.1f, Random.Range(0.4f, 0.5f));
            roadArray[i+3] = GeneratePrimitiveMesh(PrimitiveType.Cube, "Road Block" + (i+3), randPosition, randScale, noColor, true);

        }
    }

    private GameObject GeneratePrimitiveMesh(PrimitiveType meshShape, string objName, Vector3 position, Vector3 scale, Color mtlCol, bool visible)
    {
        GameObject temp;
        temp = GameObject.CreatePrimitive(meshShape);
        temp.name = objName;
        temp.transform.position = position;
        temp.transform.localScale = scale;
        temp.GetComponent<Renderer>().material.color = mtlCol;
        temp.GetComponent<Renderer>().enabled = visible;

        return temp;
    }

}
