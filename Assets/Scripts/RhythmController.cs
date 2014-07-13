using UnityEngine;
using System.Collections;

enum SequencerState {Stopped, Playing, Paused}

public class RhythmController : MonoBehaviour {

	private float startTime;
	private float currentTime;

	private float targetTime;
	
	public int targetSample = 0;

	private int samplePerBeat;
	private float timePerBeat;

	public float bpm = 120;

	private int measure = 0;
	private int quarter = 0;
	private int sixteenth = 0;

	public GameObject[] registeredGameObjects;

	SequencerState sequencerState = SequencerState.Stopped;
	AudioController[] audioControllers;
	void Awake () {

		startTime = Time.time;
		currentTime = startTime;

		timePerBeat = 60f/bpm;
		samplePerBeat = (int)(timePerBeat * AudioSettings.outputSampleRate);

		BeginSequencer();

		audioControllers = gameObject.GetComponents<AudioController>();
	}

	void BeginSequencer(){
		startTime = currentTime;
		targetTime = currentTime + timePerBeat / 4f;
		StartCoroutine(RunSequencer());
	}

	IEnumerator RunSequencer(){
		sequencerState = SequencerState.Playing;
		while(sequencerState != SequencerState.Stopped){
			currentTime = Time.time;
			if(currentTime > targetTime - 0.1f){
				Sixteenth();
			}
			yield return new WaitForSeconds(0.00001f);
		}
	}

	void Sixteenth(){
		timePerBeat = 60f/bpm;
		samplePerBeat = (int)(timePerBeat * AudioSettings.outputSampleRate);
		
		targetTime += timePerBeat / 4;
		targetSample += samplePerBeat / 4;

		for(var i = 0; i < audioControllers.Length; i++){
			audioControllers[i].AddTrigger(targetSample, sixteenth);
			audioControllers[i].BeatTriggers(sixteenth%16);
		}

		if(registeredGameObjects != null && registeredGameObjects.Length>0){
			for(var x = 0; x < registeredGameObjects.Length; x++){
				registeredGameObjects[x].BroadcastMessage("Sixteenth", sixteenth,SendMessageOptions.DontRequireReceiver);
			}


		}

		if(sixteenth >= 16){
			measure++;
		}
		
		sixteenth %= 16;
		sixteenth++;


	}
}
