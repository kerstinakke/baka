using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

	private Transform m_Parent;

	// Use this for initialization
	void Start () {
		m_Parent = transform.parent;
	}
	
	public void LetGo(){
		transform.parent = m_Parent;
	}
}
