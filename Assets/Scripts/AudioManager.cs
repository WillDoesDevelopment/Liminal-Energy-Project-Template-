using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public AudioSource backGroundMusic;
    public Material ShaderMat;

    public CustomMesh CM;
    private float[] samples;
    private float PowerMultiplier = 100f;

    public Texture2D texture;

    private double[] Sample(float[] samples, int Channels, int beginning, int length )
    {
        double[] newSamples = new double[2*length];
        for (int i = 0; i < length; i++)
        {
            double v = 0;
            for(int c=0; c < Channels; c++)
            {
                v += (double)samples[Channels * i + c];
            }

            // array of complex numbers with imaginary value zero ready for fourier transform            
            newSamples[2*i] = v;
            newSamples[2*i + 1] = 0;

        }
        //Debug.Log("Merged sample length" + newSamples.Length );
        //Debug.Log("middle val" + newSamples[newSamples.Length/2]);
        return newSamples;
    }
    // Start is called before the first frame update
    private int NextPof2(int val)
    {

        return Mathf.CeilToInt(Mathf.Log(val, 2));
    }

    private uint ReverseBits(uint n)
    {
        n = (n >> 1) & 0x55555555 | (n << 1) & 0xaaaaaaaa;
        n = (n >> 2) & 0x33333333 | (n << 2) & 0xcccccccc;
        n = (n >> 4) & 0x0f0f0f0f | (n << 4) & 0xf0f0f0f0;
        n = (n >> 8) & 0x00ff00ff | (n << 8) & 0xff00ff00;
        n = (n >> 16) & 0x0000ffff | (n << 16) & 0xffff0000;
        return n;
    }

    private void Forrier(double[] complexArr, int LogNumElements, int Direction)
    {
        uint numElements = (uint)1 << LogNumElements;
        //swap elements (decimation in time)
        for(uint i = 0; i< numElements; i++)
        {
            uint newPos = ReverseBits(i) >> (32 - LogNumElements);
            if(i< newPos)
            {
                double temp = complexArr[i * 2];
                complexArr[i * 2] = complexArr[newPos * 2];
                complexArr[newPos * 2] = temp;

                temp = complexArr[i * 2 + 1];
                complexArr[i * 2+1] = complexArr[newPos * 2+1];
                complexArr[newPos * 2+1] = temp;
            }
        }
        uint n = numElements << 1;
        for (uint mMax = 2; mMax< n; mMax <<= 1)
        {
            uint iStep = mMax << 1;
            double theta = Direction * (System.Math.PI / mMax);
            double wTemp = System.Math.Sin(0.5 * theta);
            double wPr = -2 * wTemp * wTemp;
            double wPi = System.Math.Sin(theta);
            double wR = 1;
            double wI = 0;

            for (uint m = 0; m < mMax; m += 2)
            {
                for(uint i = m; i<n; i += iStep)
                {
                    uint j = i + mMax;
                    double tempR = wR * complexArr[j] - wI*complexArr[j+1];
                    double tempI = wR * complexArr[j + 1] + wI * complexArr[j];
                    complexArr[j] = complexArr[i] - tempR;
                    complexArr[j + 1] = complexArr[i + 1] - tempI;
                    complexArr[i] += tempR;
                    complexArr[i + 1] += tempI;
                }
                wTemp = wR;
                wR += wR*wPr-wI*wPi;
                wI += wI * wPr + wTemp * wPi;
            }
        }
    }
    void Start()
    {
       



        ShaderMat.SetFloat("_MusicInput", 5);

        float frequency = backGroundMusic.clip.frequency;
        int nsamples = backGroundMusic.clip.samples;
        float length = backGroundMusic.clip.length;
        int nchannels = backGroundMusic.clip.channels;

        //samples = new float[backGroundMusic.clip.samples];
        samples = new float[nsamples*nchannels];
        backGroundMusic.clip.GetData(samples, 0);

  /*      Debug.Log("array length" + samples.Length);
        Debug.Log("freq" + frequency);
        Debug.Log( "channels" + nchannels);
        Debug.Log("samples" + nsamples);
        Debug.Log("Duration" + length);
        Debug.Log("freq*channels*duration" + frequency*nchannels*length);
*/

        //Debug.Log(samples[48000 * 3] * 1000000);
        //samples = MergeChanels(samples, nchannels,nsamples,0,2048);
        //Debug.Log(samples[48000 * 3] * 1000000);

        int logsamplelength = 13;
        int numsamples = 1 << 13;
        int sampledist = (int) frequency / 10;
        int[] freqbands = { 43, 60, 86, 120, 172, 240, 342, 480, 684, 960, 1368 };

        int numtiles = 1 + (nsamples - numsamples) / sampledist;

        texture = new Texture2D(numtiles, 10, TextureFormat.ARGB32, false);
        //texture = new Texture2D(2048, 16, TextureFormat.ARGB32, false);

        for(int t=0; t < numtiles; t++)
        {
            double[] data = Sample(samples, nchannels, t*sampledist, numsamples);
            Forrier(data, logsamplelength, 1);
            //double[] freqenergy = new double[10];
            for (int f = 0; f < 10; f++)
            {
                double energy = 0;
                for (int f1 = freqbands[f]; f1 < freqbands[f + 1]; f1++)
                {
                    energy += data[2 * f1] * data[2 * f1] + data[2 * f1 + 1] * data[2 * f1 + 1];
                }
                Color color = new Color((float)( energy), 0,0, 1);
                Debug.Log((float)(energy));
                //texture.SetPixel(t,f, new Vector4((float)(10 * energy), (float)(10 * energy), (float)(10 * energy), (float)(10 * energy)));
                texture.SetPixel(t,f, color);
            }
        }
        
        texture.Apply();
        ShaderMat.SetTexture("_musicTexture", texture);
    }

    // Update is called once per frame
    void Update()
    {
            //Debug.Log(backGroundMusic.time);
            //ShaderMat.SetFloat("_MusicInput",samples[Mathf.RoundToInt(backGroundMusic.time * backGroundMusic.clip.frequency/2)]*50);
         
    }
}
