using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

	AudioController[] audioControllers;

	// Use this for initialization
	void Start () {
		audioControllers = GetComponents<AudioController>();
	}

	void OnGUI(){
		GUILayout.BeginArea(new Rect(150,400,160,120));{
			DrawSequencer(audioControllers[0]);
		}GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(400,400,160,120));{
			DrawSequencer(audioControllers[1]);
		}GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(650,400,160,120));{
			DrawSequencer(audioControllers[2]);
		}GUILayout.EndArea();
	}

	void DrawSequencer(AudioController audioController){
		GUILayout.BeginVertical("Box");{
			GUILayout.BeginHorizontal();{
				for(var i = 0; i < audioController.pattern.sequence.Length/2; i++){
					audioController.pattern.sequence[i] = GUILayout.Toggle(audioController.pattern.sequence[i]==1,"") ? 1 : 0;
				}
			}GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();{
				for(var i = audioController.pattern.sequence.Length/2; i < audioController.pattern.sequence.Length; i++){
					audioController.pattern.sequence[i] = GUILayout.Toggle(audioController.pattern.sequence[i]==1,"") ? 1 : 0;
				}
			}GUILayout.EndHorizontal();
		}GUILayout.EndVertical();
	}
}
