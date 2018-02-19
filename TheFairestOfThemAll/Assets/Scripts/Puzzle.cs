using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {

	public Vector3 correctPlayerPos;
	public Piece[] movingPieces;

	private bool m_IsFocus = false;
	private bool m_Checked = false;
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
				m_Checked = false;
			}
			m_IsFocus = focusing;
			if (m_IsFocus && !m_Checked) {
				Vector3 pos = m_Player.transform.position;
				print (pos+" "+correctPlayerPos);
				if (Mathf.Abs (pos.x - correctPlayerPos.x) <= 0.1 && Mathf.Abs (pos.z - correctPlayerPos.z) <= 0.1) {
					m_Solved = true;
					gameObject.transform.Find ("whole").gameObject.SetActive (true);
				}
				m_Checked = true;
			}
		}
	}
}
