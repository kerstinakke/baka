using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** Effects for backgrounds used by Overlay effects */
public class ImageEffects : MonoBehaviour
{

	private Image image;
	private Color startColor;
    private Color defaultColor;
	[SerializeField] private bool fadingOnce = false;
	private float change = 0.15f;
    private float alpha;


	// Use this for initialization
	void Awake ()
	{
		image = GetComponent<Image> ();
        defaultColor = image.color;
        startColor = image.color;
	}

	void OnEnable ()
	{
		image.color = startColor;
        alpha = startColor.a;
        startColor = defaultColor;
		if (fadingOnce)
			StartCoroutine ("Fade");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (fadingOnce)
			return;
		if (image.color.a <= 0.2f || image.color.a >= alpha)
			change *= -1;
		image.color += new Color (0f, 0f, 0f, change * Time.deltaTime);
	}

	private IEnumerator Fade ()
	{
		bool fading = true;
		while (fading) {
			image.color += new Color (0f, 0f, 0f, -0.2f * Time.deltaTime);
			fading = image.color.a > 0 ? true : false;
			yield return null;
		}
		gameObject.SetActive (false);
	}

    public void SetTempColor(Color temp) {
        startColor = temp;
    }
}
