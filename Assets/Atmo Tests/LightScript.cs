using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Sixteenth(int sixteenth){
		if(sixteenth%16 == 0){
			light.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
		}
	}
}
