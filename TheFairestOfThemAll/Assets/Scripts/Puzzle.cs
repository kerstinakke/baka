using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

public class Puzzle : MonoBehaviour {

	//[SerializeField] private MovablePiece[] movingPieces;
	[SerializeField] private int level=1;

	private Image<Bgr, byte> templateImage;
	private Image<Bgr, byte> mask;
	private MovableBeacon levelBeacon;
	private bool wasFocus;
	private Camera FPCamera;
	[SerializeField] private bool solved = false;
	[SerializeField] private float accuracy = 0.9f;
	[SerializeField] Texture2D templateImageTexture;
	[SerializeField] Texture2D maskTexture;

	// Use this for initialization
	void Start () {
		levelBeacon = GameObject.FindGameObjectWithTag ("Beacon").GetComponent<MovableBeacon>();
		wasFocus = false;
		FPCamera = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Camera> ();
		templateImage = ImageHelper.ToImage(templateImageTexture);
		print (templateImage.Size);
		mask = ImageHelper.ToImage(maskTexture);
		print (mask.Size);
	}
	
	// Update is called once per frame
	void Update () {
		if (!solved) {
			if (!levelBeacon.GetActive ())
				return;
			bool focus = Input.GetKey (KeyCode.F);
			if (focus && !wasFocus) { //do once after f is pressed	
				levelBeacon.SetCameraActive (true);
				StartCoroutine(levelBeacon.TakePicture (this));
			} else if (!focus && wasFocus) {//do once after release
				levelBeacon.SetCameraActive(false);
				FPCamera.enabled=true;
			}
			wasFocus = focus;
		}
	}

	public void CheckCorrect(Texture2D sourceTexture){
		FPCamera.enabled = false;
		Image<Bgr,  byte> sourceImage = ImageHelper.ToImage(sourceTexture);
		Image<Gray, float> result = new Image<Gray, float>(257-templateImage.Width, 257-templateImage.Height);
		Emgu.CV.CvInvoke.MatchTemplate(sourceImage, templateImage, result, TemplateMatchingType.CcorrNormed, mask);
		double[] minValues, maxValues;
		Point[] minLocations, maxLocations;
		result.MinMax (out minValues, out maxValues, out minLocations, out maxLocations);
		print (maxValues[0]+" "+maxLocations[0]);
		if (maxValues [0] > accuracy) {
			solved = true;
			gameObject.transform.Find ("whole").gameObject.SetActive (true);
			Invoke ("NextLevel", 3f);
		}
	}
		
	private void NextLevel(){
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
			SceneManager.LoadSceneAsync(nextSceneIndex);
	}
}
