using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

public class Puzzle : MonoBehaviour {

	[SerializeField] private MovablePiece[] movingPieces;
	[SerializeField] private Texture2D template;

	private MovableBeacon levelBeacon;
	private bool WasFocus;
	private Camera FPCamera;
	public bool Solved = false;


	// Use this for initialization
	void Start () {
		levelBeacon = GameObject.FindGameObjectWithTag ("Beacon").GetComponent<MovableBeacon>();
		WasFocus = false;
		FPCamera = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Solved) {
			bool focus = Input.GetKey (KeyCode.F);
			if (levelBeacon.GetActive() && focus && !WasFocus) { //do once after f is pressed	
				levelBeacon.SetCameraActive (true);
				StartCoroutine(levelBeacon.TakePicture (this));
			} else if (!focus && WasFocus) {//do once after release
				levelBeacon.SetCameraActive(false);
				FPCamera.enabled=true;
			}
			WasFocus = focus;
		}
	}

	public void IsCorrect(){
		FPCamera.enabled = false;
		Image<Bgr,  byte> sourceImage = new Image<Bgr, byte> ("Assets/Rendering/textures/puzzle0/test.png");
		Image<Bgr, byte> templateImage = new Image<Bgr, byte> ("Assets/Rendering/textures/puzzle0/solution.png");
		Image<Gray, float> result = sourceImage.MatchTemplate (templateImage, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
		double[] minValues, maxValues;
		Point[] minLocations, maxLocations;
		result.MinMax (out minValues, out maxValues, out minLocations, out maxLocations);
		print (maxValues[0]);
		if (maxValues [0] > 0.9) {
			Solved = true;
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
