using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public GameObject top, bottom, left, right, front, back;
    public int sideLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* front.transform.position = new Vector3(0, 0, sideLength / 2);
        back.transform.position = new Vector3(0, 0, -sideLength / 2);
        left.transform.position = new Vector3(sideLength / 2, 0, 0);
        right.transform.position = new Vector3(-sideLength / 2, 0, 0);
        top.transform.position = new Vector3(0, sideLength / 2, 0);
        bottom.transform.position = new Vector3(0, -sideLength / 2, 0);

        front.transform.rotation = Quaternion.Euler(0, 0, 0); */
    }
}
