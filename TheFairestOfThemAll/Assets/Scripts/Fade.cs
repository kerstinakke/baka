using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	private float alpha;
	private Image image;
	private float change=0.15f;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		alpha = image.color.a;
	}
	
	// Update is called once per frame
	void Update(){
		Color c = image.color;
		if (c.a <= 0.2f || c.a >= alpha)
			change *= -1;
		image.color += new Color (0f, 0f, 0f, change*Time.deltaTime);
	}
}
