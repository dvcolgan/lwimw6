using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
	
    }
	
    // Update is called once per frame
    void Update ()
    {
        transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (1, 1, 1), 0.1f);
    }

    void BeatTrigger ()
    {
        transform.localScale = new Vector3 (2, 2, 2);
    }
}
