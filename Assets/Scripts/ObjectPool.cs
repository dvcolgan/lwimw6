using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject pooledObject;
    List<GameObject> pooledObjects;
    private int activeCount = 0;

    void Awake ()
    {
        pooledObjects = new List<GameObject> ();
    }

    public GameObject GetPooledObject ()
    {
        activeCount++;
        for (int i=0; i<pooledObjects.Count; i++) {
            if (!pooledObjects [i].activeInHierarchy) {
                pooledObjects [i].SetActive (true);
                return pooledObjects [i];
            }
        }
        GameObject obj = (GameObject)Instantiate (pooledObject);
        obj.transform.parent = transform;
        pooledObjects.Add (obj);
        return obj;
    }

    public void ReclaimPooledObject (GameObject obj)
    {
        obj.SetActive (false);
        activeCount--;
    }

    public int GetActiveCount ()
    {
        return activeCount;
    }
}
