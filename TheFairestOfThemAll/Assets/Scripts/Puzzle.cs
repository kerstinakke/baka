using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

/** Class for checking result */
public class Puzzle : MonoBehaviour
{

	private float startTime;
	private Image<Bgr, byte> templateImage;
	private Image<Bgr, byte> mask;
	private MovableBeacon levelBeacon;
	private bool wasFocus;
	private Camera FPCamera;
	//private SendFeedback sender;
	[SerializeField] private bool solved = false;
	[SerializeField] private float accuracy = 0.9f;
	[SerializeField] Texture2D templateImageTexture;
	[SerializeField] Texture2D maskTexture;

	// Use this for initialization
	void Start ()
	{
		levelBeacon = GameObject.FindGameObjectWithTag ("Beacon").GetComponent<MovableBeacon> ();
		wasFocus = false;
		FPCamera = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Camera> ();
		templateImage = ImageHelper.ToImage (templateImageTexture);
		mask = ImageHelper.ToImage (maskTexture);
		startTime = Time.time;
		//sender = GetComponent<SendFeedback> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!solved) {
			if (!levelBeacon.GetActive ())
				return;
			bool focus = Input.GetKey (KeyCode.F);
			if (focus && !wasFocus) { //do once after f is pressed	
				levelBeacon.SetCameraActive (true);
				StartCoroutine (levelBeacon.TakePicture (this));
			} else if (!focus && wasFocus) {//do once after release
				levelBeacon.SetCameraActive (false);
				FPCamera.enabled = true;
			}
			wasFocus = focus;
			OverlayEffects.ShowTime (formatTime (Time.time - startTime));
		}

	}

	public void CheckCorrect (Texture2D sourceTexture)
	{

		if (Application.isEditor) {
			byte[] bytes = sourceTexture.EncodeToPNG ();
			File.WriteAllBytes (Application.dataPath + "/Rendering/textures/test.png", bytes);
		}

		Image<Bgr,  byte> sourceImage = ImageHelper.ToImage (sourceTexture);
		Image<Gray, float> result = new Image<Gray, float> (257 - templateImage.Width, 257 - templateImage.Height);
		Emgu.CV.CvInvoke.MatchTemplate (sourceImage, templateImage, result, TemplateMatchingType.CcorrNormed, mask);
		double[] minValues, maxValues;
		Point[] minLocations, maxLocations;
		result.MinMax (out minValues, out maxValues, out minLocations, out maxLocations);
		print (maxValues [0] + " " + maxLocations [0]);
		if (maxValues [0] > accuracy) {
			solved = true;
			gameObject.transform.Find ("whole").gameObject.SetActive (true);
			string levelName = "Level" + (SceneManager.GetActiveScene ().buildIndex - 1);
			float currentTime = Time.time - startTime;
			if (!PlayerPrefs.HasKey (levelName)) {
				PlayerPrefs.SetFloat (levelName, currentTime);
			} else if (PlayerPrefs.GetFloat (levelName) > currentTime) {
				PlayerPrefs.SetFloat (levelName, currentTime);
				OverlayEffects.ShowTime ("New best time! " + formatTime (currentTime));
			} else {
				OverlayEffects.ShowTime (formatTime (currentTime) + "\nBest time: " + formatTime (PlayerPrefs.GetFloat (levelName)));
			}
			PlayerPrefs.Save ();
            //sender.SendTime (levelName, currentTime);
            if(Application.isEditor)
                RotRememberer.WriteString(gameObject);
			StartCoroutine ("NextLevel");
		}
	}

		
	private IEnumerator NextLevel ()
	{
        float pass = Time.time + 2;
		while (!RotRememberer.done || Time.time < pass) {
			yield return new WaitForSeconds (0.1f);
		}
		int nextSceneIndex = SceneManager.GetActiveScene ().buildIndex + 1;
		SceneManager.LoadSceneAsync (nextSceneIndex);
	}

	private string formatTime (float currentTime)
	{
		return string.Format ("{0:00}:{1:00}", Mathf.Floor (currentTime / 60), currentTime % 60);
	}

	/** This function should be called if time was paused for some reason **/
	public void ReturnFromNap (float timeChange)
	{
		startTime += timeChange;
	}
}

