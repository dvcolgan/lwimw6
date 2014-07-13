using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{

    public float thrustForce = 20f;
    public float rotationForce = 20f;
    public float maxVelocity = 0.5f;
    public float fireForce = 300f;
    private float sqrMaxVelocity;

    private ObjectPool bulletPool;

    void Start ()
    {
        sqrMaxVelocity = maxVelocity * maxVelocity;

        GameObject bulletPoolObject = GameObject.Find ("BulletPool");
        bulletPool = bulletPoolObject.GetComponent<ObjectPool> ();
    }

    void Fire ()
    {
        for (int i=0; i<5; i++) {
            GameObject bullet = bulletPool.GetPooledObject ();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.rigidbody2D.AddForce (transform.right * fireForce);
        }
    }

    void FixedUpdate ()
    {
        float rotationAmount = Input.GetAxis ("Horizontal");
        float thrustAmount = Input.GetAxis ("Vertical");

        rigidbody2D.AddForce (transform.right * thrustAmount * thrustForce);
        rigidbody2D.angularVelocity = thrustAmount;
        rigidbody2D.AddTorque (-rotationAmount * rotationForce);

        if (rigidbody2D.velocity.sqrMagnitude > sqrMaxVelocity) {
            print ("Speed at " + rigidbody2D.velocity);
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxVelocity;
            print ("Normalized " + rigidbody2D.velocity);
        }

        if (Input.GetButton ("Fire1")) {
            Fire ();
        }
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
        if (coll.gameObject.tag == "Asteroid") {
            Scorekeeper.instance.LoseLife ();
            gameObject.SetActive (false);
            print ("LOST LIFE");
            if (Scorekeeper.instance.hasNoLives ()) {
                print ("NO LIVES LEFT");
                Application.LoadLevel ("GameOver");
            }
            Invoke ("Respawn", 2);
        }
    }

    void Respawn ()
    {
        gameObject.SetActive (true);
        transform.position = new Vector3 (0, 0, 0);
    }
}