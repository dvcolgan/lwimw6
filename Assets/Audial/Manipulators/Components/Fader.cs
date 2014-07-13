using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Fader : MonoBehaviour {

		[SerializeField]
		[Range(0,3)]
		private float _gain = 1;
		public float Gain{
			get{
				return _gain;
			}
			set{
				_gain = Mathf.Clamp(value, 0, 3);
			}
		}

		public bool Mute = false;

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
			if(Mute){
				for(var i = 0; i < data.Length; i++){
					data[i] = 0;
				}
			}else{
				for(var i = 0; i < data.Length; i++){
					data[i] *= Gain;
				}
			}

		}
	}
}