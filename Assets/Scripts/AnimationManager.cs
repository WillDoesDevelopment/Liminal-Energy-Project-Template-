using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] public Animator SunRiseAnim;
    public GameObject SunObj;
    private int gameDuration = 170;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SunSpinInput();
    }



    public void SunSpinInput()
    {
        SunObj.transform.position += new Vector3(0, Time.time / 135/170);
        SunObj.transform.GetChild(0).transform.eulerAngles += new Vector3(0,50*Time.deltaTime,0);
        SunObj.transform.GetChild(1).transform.eulerAngles += new Vector3(0,50*Time.deltaTime,0);
    }
}
