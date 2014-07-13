using UnityEngine;
using System.Collections.Generic;

namespace Audial{

	[ExecuteInEditMode]
	public class AudioTester : MonoBehaviour {

		[HideInInspector]
		public bool hasAudioSource = true;

		[HideInInspector]
		public AudioSource audioSource;

#if UNITY_EDITOR
		private void CheckAudioSource(){
			audioSource = GetComponent<AudioSource>();
			if(audioSource!=null){
				hasAudioSource = true;
			}else{
				hasAudioSource = false;
			}
		}

		public bool RunEffectsInEditMode = false;
		private bool _RunEffectsInEditMode;

		void Update(){
			if(_RunEffectsInEditMode != RunEffectsInEditMode){
				_RunEffectsInEditMode = RunEffectsInEditMode;
				gameObject.SendMessage("SetRunEffectInEditMode",RunEffectsInEditMode);
			}
			CheckAudioSource();
		}
#endif

		public void ClearBuffer(){}
		public void SetRunEffectInEditMode(){}

		public bool playAudio {
			set {
				gameObject.SendMessage("ClearBuffer");
				if(hasAudioSource&&audioSource.clip!=null){
					audioSource.Play();
				}
			}
		}
		
		public bool stopAudio {
			set {
				gameObject.SendMessage("ClearBuffer");
				if(hasAudioSource){
					audioSource.Stop();
				}
			}
		}
		
	}
}
