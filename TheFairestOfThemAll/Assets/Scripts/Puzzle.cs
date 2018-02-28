﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

public class Puzzle : MonoBehaviour {

	[SerializeField] private Vector3 correctBeaconPos;
	[SerializeField] private MovablePiece[] movingPieces;
	[SerializeField] private float m_PlayerError = 0.2f;
	[SerializeField] private Texture2D m_template;

	private MovableBeacon m_levelBeacon;
	private bool m_WasFocus;
	private Camera m_FPCamera;
	public bool m_Solved = false;


	// Use this for initialization
	void Start () {
		m_levelBeacon = GameObject.FindGameObjectWithTag ("Beacon").GetComponent<MovableBeacon>();
		m_WasFocus = false;
		m_FPCamera = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_Solved) {
			bool focus = Input.GetKey (KeyCode.F);
			if (m_levelBeacon.isActivated && focus && !m_WasFocus) { //do once after f is pressed	
				m_levelBeacon.SetCameraActive (true);
				StartCoroutine(m_levelBeacon.TakePicture (this));
			} else if (!focus && m_WasFocus) {//do once after release
				m_levelBeacon.SetCameraActive(false);
				m_FPCamera.enabled=true;
			}
			m_WasFocus = focus;
		}
	}

	public void IsCorrect(){
		m_FPCamera.enabled = false;
		Image<Bgr,  byte> sourceImage = new Image<Bgr, byte> ("Assets/Rendering/textures/puzzle0/test.png");
		Image<Bgr, byte> templateImage = new Image<Bgr, byte> ("Assets/Rendering/textures/puzzle0/solution.png");
		Image<Gray, float> result = sourceImage.MatchTemplate (templateImage, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
		double[] minValues, maxValues;
		Point[] minLocations, maxLocations;
		result.MinMax (out minValues, out maxValues, out minLocations, out maxLocations);
		print (maxValues[0]);
		if (maxValues [0] > 0.9) {
			m_Solved = true;
			gameObject.transform.Find ("whole").gameObject.SetActive (true);
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
