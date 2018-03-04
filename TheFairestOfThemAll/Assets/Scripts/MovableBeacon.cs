﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBeacon : Movable{
	private bool isActivated;
	private Vector3 mirrorPos;

	[SerializeField] private RenderTexture answerTexture; 
	private Camera Camera;
	private Texture2D answerImage;
	private ParticleSystem effect;
	public float FOV;

	public void Start(){
		base.Start ();
		Camera = transform.Find ("Camera").GetComponent<Camera>();
		Camera.enabled = false;
		FOV = Camera.fieldOfView;
		isActivated = true;
		answerImage = new Texture2D(256,256, TextureFormat.RGB24, false); 
		effect = GetComponentInChildren<ParticleSystem> ();
		effect.Stop ();
		mirrorPos = GameObject.FindGameObjectWithTag ("Mirror").transform.position;

	}
		
	public void SetCameraActive(bool value){
		Camera.enabled = value;
	}

	public bool LetGo(Vector3 pos){
		bool correct = false;
		if (WithinLimits(posError)) {
			transform.position = correctPos;
			transform.LookAt (mirrorPos);
			effect.Play ();
			correct = true;
		} else {
			transform.position = pos;
		}
		isActivated = true;
		return correct;
	}

	public IEnumerator TakePicture(Puzzle puzzle){
		yield return new WaitForEndOfFrame();
		Camera.targetTexture = answerTexture;
		Camera.Render ();
		RenderTexture.active = answerTexture;
		answerImage.ReadPixels (new Rect (0, 0, 256, 256), 0, 0);
		answerImage.Apply ();
		Camera.targetTexture = null;
		RenderTexture.active = null;
		byte[] bytes = answerImage.EncodeToPNG ();
		System.IO.File.WriteAllBytes ("Assets/Rendering/textures/test.png",bytes);
		puzzle.IsCorrect ();
	}

	public void Inactivate(){
		isActivated = false;
		effect.Stop ();
	}
		
	public bool GetActive(){
		return isActivated;
	}
}
