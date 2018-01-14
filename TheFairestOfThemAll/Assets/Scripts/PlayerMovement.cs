using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 3f;

	Vector3 playerDirection = Vector3.forward;
	public Rigidbody rigidBody;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move() {
		playerDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
		rigidBody.MovePosition (transform.position + playerDirection * speed * Time.deltaTime);
	}
}
