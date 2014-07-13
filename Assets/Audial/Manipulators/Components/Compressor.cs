using UnityEngine;
using System;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Compressor : MonoBehaviour {

		private float sampleFrequency;
		void Awake(){
			sampleFrequency = AudioSettings.outputSampleRate;
		}

		[SerializeField]
		[Range(0,3)]
		private float _inputGain = 1;
		public float InputGain{
			get{
				return _inputGain;
			}
			set{
				_inputGain = Mathf.Clamp(value,0,3);
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _threshold = 0.247f;
		public float Threshold{
			get{
				return _threshold;
			}
			set{
				_threshold = Mathf.Clamp(value,0,1);
			}
		}

		[SerializeField]
		[Range(0,2)]
		public float _slope = 1.727f;
		public float Slope{
			get{
				return _slope;
			}
			set{
				_slope = Mathf.Clamp(value,0,2);
			}
		}

		[SerializeField]
		[Range(0.0001f,1)]
		private float _attack = 0.0001f;
		private float _attackMod;
		public float Attack{
			get{
				return _attack;
			}
			set{
				_attack = Mathf.Clamp(value, 0.0001f, 1);
				_attackMod = Mathf.Exp(-1/(sampleFrequency * _attack));
			}
		}

		[SerializeField]
		[Range(0.0001f,1)]
		public float _release = 0.68f;
		private float _releaseMod;
		public float Release{
			get{
				return _release;
			}
			set{
				_release = Mathf.Clamp(value, 0.0001f, 1);
				_releaseMod = Mathf.Exp(-1/(sampleFrequency * _release));
			}
		}

		[SerializeField]
		[Range(0,5)]
		private float _outputGain = 1;
		public float OutputGain{
			get{
				return _outputGain;
			}
			set{
				_outputGain = Mathf.Clamp(value, 0, 5);
			}
		}

		private void Callibrate(){
			_attackMod = Mathf.Exp (-1/(sampleFrequency * _attack));
			_releaseMod = Mathf.Exp(-1/(sampleFrequency * _release));
		}

		private float env = 0;

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;
		
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
			Callibrate();
		}
#endif
		
		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
#endif
			for(var i = 0; i < data.Length; i += channels){

				data[i] *= InputGain;
				data[i+1] *= InputGain;

				float rms = 0.5f * data[i] + 0.5f * data[i+1];

				float theta = rms > env ? _attackMod : _releaseMod;

				env = (1-theta) * rms + theta * env;

				float gain = 1;

				if(env > Threshold)
					gain = Mathf.Clamp(gain - (env - Threshold) * Slope, 0, 1);

				data[i] *= gain * OutputGain;
				data[i+1] *= gain * OutputGain;
			}
		}

	}
}