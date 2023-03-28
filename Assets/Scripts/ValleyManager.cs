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

    public GameObject SunObj;

    public Material Mat;    

    public float gapAdjustment;
    // Start is called before the first frame update



    void Start()
    {



        Camera.transform.position = new Vector3(CM.MeshGridSize_x / 2,10, 150);
        //CM.MeshGridLength;
        ValleyObjs = new GameObject[ValleyAmount];
        SunObj.transform.position = new Vector3(CM.MeshGridSize_x / 2-7, -45, ValleyObjs.Length * CM.MeshGridLength );
        for (int i =0; i<ValleyObjs.Length; i++)
        {
            ValleyObjs[i] = Instantiate(CustomMeshObj, new Vector3(0, 0, i*(CM.MeshGridLength)- 1), Quaternion.identity);
            ValleyObjs[i].GetComponent<Renderer>().material = Mat;
            //ValleyObjs[i].transform.localEulerAngles = new Vector3(-0.01f, 0, 0);
            /*if(i%2 == 0)
            {
                ValleyObjs[i].transform.localScale = new Vector3(1, 1, -1f);
                ValleyObjs[i].transform.position += new Vector3(0,0,CM.MeshGridLength);
            }*/

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < ValleyObjs.Length; i++)
        {
            ValleyObjs[i].transform.position += new Vector3(0f, 0f, -10f * Time.deltaTime);

            if (ValleyObjs[i].transform.position.z < 0f )
            {
                //ValleyObjs[i].SetActive(false);
                ValleyObjs[i].transform.position = new Vector3(0,0,(CM.MeshGridLength-0.005f) *(ValleyAmount-1));
            }
        }
    }


}
