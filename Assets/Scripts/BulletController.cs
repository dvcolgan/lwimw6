using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    private ObjectPool bulletPool;
	
    void Start ()
    {
        GameObject bulletPoolObject = GameObject.Find ("BulletPool");
        bulletPool = bulletPoolObject.GetComponent<ObjectPool> ();
    }

    void OnBecameInvisible ()
    {
        bulletPool.ReclaimPooledObject (gameObject);
    }
}