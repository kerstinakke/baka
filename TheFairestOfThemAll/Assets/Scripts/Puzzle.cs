using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

public class Puzzle : MonoBehaviour {

	//[SerializeField] private MovablePiece[] movingPieces;
	[SerializeField] private int level=1;

	private Image<Bgr, Byte> templateImage;
	private Image<Bgr,Byte> mask;
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
		templateImage = new Image<Bgr, Byte> ("Assets/Rendering/textures/puzzle"+level+"/solution.png");
		mask = new Image<Bgr, Byte> ("Assets/Rendering/textures/puzzle" + level + "/mask.png");
	}
	
	// Update is called once per frame
	void Update () {
		if (!Solved) {
			if (!levelBeacon.GetActive ())
				return;
			bool focus = Input.GetKey (KeyCode.F);
			if (focus && !WasFocus) { //do once after f is pressed	
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
		Emgu.CV.CvInvoke.MatchTemplate(sourceImage, templateImage, result, TemplateMatchingType.CcorrNormed, mask);
		double[] minValues, maxValues;
		Point[] minLocations, maxLocations;
		result.MinMax (out minValues, out maxValues, out minLocations, out maxLocations);
		print (maxValues[0]+" "+maxLocations[0]);
		if (maxValues [0] > accuracy) {
			Solved = true;
			gameObject.transform.Find ("whole").gameObject.SetActive (true);
			Invoke ("NextLevel", 3f);
		}
	}
		
	private void NextLevel(){
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
		if (SceneManager.sceneCount > nextSceneIndex)
		{
			SceneManager.LoadSceneAsync(nextSceneIndex);
		}
	}
}
