using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Crusher : MonoBehaviour {
		[SerializeField]
		[Range(1,32)]
		private int _bitDepth = 8;
		private int m;
		public int BitDepth{
			get{
				return _bitDepth;
			}
			set{
				_bitDepth = Mathf.Clamp(value, 1,32);
				m = 1<<(_bitDepth-1);
			}
		}

		[SerializeField]
		[Range(0.001f,1)]
		private float _sampleRate = 0.1f;
		public float SampleRate{
			get{
				return _sampleRate;
			}
			set{
				_sampleRate = Mathf.Clamp(value, 0.001f,1);
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _dryWet = 1;
		public float DryWet{
			get{
				return _dryWet;
			}
			set{
				_dryWet = Mathf.Clamp(value,0,1);
			}
		}

		private float[] y;
		private float cnt = 0;

		void Awake(){
			y = new float[2]{0,0};
			Callibrate();
		}

		void Callibrate(){
			m = 1<<(BitDepth-1);
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
			Callibrate();
		}
#endif
		
		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
#endif
			for (var i = 0; i < data.Length; i = i + channels){
				cnt+=SampleRate;
				if(cnt>=1){
					cnt-=1;
					for(var c = 0; c < channels; c++){
						y[c]=((int)(data[i+c]*m))/(float)m;
					}
				}

				for(var c = 0; c < channels; c++){
					float wet = y[c];
					data[i+c] = data[i+c]*(1-DryWet) + wet*DryWet;
				}
			}
		}
	}
}