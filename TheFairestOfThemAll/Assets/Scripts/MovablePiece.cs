using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece: Movable {

	public Vector3 correctPos;
	public Vector3 correctRot;
	public float m_PosError = 2f;
	public float m_RotError = 5f;

	public bool CheckPlace(){
		Vector3 currentPos = transform.position;
		Vector3 currentRot = transform.eulerAngles;
		print(correctPos - currentPos);
		if (Mathf.Abs (correctPos.x - currentPos.x) <= m_PosError 
			&& Mathf.Abs (correctPos.y - currentPos.y) <= m_PosError 
			&& Mathf.Abs (correctPos.z - currentPos.z)<= m_PosError) {
			if (Mathf.Abs (correctRot.x - currentRot.x) <= m_RotError 
				&& Mathf.Abs (correctRot.y - currentRot.y) <= m_RotError
				&& Mathf.Abs (correctRot.z - currentRot.z) <= m_RotError) {
				transform.eulerAngles = correctRot;
				transform.localPosition = correctPos;
				return true;
			}
		}
		return false;
	}
}
