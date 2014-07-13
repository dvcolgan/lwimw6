using UnityEngine;
using System.Collections;

public class Scorekeeper : MonoBehaviour
{
    public static Scorekeeper instance;
    public int score = 0;
    public int lives = 3;

    void Awake ()
    {
        instance = this;
        DontDestroyOnLoad (this);
    }

    public void AddScore (int amount)
    {
        score += amount;
    }

    public void LoseLife ()
    {
        lives--;
    }

    public bool hasNoLives ()
    {
        return lives <= 0;
    }

    public void ResetScore ()
    {
        score = 0;
        lives = 1;
    }

    void OnGUI ()
    {
        GUI.Label (new Rect (10, 10, 200, 60), "Score " + score);
        GUI.Label (new Rect (10, 40, 200, 60), "Lives " + lives);
    }
}
