using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MyLevelGeneration : MonoBehaviour
{

    [SerializeField] Vector3 startPoint;
    [SerializeField] Vector3 endPoint;

    [Range(0, 1)]
    [SerializeField]
    float movementFactor;

    [SerializeField] GameObject inPrefab;
    private GameObject levelWall_1, levelWall_2;

    // Start is called before the first frame update
    void Start()
    {
        randomGeverteLevel();
    }

    // Update is called once per frame
    void Update()
    {
        updateLevel();
    }

    private void randomGeverteLevel()
    {
        startPoint = new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(1.0f, 15.0f), Random.Range(-5.0f, 5.0f));
        endPoint = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(-1.0f, 15.0f), Random.Range(-5.0f, 5.0f));
        levelWall_1 = Instantiate(inPrefab, startPoint, Quaternion.identity);
        levelWall_2 = Instantiate(inPrefab, startPoint + new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity);
    }
    private void updateLevel()
    {
        movementFactor = Mathf.Sin(Time.time);
        levelWall_1.transform.position = startPoint + movementFactor * (endPoint - startPoint);
        levelWall_2.transform.position += 0.1f * new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }
}
