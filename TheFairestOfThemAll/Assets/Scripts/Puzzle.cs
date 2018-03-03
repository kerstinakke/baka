using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

public class Puzzle : MonoBehaviour {

	//[SerializeField] private MovablePiece[] movingPieces;
	[SerializeField] private int level;

	private Image<Bgr, byte> templateImage;
	private Image<Gray, byte> mask;
	private MovableBeacon levelBeacon;
	private bool WasFocus;
	private Camera FPCamera;
	[SerializeField] private bool Solved = false;
	[SerializeField] private float accuracy = 0.9f;


	// Use this for initialization
	void Start () {
		levelBeacon = GameObject.FindGameObjectWithTag ("Beacon").GetComponent<MovableBeacon>();
		WasFocus = false;
		FPCamera = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Camera> ();
		templateImage = new Image<Bgr, byte> ("Assets/Rendering/textures/puzzle"+level+"/solution.png");

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
		Image<Bgr,  byte> sourceImage = new Image<Bgr, byte> ("Assets/Rendering/textures/test.png");
		Image<Gray, float> result = new Image<Gray, float>(257-templateImage.Width, 257-templateImage.Height);
		Emgu.CV.CvInvoke.MatchTemplate(sourceImage, templateImage, result, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed,mask);
		double[] minValues, maxValues;
		Point[] minLocations, maxLocations;
		result.MinMax (out minValues, out maxValues, out minLocations, out maxLocations);
		print (maxValues[0]+" "+maxLocations[0]);
		if (maxValues [0] > accuracy) {
			Solved = true;
			gameObject.transform.Find ("whole").gameObject.SetActive (true);
		}
	}
		
}
