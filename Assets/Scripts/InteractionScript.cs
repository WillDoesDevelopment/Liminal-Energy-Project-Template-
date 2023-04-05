using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
/*using Liminal.SDK.Input;
using Liminal.SDK.Examples;
using Liminal.SDK.Collections;
using Liminal.SDK.Core;
using Liminal.SDK.Build;
using Liminal.SDK.Editor;
using Liminal.SDK.Extensions;
using Liminal.SDK.OpenVR;
using Liminal.SDK.Serialization;
using Liminal.SDK.Tools;
using Liminal.SDK.XR;*/




public class InteractionScript : MonoBehaviour
{
    public GameObject InteractionObj;
    public GameObject InteractionFollow;
    public ParticleSystem particleSystem;
    public ParticleSystem particleSystemBurst;
    public ParticleSystemForceField particleSystemFF;


    public GameObject[] InteractionObjArr;



    public bool SpawnNew;

    public Vector3 IFStartPos;
    public Vector3 InteractionStartPos;

    // Start is called before the first frame update
    void Start()
    {
        IFStartPos = InteractionFollow.transform.position;
        InteractionStartPos = InteractionObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InteractionLifetime();

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("Working");
            Destroy(InteractionObj);
        }
    }

    public void InteractionLifetime()
    {
        if (InteractionObj != null)
        {

            if (InteractionObj.transform.position.z > 250)
            {
                InteractionFollow.transform.position += new Vector3(0, Mathf.Sin(Time.time * 5) / 10, -100 * Time.deltaTime);
                InteractionObj.transform.position = Vector3.Lerp(InteractionObj.transform.position, InteractionFollow.transform.position, 0.2f);
            
            }
            else
            {
                //AlphaClipVal += 0.02f;
                
                InteractionObj.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                //particleSystem.emission.SetBurst(0, new ParticleSystem.Burst(1.0f, 150, 200) );
                //InteractionObj.GetComponent<Renderer>().material.SetFloat("_AlphaClip", 0);
                if (InteractionObj.transform.localScale.z >= 7)
                {

                    particleSystemBurst.gameObject.transform.position = InteractionObj.transform.position;
                    InteractionObj.transform.position = InteractionStartPos;
                    InteractionObj.transform.localScale = new Vector3(1.6f,1.6f,1.6f);

                    InteractionFollow.transform.position = IFStartPos;
                    particleSystemBurst.gameObject.SetActive(true);
                    particleSystemBurst.Play();
                }
            }

        }
    }
}
