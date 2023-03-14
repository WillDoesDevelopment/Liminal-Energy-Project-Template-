using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyManager : MonoBehaviour
{
    public AudioSource backGroundMusic;

    public CustomMesh CM;
    public GameObject Camera;
    public int ValleyAmount;
    public GameObject CustomMeshObj;
    public GameObject[] ValleyObjs;

    public float gapAdjustment;
    // Start is called before the first frame update



    void Start()
    {



        Camera.transform.position = new Vector3(CM.MeshGridSize_x / 2, 5, 0);
        //customMesh.MeshGridLength;
        ValleyObjs = new GameObject[ValleyAmount];
        for (int i =0; i<ValleyObjs.Length; i++)
        {
            ValleyObjs[i] = Instantiate(CustomMeshObj, new Vector3(0, 0, i*(CM.MeshGridLength)), Quaternion.identity);

            if(i%2 == 0)
            {
                ValleyObjs[i].transform.localScale = new Vector3(1, 1, -1f);
                ValleyObjs[i].transform.position += new Vector3(0,0,CM.MeshGridLength);
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < ValleyObjs.Length; i++)
        {
            ValleyObjs[i].transform.position += new Vector3(0f, 0f, -10f * Time.deltaTime);

            if (ValleyObjs[i].transform.position.z < Camera.transform.position.z - CM.MeshGridLength )
            {
                //ValleyObjs[i].SetActive(false);
                ValleyObjs[i].transform.position = new Vector3(0,0,(CM.MeshGridLength) *(ValleyAmount-1));
            }
        }
    }


}
