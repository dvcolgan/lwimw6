using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        print ("In game over");
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    void OnGUI ()
    {
        GUI.Label (new Rect (300, 300, 200, 60), "GAME OVER");
        GUI.Label (new Rect (300, 280, 200, 60), "Final Score: " + Scorekeeper.instance.score);

        if (GUI.Button (new Rect (300, 200, 200, 60), "Retry?")) {
            Application.LoadLevel ("Asteroids");
        }
    }
}
