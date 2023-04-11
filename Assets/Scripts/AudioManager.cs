using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.IO;

public class AudioManager : MonoBehaviour
{
    public AudioSource backGroundMusic;
    public Material ShaderMat;

    public CustomMesh CM;
    private float[] samples;
    private float PowerMultiplier = 100f;

    public Texture2D texture;

    private Complex[] Sample(float[] samples, int Channels, int beginning, int length )
    {
        Complex[] newSamples = new Complex[length];
        for (int i = 0; i < length; i++)
        {
            int pos = beginning + i;
            double v = 0;
            for(int c=0; c < Channels; c++)
            {
                v += (double)samples[Channels * pos + c];
            }

            // array of complex numbers with imaginary value zero ready for fourier transform            
            newSamples[i] = v;
        }

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

    public static Complex[] FFT(Complex[] input, bool invert)
    {
        //in case there is only one element
        if (input.Length == 1)
        {
            return new Complex[] { input[0] };
        }

        //for more elements we need to otain the lenght of the input data stream
        int length = input.Length;

        //half will be the half of the lenght
        int half = length / 2;

        //this is the result of the FFT
        Complex[] result = new Complex[length];

        // factor that goes in the
        double factorEXP = -2.0 * System.Math.PI / length;

        //in case we want to invert the factor
        if (invert)
        {
            factorEXP = -factorEXP;
        }


        //
        // Cooley–Tukey algorithm. This is a divide and conquer algorithm that recursively breaks down a DFT of any composite size N = N1N2 into many smaller DFTs of sizes N1 and N2,
        // it is divided into even and odd components
        //

        //even
        Complex[] evens = new Complex[half];
        for (int i = 0; i < half; i++)
        {
            evens[i] = input[2 * i];
        }
        //FFT recursive call
        Complex[] evenResult = FFT(evens, invert);

        //odd
        Complex[] odds = evens;
        for (int i = 0; i < half; i++)
        {
            odds[i] = input[2 * i + 1];
        }
        // FFT recursive call
        Complex[] oddResult = FFT(odds, invert);


        // final algorithm
        //          N/2-1                                N/2-1
        //   FFT_k= SUM  X_2n ·e^(-2*pi*(2n)*k)/(N/2)  +  SUM  X_2n+1 ·e^(-2*pi*(2n+1)*k)/(N/2) 
        //           0                                    0
        //
        // = Even_k + O_k·e^(-2*pi**k)/(N)

        for (int k = 0; k < half; k++)
        {
            double factor_K = factorEXP * k;

            //                     odd part   &  this is the second part that is added module 1 argument factor_k
            Complex oddComponent = oddResult[k] * new Complex(1 * System.Math.Cos(factor_K), 1 * System.Math.Sin(factor_K));

            //first part of the chart
            result[k] = evenResult[k] + oddComponent;
            //second part of the chart
            result[k + half] = evenResult[k] - oddComponent;
        }

        //reutrn the values (complex). To show FFT we need to display module or "abs" of the complex number
        return result;
    }


    void Start()
    {
       



        ShaderMat.SetFloat("_MusicInput", 5);

        float frequency = backGroundMusic.clip.frequency;
        int nsamples = backGroundMusic.clip.samples;
        float length = backGroundMusic.clip.length;
        int nchannels = backGroundMusic.clip.channels;

        samples = new float[nsamples*nchannels];
        backGroundMusic.clip.GetData(samples, 0);

/*        Debug.Log("array length" + samples.Length);
        Debug.Log("freq" + frequency);
        Debug.Log( "channels" + nchannels);
        Debug.Log("samples" + nsamples);
        Debug.Log("Duration" + length);
        Debug.Log("freq*channels*duration" + frequency*nchannels*length);*/


        //Debug.Log(samples[48000 * 3] * 1000000);
        //samples = MergeChanels(samples, nchannels,nsamples,0,2048);
        //Debug.Log(samples[48000 * 3] * 1000000);

        int logsamplelength = 10;
        int numsamples = 1 << logsamplelength;
        //int sampledist = (int) frequency / 100;
        int sampledist = numsamples;
        //int[] freqbands = { 40, 56, 65, 82, 90, 108, 115, 124, 140, 160, 260 };
        int[] freqbands = { 21, 25, 30, 36, 42, 50, 60,72, 84,100, 144 };
        float[] freqscale = { 1f, 1f, 0.1f, 0.001f, };

        int numtiles = 1 + (nsamples - numsamples) / sampledist;

        texture = new Texture2D(numtiles, 10, TextureFormat.ARGB32, false);
        float[,] energyArr = new float[numtiles, 10];

        double[] MaxE = new double[10] { 0,0,0,0,0,0,0,0,0,0 };

       /* for (int t = 0; t < numtiles; t++)
        {
            Complex[] data = Sample(samples, nchannels, t * sampledist, numsamples);
            Complex[] Freq = FFT(data, false);
            //Complex[] Freq = data;

            for (int f = 0; f < 10; f++)
            {
                double energy = 0;
                for (int f1 = freqbands[f]; f1 < freqbands[f + 1]; f1++)
                {
                    double M = Freq[f1].Magnitude;
                    energy += M * M;
                }
                //Color color = new Color(Mathf.Sin((t+f)/10), 0,0, 1);
                //Color color = new Color((float)(0.00001 * energy), 0, 0, 1);

                //texture.SetPixel(t,f, color);
                energyArr[t, f] = (float) energy;
                if (energy > MaxE[f])
                {
                    MaxE[f] = energy;
                }

            }
        }
        for(int t=0; t<numtiles; t++)
        {
            for(int f=0; f<10; f++)
            {
                Color color = new Color((float)(2*(energyArr[t,f])/MaxE[f]), 0, 0, 1);
                texture.SetPixel(t, f, color);
            }
        }
        //texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/../Saved Fourrier Images/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + "1K_5K_100sps_FourierTexture2D_new2" + ".png", bytes);
*/

        //ShaderMat.SetTexture("_musicTexture", texture);
    }
    void Update()
    {
            //Debug.Log(backGroundMusic.time);
            //ShaderMat.SetFloat("_MusicInput",samples[Mathf.RoundToInt(backGroundMusic.time * backGroundMusic.clip.frequency/2)]*50);
         
    }
}
