using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour
{
    public enum AsteroidSize
    {
        Small,
        Medium,
        Large }
    ;
    public AsteroidSize size;
    public float speed;
    public float rotationSpeed;

    ObjectPool bulletPool;
    ObjectPool largeAsteroidPool;
    ObjectPool mediumAsteroidPool;
    ObjectPool smallAsteroidPool;

    // Use this for initialization
    void Start ()
    {
        GameObject bulletPoolObject = GameObject.Find ("BulletPool");
        bulletPool = bulletPoolObject.GetComponent<ObjectPool> ();

        GameObject largeAsteroidPoolObject = GameObject.Find ("LargeAsteroidPool");
        largeAsteroidPool = largeAsteroidPoolObject.GetComponent<ObjectPool> ();

        GameObject mediumAsteroidPoolObject = GameObject.Find ("MediumAsteroidPool");
        mediumAsteroidPool = mediumAsteroidPoolObject.GetComponent<ObjectPool> ();

        GameObject smallAsteroidPoolObject = GameObject.Find ("SmallAsteroidPool");
        smallAsteroidPool = smallAsteroidPoolObject.GetComponent<ObjectPool> ();

        if (Random.Range (0, 1) == 0) {
            rigidbody2D.AddTorque (rotationSpeed);
        } else {
            rigidbody2D.AddTorque (-rotationSpeed);
        }
    }

    void OnEnable ()
    {
        MoveInRandomDirection ();
    }

    void MoveInRandomDirection ()
    {
        rigidbody2D.velocity = Random.insideUnitCircle.normalized * speed;
    }

    void Update ()
    {
        transform.localScale = Vector3.Lerp (transform.localScale, new Vector3 (1, 1, 1), 0.5f);
    }
    
    void BeatTrigger ()
    {
        print ("BEAT");
        transform.localScale = new Vector3 (2, 2, 2);
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet") {
            SpawnFragments ();

            if (size == AsteroidSize.Large) {
                largeAsteroidPool.ReclaimPooledObject (gameObject);
                Scorekeeper.instance.AddScore (1);
            } else if (size == AsteroidSize.Medium) {
                mediumAsteroidPool.ReclaimPooledObject (gameObject);
                Scorekeeper.instance.AddScore (2);
            } else if (size == AsteroidSize.Small) {
                smallAsteroidPool.ReclaimPooledObject (gameObject);
                Scorekeeper.instance.AddScore (3);
            }

            bulletPool.ReclaimPooledObject (coll.gameObject);
        }
    }

    void SpawnFragments ()
    {
        ObjectPool poolToUse = null;
        if (size == AsteroidSize.Large) {
            poolToUse = mediumAsteroidPool;
        } else if (size == AsteroidSize.Medium) {
            poolToUse = smallAsteroidPool;
        }
		
        if (poolToUse != null) {
            GameObject frag1 = poolToUse.GetPooledObject ();
            GameObject frag2 = poolToUse.GetPooledObject ();

            frag1.transform.position = gameObject.transform.position;
            frag2.transform.position = gameObject.transform.position;
        }
    }
}