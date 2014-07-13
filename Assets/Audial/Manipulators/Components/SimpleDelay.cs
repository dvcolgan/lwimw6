using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{

	[ExecuteInEditMode]
	public class SimpleDelay : MonoBehaviour {
		
		private float sampleFrequency;
		void Awake(){
			sampleFrequency = AudioSettings.outputSampleRate;
			ChangeDelay();
		}

		private float[,] delayBuffer;
		private int index = 0;

		[SerializeField]
		[Range(10,3000)]
		private int _delayLengthMS = 120;
		private int DelayLengthMSPrev = 10;
		public int DelayLengthMS {
			get{
				return _delayLengthMS;
			}
			set{
				_delayLengthMS = Mathf.Clamp(value, 10, 3000);
				ChangeDelay();
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _dryWet = 0.5f;
		public float DryWet{
			get{
				return _dryWet;
			}
			set{
				_dryWet = Mathf.Clamp(value,0,1);
			}
		}

		[SerializeField]
		[Range(0.1f,1)]
		private float _decayLength = 0.25f;
		public float DecayLength{
			get{
				return _decayLength;
			}
			set{
				_decayLength = Mathf.Clamp(value,0.1f, 1);
			}
		}
		
		private float delayLength;
		private int delaySamples;
		private float output = 0;
		
		private void ChangeDelay(){
			delaySamples = (int)Mathf.Round((float)DelayLengthMS*sampleFrequency/1000);
			delayBuffer = new float[2,delaySamples];
		}

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;
		private float BPMPrev = 0;
		private float SimpleDelayCountPrev = 0;
		private float SimpleDelayUnitPrev = 0;

		void SetRunEffectInEditMode(bool val){
			runEffectInEditMode = val;
			runEffect = val;
		}

		void ClearBuffer(){
			delayBuffer = null;
		}

		void Update(){
			if(!runEffectInEditMode&&!Application.isPlaying){
				runEffect = false;
				ClearBuffer();
				return;
			}
			runEffect = true;
			if(DelayLengthMSPrev != DelayLengthMS){
				DelayLengthMSPrev = DelayLengthMS;
				ChangeDelay();
			}
		}
#endif

		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
#endif
			if(delayBuffer==null){
				ChangeDelay();
			}

			float dry;
			float wet;

			float[] tempDelay = new float[channels];

			for (var i = 0; i < data.Length; i = i + channels){
				index %= delaySamples;
				for (var c = 0; c < channels; c++){
					tempDelay[c] = delayBuffer[c,index];
					delayBuffer[c,index] = 0;

					dry = data[i+c];
					wet = tempDelay[c];
					output = dry * (1-DryWet) + wet * DryWet;
					data[i+c] = (float)(output);

					delayBuffer[c, index] += wet * DecayLength;
					delayBuffer[c,index] += dry;
				}

				index++;		
			}
		}


	}
}