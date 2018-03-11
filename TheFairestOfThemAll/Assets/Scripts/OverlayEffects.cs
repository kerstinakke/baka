using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayEffects : MonoBehaviour {

	[SerializeField] private float maxAlpha;
	private Image image;
	private float change=0.15f;
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private Color correctColor;
	[SerializeField] private Color defaultColor;
	private int spriteIndex;
	private Canvas canvas;
	private bool fading;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
	}

	public void PlayEffects(){
		fading = false;
		StopAllCoroutines();
		image.sprite = sprites [0];
		image.color = defaultColor;
		canvas.enabled = true;
		print ("fading " + fading);
	}

	// Update is called once per frame
	void Update(){
		if (!fading) {
			if (image.color.a <= 0.2f || image.color.a >= maxAlpha)
				change *= -1;
			image.color += new Color (0f, 0f, 0f, change * Time.deltaTime);
		}
	}

	public void RotateEffect(bool rotateMode){
		if (rotateMode) {
			image.sprite = sprites [1];
		}else 
			image.sprite = sprites [0];
	}

	public void CorrectEffect(bool playCorrect){
		if (playCorrect) {
			image.sprite = sprites [2];
			image.color = correctColor;
			StartCoroutine (Fade ());
		} else
			canvas.enabled = false;
	}

	private IEnumerator Fade(){
		fading = true;
		while (fading)
		{
			image.color += new Color (0f, 0f, 0f,-0.2f*Time.deltaTime);
			fading = image.color.a>0 ? true : false;
			yield return null;
		}
		canvas.enabled = false;
	}

}
