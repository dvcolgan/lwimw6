using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MixerGUI : MonoBehaviour {

	GameObject drums;
	GameObject bass;
	GameObject piano;
	GameObject subbass;
	GameObject master;

	Audial.Fader[] faders = new Audial.Fader[4];
	Audial.Crusher masterCrusher;

	List<MonoBehaviour>[] audialComponents = new List<MonoBehaviour>[5];

	// Use this for initialization
	void Awake () {
		MonoBehaviour[] components;

		for(var x = 0; x < audialComponents.Length; x++){
			audialComponents[x] = new List<MonoBehaviour>();
		}

		drums = GameObject.Find("Drums");
		components = drums.GetComponents<MonoBehaviour>();
		for(var d = 0; d < components.Length; d++){ 
			if(components[d].GetType().Namespace == "Audial"){
				if(components[d].GetType().Name == "Fader"){
					faders[0] = (Audial.Fader)components[d];
				}else{
					audialComponents[0].Add(components[d]);
					components[d].enabled = false;
				}
			}
		}

		bass = GameObject.Find("Bass");
		components = bass.GetComponents<MonoBehaviour>();
		for(var b = 0; b < components.Length; b++){
			if(components[b].GetType().Namespace == "Audial"){
				if(components[b].GetType().Name == "Fader"){
					faders[1] = (Audial.Fader)components[b];
				}else{
					audialComponents[1].Add(components[b]);
					components[b].enabled = false;
				}
			}
		}

		piano = GameObject.Find("Piano");
		components = piano.GetComponents<MonoBehaviour>();
		for(var p = 0; p < components.Length; p++){
			if(components[p].GetType().Namespace == "Audial"){
				if(components[p].GetType().Name == "Fader"){
					faders[2] = (Audial.Fader)components[p];
				}else{
					audialComponents[2].Add(components[p]);
					components[p].enabled = false;
				}
			}
		}

		subbass = GameObject.Find("Subbass");
		components = subbass.GetComponents<MonoBehaviour>();
		for(var s = 0; s < components.Length; s++){
			if(components[s].GetType().Namespace == "Audial"){
				if(components[s].GetType().Name == "Fader"){
					faders[3] = (Audial.Fader)components[s];
				}else{
					audialComponents[3].Add(components[s]);
					components[s].enabled = false;
				}
			}
		}

		master = GameObject.Find("AudioListener");
		components = master.GetComponents<MonoBehaviour>();
		for(var m = 0; m < components.Length; m++){
			if(components[m].GetType().Namespace == "Audial"){
				if(components[m].GetType().Name.ToString() == "Crusher"){
					masterCrusher = (Audial.Crusher)components[m];
				}else{
					audialComponents[4].Add(components[m]);
					components[m].enabled = false;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void DrawInstrument(string name, int index){
		bool effectEnabled = audialComponents[index][0].enabled;
		string effectStatus = effectEnabled ? "ON" : "OFF";

		bool channelMute = faders[index].Mute;
		string muteText = channelMute ? "ON" : "OFF";

		GUILayout.BeginVertical("Box");{
			GUILayout.Label(name);
			GUILayout.BeginHorizontal();{
				if(GUILayout.Button("Effects")){
					audialComponents[index].ForEach(delegate(MonoBehaviour component){
						component.enabled = !component.enabled;
					});
				}
				GUILayout.Label(effectStatus);
			}GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();{
				if(GUILayout.Button("Mute")){
					faders[index].Mute = !faders[index].Mute;
				}
				GUILayout.Label(muteText);
			}GUILayout.EndHorizontal();
		}GUILayout.EndVertical();
	}

	void OnGUI(){
		GUILayout.BeginArea(new Rect(0,300,Screen.width,Screen.height-300));{
			GUILayout.BeginHorizontal();{
				GUILayout.FlexibleSpace();

				GUILayout.BeginHorizontal("Box");{
					GUILayout.BeginVertical();{
						GUILayout.Label("");
						GUILayout.Label("Audial Effects");
						GUILayout.Label("Channel Status");
					}GUILayout.EndVertical();
					DrawInstrument("Drums", 0);
					DrawInstrument("Bass", 1);
					DrawInstrument("Piano", 2);
					DrawInstrument("Sub Bass", 3);

					GUILayout.BeginVertical("Box");{
						bool effectEnabled = audialComponents[4][0].enabled;
						string effectsText = effectEnabled ? "ON" : "OFF";
						GUILayout.Label("Master");
						GUILayout.BeginHorizontal();{
							if(GUILayout.Button("Effects")){
								audialComponents[4].ForEach(delegate(MonoBehaviour component){
									component.enabled = !component.enabled;
								});
							}
							GUILayout.Label(effectsText);
						}GUILayout.EndHorizontal();

						GUILayout.BeginHorizontal();{
							string crusherText = masterCrusher.enabled ? "ON" : "OFF";
							if(GUILayout.Button("BitCrusher")){
								masterCrusher.enabled = !masterCrusher.enabled;
							}
							GUILayout.Label(crusherText);
						}GUILayout.EndHorizontal();
					}GUILayout.EndVertical();

				}GUILayout.EndHorizontal();

				GUILayout.FlexibleSpace();
			}GUILayout.EndHorizontal();
		}GUILayout.EndArea();
	}
}
