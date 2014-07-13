using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class PanControl : MonoBehaviour {

		[SerializeField]
		[Range(-1,1)]
		private float _panAmount = 0;
		public float PanAmount{
			get{
				return _panAmount;
			}
			set{
				_panAmount = Mathf.Clamp(value, -1, 1);
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
			if(channels!=2) return;

			for (var i = 0; i < data.Length; i += channels){
				if(Mathf.Sign(PanAmount) > 0){
					data[i] = (1f - Mathf.Abs(PanAmount)) * data[i];
				}else{
					data[i+1] = (1f - Mathf.Abs(PanAmount)) * data[i+1];
				}
			}
		}
	}
}