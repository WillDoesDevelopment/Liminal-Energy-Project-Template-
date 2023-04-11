using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;





public class InteractionScript : MonoBehaviour
{
    public GameObject InteractionObj;
    public GameObject InteractionFollow;

    public Texture2D Fourrier;
    public AudioSource BackgroundMusic;

    public GameObject[] InteractionObjArr;

    public GameObject LeftHandObj;
    public GameObject RightHandObj;

    public bool Interacted = false;
    public bool SpawnCoolDown = false;
    
    public Vector3 InteractionStartPos;

    public float[] threshholds = new float[10];
    public int testChanell;
    // Start is called before the first frame update
    void Start()
    {
        InteractionStartPos = InteractionObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();    
        var device = VRDevice.Device;
        var rightHand = device.PrimaryInputDevice;
        var leftHand = device.SecondaryInputDevice;
        if (rightHand.GetButton(VRButton.Primary))
        {
            Ray ray = new Ray(RightHandObj.transform.position, RightHandObj.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200))
            {
                Interacted = true;
                if(hit.transform.GetComponentInParent<InteractionObjScript>() != null)
                {
                    hit.transform.GetComponentInParent<InteractionObjScript>().Interacted = true;
                }
            }
        }
        else if (leftHand.GetButton(VRButton.Primary))
        {
            Ray ray = new Ray(LeftHandObj.transform.position, LeftHandObj.transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200))
            {
                Debug.Log(hit.transform.gameObject);
                Interacted = true;
                if (hit.transform.GetComponentInParent<InteractionObjScript>() != null)
                {
                    hit.transform.GetComponentInParent<InteractionObjScript>().Interacted = true;
                }
            }
        }
    }

    private void Spawn()
    {
        float[] MusicValArr = new float[10];

        for (int i = 0; i < 10; i++)
        {
            Color MusicVal1 = Fourrier.GetPixel(Mathf.RoundToInt(BackgroundMusic.time * BackgroundMusic.clip.frequency/1024), i, 0);
            Color MusicVal2 = Fourrier.GetPixel(Mathf.RoundToInt(BackgroundMusic.time * BackgroundMusic.clip.frequency / 1024) + 1, i, 0);
            Color MusicVal0 = Fourrier.GetPixel(Mathf.RoundToInt(BackgroundMusic.time * BackgroundMusic.clip.frequency / 1024 ) - 1, i, 0);

            float Rval0 = Mathf.Pow(MusicVal0.r, 2);
            float Rval1 = Mathf.Pow(MusicVal1.r, 2);
            float Rval2 = Mathf.Pow(MusicVal2.r, 2);


            //if (true)
            if (Rval1 > Rval2 && Rval1 > Rval0 && SpawnCoolDown == false && Rval1 > 0.04 )
            {
                var NewObj = Instantiate(InteractionObj, this.transform.position + new Vector3(Random.Range(-5, 5), 0, 0), Quaternion.identity);
                NewObj.SetActive(true);
                NewObj.GetComponent<InteractionObjScript>().Follow = Instantiate(InteractionFollow, this.transform.position + new Vector3(Random.Range(-5, 5), 40 + Random.Range(-5, 5), 0), Quaternion.identity);
                SpawnCoolDown = true;
                StartCoroutine(CoolDown());
            }
        }
        //Debug.Log(Mathf.Pow(MusicVal.r,2));
    }
    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.5f);

        SpawnCoolDown = false;
    }
 
}
