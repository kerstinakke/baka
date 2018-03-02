using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece: Movable {

	public Vector3 correctRot;
	private float RotError = 0.1f;

	public bool CheckPlace(){
		Vector3 currentPos = transform.position;
		Vector3 currentRot = transform.eulerAngles;
		if (Mathf.Abs (correctPos.x - currentPos.x) <= PosError 
			&& Mathf.Abs (correctPos.y - currentPos.y) <= PosError 
			&& Mathf.Abs (correctPos.z - currentPos.z)<= PosError) {
			if (Mathf.Abs (correctRot.x - currentRot.x) <= RotError 
				&& Mathf.Abs (correctRot.y - currentRot.y) <= RotError
				&& Mathf.Abs (correctRot.z - currentRot.z) <= RotError) {
				transform.eulerAngles = correctRot;
				transform.localPosition = correctPos;
				return true;
			}
		}
		return false;
	}
}
