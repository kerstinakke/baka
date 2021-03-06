﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** Class dealing with arrows and text in tutorial
*/
public class Tutorial : MonoBehaviour
{
	
	private enum State
	{
		LeadToBeacon,
		CarryBeacon,
		LeadToPiece,
		CarryPiece}

	;


	private Transform arrows;
	//private Text overlayText;
	//	private TextMesh beaconText;
	private State state = State.LeadToBeacon;
	private GameObject tutorialText;


	// Use this for initialization
	void Start ()
	{
		arrows = transform.Find ("Arrows");
		for (int i = 1; i < arrows.childCount; i++) {
			arrows.GetChild (i).gameObject.SetActive (false);
		}
		tutorialText = GameObject.FindGameObjectWithTag ("Overlay").transform.Find ("HoldEffect").gameObject.transform.Find ("TutorialText").gameObject;
		/*	beaconText = GameObject.FindGameObjectWithTag ("Beacon").GetComponentInChildren<TextMesh> ();
		print (beaconText);
		beaconText.text = "X"; */
	}

	public void CarryBeacon ()
	{
		
		tutorialText.SetActive (false);
		if (state != State.LeadToBeacon && arrows.GetChild (0).gameObject.activeSelf == false)
			return;
		arrows.GetChild (0).gameObject.SetActive (false);
		arrows.GetChild (1).gameObject.SetActive (true);

		//overlayText.text = "";
		state = State.CarryBeacon;
		//beaconText.text = "";

	}

	public void LeadToPiece ()
	{
		if (state != State.CarryBeacon)
			return;
		arrows.GetChild (1).gameObject.SetActive (false);
		arrows.GetChild (2).gameObject.SetActive (true);
		state = State.LeadToPiece;
	}

	public void CarryPiece ()
	{
		tutorialText.SetActive (true);
		if (state == State.LeadToPiece)
			arrows.GetChild (2).gameObject.SetActive (false);
		//overlayText.text = "Q W E\nA S D\n\nF\n\nR";
		state = State.CarryPiece;
	}

}
