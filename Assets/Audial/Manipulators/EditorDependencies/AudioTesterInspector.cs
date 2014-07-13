using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Audial{
	[CustomEditor(typeof(AudioTester))]
	public class AudioTesterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			AudioTester audioTester = (AudioTester) target;

			DrawDefaultInspector();

			GUILayout.BeginHorizontal();
			if(GUILayout.Button("PLAY"))
				audioTester.playAudio = true;
			if (GUILayout.Button("STOP"))
				audioTester.stopAudio = true;
			GUILayout.EndHorizontal();

			string msgText;
			if(!audioTester.hasAudioSource||audioTester.audioSource.clip==null)
				msgText = "-AUDIO CLIP REQUIRED FOR TESTING-";
			else
				msgText = "-READY FOR TESTING-";
			
			EditorGUILayout.HelpBox(msgText, MessageType.Info);

		}
	}
}
