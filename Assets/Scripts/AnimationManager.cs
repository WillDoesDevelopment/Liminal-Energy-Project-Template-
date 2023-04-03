using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] public Animator SunRiseAnim;
    public GameObject SunObj;
    private int gameDuration = 170;
    public AudioSource BackgroundMusic;

    public Texture2D Fourrier;

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
        Color MusicVal = Fourrier.GetPixel(Mathf.RoundToInt(BackgroundMusic.time * 10), 1, 0);

        float scaleVal = MusicVal.r*1+1;

        if(BackgroundMusic.time != 0)
        {
            SunObj.transform.position = new Vector3(SunObj.transform.position.x, (BackgroundMusic.time/ BackgroundMusic.clip.length)*198-63, SunObj.transform.position.z);

        }
        SunObj.transform.GetChild(0).transform.eulerAngles += new Vector3(0,50*Time.deltaTime,0);
        SunObj.transform.GetChild(1).transform.eulerAngles += new Vector3(0,50*Time.deltaTime,0);

    }
}
