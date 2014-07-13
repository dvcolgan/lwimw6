using System;
using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Reverb : MonoBehaviour {

		private float sampleFrequency;
		void Awake(){
			sampleFrequency = AudioSettings.outputSampleRate;
			Initialize();
		}

		[SerializeField]
		[Range(0.5f,10)]
		private float _reverbTime = 1.55f;
		public float ReverbTime{
			get{
				return _reverbTime;
			}
			set{
				_reverbTime = Mathf.Clamp(value,0.5f,10);
				CallibrateCombFilters();
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _dryWet = 0.16f;
		public float DryWet{
			get{
				return _dryWet;
			}
			set{
				_dryWet = Mathf.Clamp(value,0,1);
			}
		}

		public CombFilter[] combFilters;
		public class CombFilter{
			private float l;
			public float gain = 0.7f;
			public float[,] delayBuffer;
			public float rvt;
			public float loopTime;

			public int pos = 0;

			public CombFilter(float r, float l, float sampleFrequency){
				rvt = r*1000;
				this.l = l;
				loopTime = l*((float)sampleFrequency/1000);
				gain = Mathf.Pow(0.001f,l/rvt);
				delayBuffer = new float[2,(int)loopTime];
			}

			public void Callibrate(float r){
				rvt = r*1000;
				gain = Mathf.Pow(0.001f,l/rvt);
			}

			public float ProcessSample(int channel, float sample){
				pos %= (int)loopTime;
				float output = delayBuffer[channel,pos];
				delayBuffer[channel,pos] = sample + delayBuffer[channel,pos] * gain;
				pos++;
				return output;
			}
		}
		public AllPassFilter[] allPassFilters;
		public class AllPassFilter{
			public float gain = 0.7f;
			public float[,] delayBuffer;
			public float rvt;
			public float loopTime;
			
			public int pos = 0;

			public AllPassFilter(float r, float l, float sampleFrequency){
				rvt = r;
				loopTime = l*((float)sampleFrequency/1000);
				gain = Mathf.Pow(0.001f,l/rvt);
				delayBuffer = new float[2,(int)loopTime];
			}

			public float ProcessSample(int channel, float sample){
				pos %= (int)loopTime;
				float output = delayBuffer[channel,pos];
				delayBuffer[channel,pos] = sample + delayBuffer[channel,pos] * gain;
				pos++;
				return output - gain*sample;
			}

		}

		void Initialize(){
			combFilters = new CombFilter[4];
			combFilters[0] = new CombFilter(ReverbTime,29.7f, sampleFrequency);
			combFilters[1] = new CombFilter(ReverbTime,37.1f, sampleFrequency);
			combFilters[2] = new CombFilter(ReverbTime,41.1f, sampleFrequency);
			combFilters[3] = new CombFilter(ReverbTime,43.7f, sampleFrequency);

			allPassFilters = new AllPassFilter[2];
			allPassFilters[0] = new AllPassFilter(96.83f,5.0f, sampleFrequency);
			allPassFilters[1] = new AllPassFilter(32.92f,1.7f, sampleFrequency);
		}

		void CallibrateCombFilters(){
			if(combFilters==null)return;
			for(var i = 0; i < combFilters.Length; i++){
				combFilters[i].Callibrate(ReverbTime);
			}
		}

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;
		private float ReverbTimePrev = 0;
		
		void SetRunEffectInEditMode(bool val){
			runEffectInEditMode = val;
			runEffect = val;
		}
		
		void Update(){
			if(!runEffectInEditMode&&!Application.isPlaying){
				runEffect = false;
				return;
			}
			runEffect = true;
			if(ReverbTimePrev != _reverbTime){
				ReverbTimePrev = _reverbTime;
				CallibrateCombFilters();
			}
		}
#endif
		
		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
#endif
			if(combFilters==null||allPassFilters==null){
				Initialize();
			}
			for (var i = 0; i < data.Length; i = i + channels){
				for (var c = 0; c < channels; c++){
					float combBase = data[i+c];
					for(var f = 0; f < combFilters.Length; f++){
						if(combFilters[f]==null) break;
						combBase += combFilters[f].ProcessSample(c,data[i+c]);
					}

					combBase /= combFilters.Length;

					float allPassBase = combBase / combFilters.Length;
					for(var a = 0; a < allPassFilters.Length; a++){
						allPassBase += allPassFilters[a].ProcessSample(c,combBase);
					}

					data[i+c] = data[i+c] * (1f-DryWet) + (allPassBase / allPassFilters.Length) * DryWet ;// allPassFilters.Length;
				}
			}
		}
	}
}