using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrintPositions : MonoBehaviour {

	// Update is called once per frame
	void Start () {
		foreach (Movable m in GetComponentsInChildren<Movable>()) {
			//m.correctPos = m.transform.position;
			Debug.Log(m.name +" ("+m.transform.position.x+", "+m.transform.position.y+", "+m.transform.position.z+")");
		}
	}
}
