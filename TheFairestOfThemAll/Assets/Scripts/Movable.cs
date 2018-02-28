using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

	private Vector3 m_offset;
	private Collider m_collider;

	public void Start(){
		m_collider = GetComponent<Collider> ();
	}

	public void Pickup (Vector3 pos){
		m_offset = transform.position-pos;
		m_collider.isTrigger = true;
	}

	public void LetGo (){
		m_collider.isTrigger = false;
	}

	public void Follow(Vector3 pos){
		transform.position = pos+m_offset;
	}

}
