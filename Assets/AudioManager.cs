using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backGroundMusic;
    public Material ShaderMat;

    private float[] samples;
    private float PowerMultiplier = 100f;

    private float[] MergeChanels(float[] samples, int Channels, int nSamples )
    {
        float[] newSamples = new float[nSamples];
        for (int i = 0; i < (nSamples); i+=2)
        {
            float total = 0;
            for (int c = 0; c < Channels; c++)
            {
                float v = samples[i * Channels + c];
                total += v * v;
            }
            newSamples[i] = total;

        }
        Debug.Log(newSamples.Length);
        Debug.Log(newSamples[newSamples.Length/2]);
        return newSamples;
    }
    // Start is called before the first frame update
    void Start()
    {
        float frequency = backGroundMusic.clip.frequency;
        int nsamples = backGroundMusic.clip.samples;
        float length = backGroundMusic.clip.length;
        int nchannels = backGroundMusic.clip.channels;

        samples = new float[backGroundMusic.clip.samples];
        Debug.Log(frequency);
        Debug.Log(nsamples);
        Debug.Log(length);


        float[] Tempsamples = new float[nsamples * nchannels];
        backGroundMusic.clip.GetData(samples, 0);
        Debug.Log(samples[48000 * 3] * 1000000);
        //samples = MergeChanels(Tempsamples, nchannels,nsamples);
        Debug.Log(samples[48000 * 3] * 1000000);


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(samples[Mathf.RoundToInt(backGroundMusic.time * backGroundMusic.clip.frequency)]);
        ShaderMat.SetFloat("_MusicInput", Mathf.Pow( samples[Mathf.RoundToInt(backGroundMusic.time * backGroundMusic.clip.frequency*2)],5));
    }
}
