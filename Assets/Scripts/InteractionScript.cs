using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public GameObject InteractionObj;
    public GameObject InteractionFollow;
    public GameObject[] InteractionObjOrbits;
    public ParticleSystem particleSystem;
    public ParticleSystemForceField particleSystemFF;


    public float[] RandRot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InteractionFollow.transform.position = new Vector3(0, 0, 10 * Time.deltaTime);
        InteractionObj.transform.position = Vector3.Lerp(InteractionObj.transform.position, InteractionFollow.transform.position, 0.2f);
    }
}
