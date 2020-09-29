using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform playerTrans;
    Transform cameraTrans;
    // Start is called before the first frame update
    void Start()
    {
        cameraTrans = gameObject.transform;
        //playerTrans = GameObject.Find("choppy_emp").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameraTrans.position = playerTrans.position + new Vector3(0.0f, 5.0f, -15.0f);
    }
}
