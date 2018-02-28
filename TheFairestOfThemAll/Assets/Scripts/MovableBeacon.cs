﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MovableBeacon : Movable{
	[SerializeField] private RenderTexture m_answerTexture; 
	private Camera m_Camera;
	public bool isActivated;
	private Texture2D m_answerImage;

	public void Start(){
		base.Start ();
		m_Camera = transform.Find ("Camera").GetComponent<Camera>();
		m_Camera.enabled = false;
		isActivated = false;
		m_answerImage = new Texture2D(256,256, TextureFormat.RGB24, false); 
	}

	public void SetCameraActive(bool value){
		m_Camera.enabled = value;
	}

	public void LetGo(Vector3 pos){
		transform.position = pos;
		transform.LookAt (GameObject.Find ("mirror1").transform.position);
		isActivated = true;
	}

	public IEnumerator TakePicture(Puzzle puzzle){
		yield return new WaitForEndOfFrame();
		m_Camera.targetTexture = m_answerTexture;
		m_Camera.Render ();
		RenderTexture.active = m_answerTexture;
		print ("boo");
		m_answerImage.ReadPixels (new Rect (0, 0, 256, 256), 0, 0);
		m_answerImage.Apply ();
		m_Camera.targetTexture = null;
		RenderTexture.active = null;
		byte[] bytes = m_answerImage.EncodeToPNG ();
		System.IO.File.WriteAllBytes ("Assets/Rendering/textures/puzzle0/test.png",bytes);
		puzzle.IsCorrect ();
	}

	public IEnumerator SavePicture(Puzzle puzzle){
		yield return new WaitForEndOfFrame();

	}
		
}
