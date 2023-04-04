using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.Core;
public class InteractionScript : MonoBehaviour
{
    public GameObject[] Hands;
    public GameObject InteractionObj;
    public GameObject InteractionFollow;
    public ParticleSystem particleSystem;
    public ParticleSystemForceField particleSystemFF;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InteractionMovment();

        //var device = VRDevice.Device;
        //var rightHand = device.PrimaryHand;

        //var ray = new Ray();
    }
    
    public void InteractionMovment()
    {
        InteractionFollow.transform.position += new Vector3(0, Mathf.Sin(Time.time * 5) / 5, -50 * Time.deltaTime);
        InteractionObj.transform.position = Vector3.Lerp(InteractionObj.transform.position, InteractionFollow.transform.position, 0.02f);

        if (InteractionObj.transform.position.z < 0)
        {
            InteractionObj.transform.localScale -= new Vector3(.01f, .01f, .01f);
            if (InteractionObj.transform.localScale.x < 0)
            {
                Destroy(InteractionObj);
                Destroy(InteractionFollow);
            }
        }
    }
}
