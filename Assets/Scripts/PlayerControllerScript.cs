using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
	//http://unity3d.com/learn/tutorials/modules/beginner/2d/2d-controllers
	public float maxSpeed = 10f;
	bool facingRight = true;

	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	bool doubleJump = false;

	public float jumpForce = 700f;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);

		if (grounded) {
			doubleJump = false;
		}

		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (move));

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		if ((move > 0 && !facingRight) || (move < 0 && facingRight)) {
			Flip ();
		}
	}

	void Update() {
		if ((grounded || !doubleJump) && Input.GetButtonDown ("Jump")) {
			anim.SetBool ("Ground", false);

			rigidbody2D.AddForce (new Vector2(0, jumpForce));

			if (!grounded) {
				doubleJump = true;
			}
		}
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
