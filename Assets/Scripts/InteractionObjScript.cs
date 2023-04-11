using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjScript : MonoBehaviour
{
    public GameObject Follow;

    public ParticleSystem particleSystemBurst;
    public bool Interacted = false;
    // Start is called before the first frame update
    void Start()
    {
        particleSystemBurst.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Interacted == false)
        {
            movement();
        }
        else
        {
            Explode();
        }
    }
    public void Explode()
    {
        Interacted = true;
        this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        if (this.transform.localScale.z >= 7)
        {
            GameObject TempParticle = Instantiate(particleSystemBurst.gameObject, this.transform.position, Quaternion.identity);
            TempParticle.SetActive(true);
            //particleSystemBurst.gameObject.transform.position = this.transform.position;

            TempParticle.GetComponent<ParticleSystem>().Play();
            Destroy(this.gameObject);
            Destroy(Follow);
            StartCoroutine(DestroyTemp(TempParticle));

        }

    }
    public IEnumerator DestroyTemp(GameObject temp)
    {
        yield return new WaitForSeconds(4);
        Destroy(temp);

    }

    public void movement()
    {

        Follow.transform.position += new Vector3(0, Mathf.Sin(Time.time * 5) / 10, -100 * Time.deltaTime);
        this.transform.position = Vector3.Lerp(this.transform.position, Follow.transform.position, 0.2f);
        if (this.transform.position.z < 0)
        {
            this.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);

            if (this.transform.localScale.z <= 0)
            {
                Destroy(this.gameObject);
                Destroy(Follow);

            }
        }  
    }
}
