﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Vaatleja. Marks the right angle from which the solution must be seen. */
public class MovableBeacon : Movable
{
	private bool isActivated;
	private Vector3 mirrorPos;

	[SerializeField] private RenderTexture answerTexture;
	private Camera beaconCamera;
	private Texture2D answerImage;
	private ParticleSystem effect;
	public float FOV;
	private Rect smallScreen = new Rect (0.7f, 0.7f, 0.3f, 0.3f);
	private Rect fullScreen = new Rect (0f, 0f, 1f, 1f);

	public void Start ()
	{
		base.Start ();
		beaconCamera = transform.Find ("Camera").GetComponent<Camera> ();
		beaconCamera.enabled = false;
		FOV = beaconCamera.fieldOfView;
		isActivated = true;
		answerImage = new Texture2D (256, 256, TextureFormat.RGB24, false); 
		effect = GetComponentInChildren<ParticleSystem> ();
		effect.Stop ();
		mirrorPos = GameObject.FindGameObjectWithTag ("Mirror").transform.position;
		rotAngle = 0f;

	}

	public void SetCameraActive (bool value)
	{
		beaconCamera.enabled = value;
	}

	public override bool Pickup (Vector3 pos)
	{
		return WithinLimits (slow);
	}

	public virtual bool LetGo (Vector3 pos)
	{
		bool correct = false;
		if (WithinLimits (posError)) {
			transform.position = correctPos;
            audioSource.Play();
			transform.LookAt (mirrorPos);
			effect.Play ();
			correct = true;
		} else {
			transform.position = pos;
		}
		isActivated = true;
		beaconCamera.rect = fullScreen;
		SetCameraActive (false);
		return correct;
	}

	public IEnumerator TakePicture (Puzzle puzzle)
	{
		yield return new WaitForEndOfFrame ();
		beaconCamera.targetTexture = answerTexture;
		beaconCamera.Render ();
		RenderTexture.active = answerTexture;
		answerImage.ReadPixels (new Rect (0, 0, 256, 256), 0, 0);
		answerImage.Apply ();
		puzzle.CheckCorrect (answerImage);
		beaconCamera.targetTexture = null;
		RenderTexture.active = null;
	}

	public void Inactivate ()
	{
		isActivated = false;
		beaconCamera.rect = smallScreen;
		SetCameraActive (true);
		effect.Stop ();
	}

	public bool GetActive ()
	{
		return isActivated;
	}

	public override bool Follow (Vector3 pos)
	{
		transform.position = pos;
		transform.LookAt (mirrorPos);
		return WithinLimits (slow);
	}
}
