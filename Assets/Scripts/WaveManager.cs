using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    private int currentWave = 0;
    
    ObjectPool largeAsteroidPool;
    ObjectPool mediumAsteroidPool;
    ObjectPool smallAsteroidPool;

    void Awake ()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        GameObject largeAsteroidPoolObject = GameObject.Find ("LargeAsteroidPool");
        largeAsteroidPool = largeAsteroidPoolObject.GetComponent<ObjectPool> ();
        
        GameObject mediumAsteroidPoolObject = GameObject.Find ("MediumAsteroidPool");
        mediumAsteroidPool = mediumAsteroidPoolObject.GetComponent<ObjectPool> ();
        
        GameObject smallAsteroidPoolObject = GameObject.Find ("SmallAsteroidPool");
        smallAsteroidPool = smallAsteroidPoolObject.GetComponent<ObjectPool> ();

        Scorekeeper.instance.ResetScore ();
    }
	
    void FixedUpdate ()
    {
        CheckForNextWave ();
    }

    void CheckForNextWave ()
    {
        int smallTotal = smallAsteroidPool.GetActiveCount ();
        int mediumTotal = mediumAsteroidPool.GetActiveCount ();
        int largeTotal = largeAsteroidPool.GetActiveCount ();
        
        if (smallTotal == 0 && mediumTotal == 0 && largeTotal == 0) {
            OnWaveOver ();
        }
    }

    public void OnWaveOver ()
    {
        print ("WAVE OVER MAN");
        currentWave++;
        for (int i=0; i<currentWave; i++) {
            GameObject asteroid = largeAsteroidPool.GetPooledObject ();
            Vector3 pos = new Vector3 (0.1f, 0.1f, 0);
            asteroid.transform.position = Camera.main.ViewportToWorldPoint (pos);
           
            asteroid.transform.position = new Vector3 (0, 0, 0);
            print ("SPAWN");
        }
    }

    void OnGUI ()
    {
        GUI.Label (new Rect (300, 10, 200, 60), "Wave " + currentWave);
    }
}
