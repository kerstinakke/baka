using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayEffects : MonoBehaviour {

	[SerializeField] private float maxAlpha;
	private Image image;
	private float change=0.15f;
	[SerializeField] private Sprite[] sprites;
	private int spriteIndex;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		image.sprite = sprites [0];
	}
	
	// Update is called once per frame
	void Update(){
		Color c = image.color;
		if (c.a <= 0.2f || c.a >= maxAlpha)
			change *= -1;
		image.color += new Color (0f, 0f, 0f, change*Time.deltaTime);
	}

	public void SwitchSprite(){
		if (++spriteIndex >= sprites.Length) {
			spriteIndex = 0;
		}
		image.sprite = sprites [spriteIndex];
	}
}
