using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {

	public Vector3 correctPlayerPos;
	public MovablePiece[] movingPieces;

	private bool m_IsFocus = false;
	private float m_PlayerError = 0.2f;
	public bool m_Solved = false;
	private GameObject m_Player;

	// Use this for initialization
	void Start () {
		m_Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_Solved) {
			bool focusing = Input.GetKey (KeyCode.F);
			if (!m_IsFocus && focusing) {
				Vector3 pos = m_Player.transform.position;
				print (pos+" "+correctPlayerPos);
				if (Mathf.Abs (pos.x - correctPlayerPos.x) <= 0.2 && Mathf.Abs (pos.z - correctPlayerPos.z) <= 0.2) {
					m_Player.transform.position = correctPlayerPos;
					if (AllSet()) {
						m_Solved = true;
						gameObject.transform.Find ("whole").gameObject.SetActive (true);
					}
				}
			}
			m_IsFocus = focusing;
		}
	}

	private bool AllSet(){
		for (int i = 0; i < movingPieces.Length; i++) {
			if (!movingPieces [i].CheckPlace())
				return false;
		}
		return true;
	}
}
