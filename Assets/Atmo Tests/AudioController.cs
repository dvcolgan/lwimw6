using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{

    [System.Serializable]
    public class Pattern
    {
        public int[] sequence;
        public Pattern (int[] sequence)
        {
            this.sequence = sequence;
        }
        public Pattern ()
        {
            sequence = new int[16];
        }
    }
    public Pattern pattern = new Pattern ();

    public GameObject[] gameObjects;
    public void BeatTriggers (int beat)
    {
        if (gameObjects != null) {
            if (pattern.sequence [beat] != 0) {
                for (var g = 0; g < gameObjects.Length; g++) {
                    gameObjects [g].BroadcastMessage ("BeatTrigger", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    private int[] triggers;

    public int currentSample = 0;
    public int nextTargetSample = 0;

    public AudioClip sample;
    public int sampleIndex = 0;
    private float[] sampleData;

    private int bufferLength;

    void Awake ()
    {
        bufferLength = 3 * AudioSettings.outputSampleRate;
        triggers = new int[bufferLength];
        AudioClipToFloat ();
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Space)) {
            sampleIndex = 0;
        }
    }

    void AudioClipToFloat ()
    {
        sampleData = new float[sample.samples];
        sample.GetData (sampleData, 0);
    }

    public void AddTrigger (int index, int val)
    {
        triggers [index % bufferLength] = val;
    }

    float[] PlayFromFloat (int channels)
    {
        if (sampleIndex >= sampleData.Length) {
            return new float[2]{0,0};
        }
        float[] retVal = new float[channels];
        for (var c = 0; c<channels; c++) {
            retVal [c] = sampleData [sampleIndex + c];
        }
        sampleIndex += channels;
        return retVal;
    }

    void OnAudioFilterRead (float[] data, int channels)
    {
        for (var i = 0; i < data.Length; i+=channels) {
            if (triggers [currentSample] != 0) {
                if (pattern.sequence [triggers [currentSample] % 16] == 1) {
                    sampleIndex = 0;
                    triggers [currentSample] = 0;
                }
            }
            float[] audioFloat = PlayFromFloat (channels);
            for (var c = 0; c<channels; c++) {
                data [i + c] += audioFloat [c];
            }
            currentSample = (currentSample + 1) % bufferLength;
        }
    }
}
