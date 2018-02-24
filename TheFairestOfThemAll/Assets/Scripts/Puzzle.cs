﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

public class Puzzle : MonoBehaviour {

	[SerializeField] private Vector3 correctPlayerPos;
	[SerializeField] private MovablePiece[] movingPieces;
	[SerializeField] private float m_PlayerError = 0.2f;
	[SerializeField] private RenderTexture m_answerTexture; 
	[SerializeField] private Texture2D m_template;

	private Texture2D m_answerImage; 
	private bool m_IsFocus = false;
	public bool m_Solved = false;
	private GameObject m_Player;
	private Camera m_Camera; 

	// Use this for initialization
	void Start () {
		m_Player = GameObject.FindGameObjectWithTag ("Player");
		m_Camera = m_Player.GetComponentInChildren<Camera> ();
		m_answerImage = new Texture2D(256,256, TextureFormat.RGB24, false); 
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_Solved) {
			bool focusing = Input.GetKey (KeyCode.F);
			if (!m_IsFocus && focusing) {
				Vector3 pos = m_Player.transform.position;
				print (pos+" "+correctPlayerPos);
				if (Mathf.Abs (pos.x - correctPlayerPos.x) <= m_PlayerError && Mathf.Abs (pos.z - correctPlayerPos.z) <=m_PlayerError) {
					m_Player.transform.position = correctPlayerPos;
					if (AllSet()) {
						m_Camera.targetTexture = m_answerTexture;
						m_Camera.transform.LookAt (GameObject.Find ("mirror1").transform.position);
						m_Camera.Render ();
						RenderTexture.active = m_answerTexture;
						m_answerImage.ReadPixels (new Rect (0, 0, 256, 256), 0, 0);
						m_Camera.targetTexture = null;
						RenderTexture.active = null;
						byte[] bytes = m_answerImage.EncodeToPNG ();
						System.IO.File.WriteAllBytes ("Assets/Rendering/textures/puzzle0/test.png",bytes);

						Image<Bgr,  byte> sourceImage = new Image<Bgr, byte> ("Assets/Rendering/textures/puzzle0/test.png");
						Image<Bgr, byte> templateImage = new Image<Bgr, byte>("Assets/Rendering/textures/puzzle0/solution.png");
						Image<Gray, float> result = sourceImage.MatchTemplate(templateImage,Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
						double[] minValues, maxValues;
						Point[] minLocations, maxLocations;
						result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
						if (maxValues [0] > 0.9) {
							m_Solved = true;
							gameObject.transform.Find ("whole").gameObject.SetActive (true);
						}
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
