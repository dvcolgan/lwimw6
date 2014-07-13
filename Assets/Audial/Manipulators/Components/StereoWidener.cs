using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class StereoWidener : MonoBehaviour {

		[SerializeField]
		[Range(0,2)]
		private float _width = 1.3f;
		public float Width{
			get{
				return _width;
			}
			set{
				_width = Mathf.Clamp(value, 0, 2);
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
			if(channels<2) return;
			float widthMod = Width * 0.5f;
			for(var i = 0; i < data.Length; i += channels){
				float mono = (data[i] + data[i+1]) * 0.5f;
				float stereo = (data[i] - data[i+1]) * widthMod;

				data[i] = mono + stereo;
				data[i+1] = mono - stereo;
			}
		}
	}
}
