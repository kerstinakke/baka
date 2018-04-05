using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour {

	private FirstPersonController player;
	private Puzzle puzzle;
	private bool paused;
	private GameObject overlay;
	private bool wasDown;
	private float pauseStartTime;
	private GameObject pauseScreen;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController>();
		puzzle = GameObject.Find ("Puzzle").GetComponent<Puzzle> ();
		overlay = GameObject.FindGameObjectWithTag ("Overlay");
		pauseScreen = transform.Find ("PauseScreen").gameObject;
		pauseScreen.SetActive(false);
		paused = false;
		wasDown = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Pause")) {
			if (!paused && !wasDown) {
				pauseStartTime = Time.time;
				player.enabled = false;
				puzzle.enabled = false;
				overlay.SetActive (false);
				pauseScreen.SetActive(true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				paused = true;
			} else if (paused && !wasDown) {
				player.enabled = true;
				puzzle.enabled = true;
				puzzle.ReturnFromNap (Time.time - pauseStartTime);
				overlay.SetActive (true);
				pauseScreen.SetActive(false);
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				paused = false;
			}
			wasDown = true;
		} else
			wasDown = false;
	}
}
