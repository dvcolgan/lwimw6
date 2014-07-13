using UnityEngine;
using System.Collections;

enum Direction {None, Left, Right, Up, Down};

public class PlayerController : MonoBehaviour {
	public int damage = 0;
	
	private const double DEADZONE = 0.25f;
	
	private Direction lastDirection = Direction.None;
	private bool firing = false;

	void Start () {
	
	}

	void Update () {
		bool right = Input.GetAxisRaw ("Horizontal") > DEADZONE;
		bool left = Input.GetAxisRaw ("Horizontal") < -DEADZONE;
		bool up = Input.GetAxisRaw ("Vertical") > DEADZONE;
		bool down = Input.GetAxisRaw ("Vertical") < -DEADZONE;
		bool fire = Input.GetButtonDown ("Fire1");

		if (lastDirection == Direction.None) {
			if (right) {
				lastDirection = Direction.Right;
				print ("Setting right");
			}
			if (left) {
				lastDirection = Direction.Left;
				print ("Setting left");
			}
			if (up) {
				lastDirection = Direction.Up;
				print ("Setting up");
			}
			if (down) {
				lastDirection = Direction.Down;
				print ("Setting down");
			}
		}
		if (fire) {
			firing = true;
		}
	}

	void HandleFault() {
		damage++;
	}

	void OnStrongBeat() {
		print (lastDirection);
		if (lastDirection != Direction.None) {
			Move(lastDirection);
			lastDirection = Direction.None;
		}
		if (firing) {
			Fire();
			firing = false;
		}
	}

	void Fire() {
		print ("FIRE");
	}

	void Move(Direction direction) {
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		if (direction == Direction.Left) {
			pos.x -= 1;
			rot = Quaternion.Euler (0, 0, 180);
		}
		else if (direction == Direction.Right) {
			pos.x += 1;
			rot = Quaternion.Euler (0, 0, 0);
		}
		else if (direction == Direction.Up) {
			pos.y += 1;
			rot = Quaternion.Euler (0, 0, 90);
		}
		else if (direction == Direction.Down) {
			pos.y -= 1;
			rot = Quaternion.Euler (0, 0, 270);
		}
		transform.position = pos;
		transform.rotation = rot;
	}
}
