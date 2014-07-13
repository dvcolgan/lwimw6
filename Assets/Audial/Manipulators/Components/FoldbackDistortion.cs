using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class FoldbackDistortion : MonoBehaviour {

		[SerializeField]
		[Range(0,3)]
		private float _inputGain = 1.14f;
		public float InputGain{
			get{
				return _inputGain;
			}
			set{
				_inputGain = Mathf.Clamp(value, 0, 3);
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _softDistortAmount = 0.177f;
		private float softThreshold = 0.002f;
		public float SoftDistortAmount{
			get{
				return _softDistortAmount;
			}
			set{
				_softDistortAmount = Mathf.Clamp(value, 0, 1);
			}
		}

		[SerializeField]
		[Range(0.000001f,1)]
		private float _threshold = 0.244f;
		public float Threshold{
			get{
				return _threshold;
			}
			set{
				_threshold = Mathf.Clamp(value, 0.000001f, 1);
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _distortAmount = 0.904f;
		public float DistortAmount{
			get{
				return _distortAmount;
			}
			set{
				_distortAmount = Mathf.Clamp(value, 0, 1);
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

		private float foldBack(float sample, float threshold){
			if(Mathf.Abs(sample)>Threshold){
				return (Mathf.Abs(Mathf.Abs(sample - Threshold % (Threshold * 4)) - Threshold * 2) - Threshold) + 0.3f*sample;
			}else{
				return sample;
			}
		}

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
		}
#endif
		
		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
#endif
			for (var i = 0; i < data.Length; i = i + channels){
				for(var c = 0; c < channels; c++){
					data[i+c] *= InputGain;

					float softDistort = foldBack(data[i+c], softThreshold);
					data[i+c] = (1-SoftDistortAmount)*data[i+c] + SoftDistortAmount*softDistort;
					data[i+c] *= OutputGain;

					float hardDistort = foldBack(data[i+c], Threshold);
					data[i+c] = (1-DistortAmount)*data[i+c] + DistortAmount * hardDistort;
					data[i+c] *= OutputGain;

				}
			}
		}
	}
}