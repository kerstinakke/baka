﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MovableBeacon : Movable{
	private bool isActivated;

	[SerializeField] private RenderTexture answerTexture; 
	private Camera Camera;
	private Texture2D answerImage;
	private ParticleSystem effect;

	public void Start(){
		base.Start ();
		Camera = transform.Find ("Camera").GetComponent<Camera>();
		Camera.enabled = false;
		isActivated = true;
		answerImage = new Texture2D(256,256, TextureFormat.RGB24, false); 
		effect = GetComponentInChildren<ParticleSystem> ();
		effect.Stop ();
	}
		
	public void SetCameraActive(bool value){
		Camera.enabled = value;
	}

	public void LetGo(Vector3 pos){
		if (Mathf.Abs (correctPos.x - pos.x) <= posError && Mathf.Abs (correctPos.x - pos.x) <= posError) {
			transform.position = correctPos;
			transform.LookAt (GameObject.Find ("mirror1").transform.position);
			effect.Play ();
		} else 
			transform.position = pos;
		isActivated = true;

	}

	public IEnumerator TakePicture(Puzzle puzzle){
		yield return new WaitForEndOfFrame();
		Camera.targetTexture = answerTexture;
		Camera.Render ();
		RenderTexture.active = answerTexture;
		print ("boo");
		answerImage.ReadPixels (new Rect (0, 0, 256, 256), 0, 0);
		answerImage.Apply ();
		Camera.targetTexture = null;
		RenderTexture.active = null;
		byte[] bytes = answerImage.EncodeToPNG ();
		System.IO.File.WriteAllBytes ("Assets/Rendering/textures/puzzle0/test.png",bytes);
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
