using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffects : MonoBehaviour {

	private Text text;
	private Color defaultColor;
	private Vector3 defaultSize;
	private float change=0.4f;

	// Use this for initialization
	void Awake () {
		text = GetComponent<Text> ();
		defaultColor = text.color;
		defaultSize = text.transform.localScale;
	}

	void OnEnable () {
		text.color = defaultColor;
		print (defaultColor);
		text.transform.localScale = defaultSize;
		StartCoroutine ("FadeGrow");
	}
	
	private IEnumerator FadeGrow(){
		while (text.color.a>0 ? true : false)
		{
			text.color += new Color (0f, 0f, 0f,-change*Time.deltaTime);
			text.transform.localScale += new Vector3(change*Time.deltaTime,change*Time.deltaTime);
			yield return null;
		}
		gameObject.SetActive (false);
	}
}
