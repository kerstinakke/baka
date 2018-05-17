using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Keep oject (Beacon) always facing mirror */
[ExecuteInEditMode]
public class LookAtMirror : MonoBehaviour
{
	private Vector3 mirrorPos;
	// Use this for initialization
	void Start ()
	{
		mirrorPos = GameObject.FindGameObjectWithTag ("Mirror").transform.position;
		transform.LookAt (mirrorPos);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.LookAt (mirrorPos);
	}
}
