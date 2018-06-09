using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** Different effects to give player feedback */
public class OverlayEffects : MonoBehaviour
{

	[SerializeField]private GameObject rotateEffect;
	[SerializeField]private GameObject holdEffect;
	[SerializeField]private GameObject correctEffect;
	[SerializeField]private GameObject dropEffect;
	[SerializeField]private GameObject aimActive;
	private static Text timeText;
    private static OverlayEffects instance;

	void Start ()
	{
		timeText = transform.Find ("Time").GetComponent<Text> ();
        instance = this;
	}

	public void HoldEffect ()
	{
		rotateEffect.SetActive (false);
		holdEffect.SetActive (true);
		correctEffect.SetActive (false);
	}

	public void RotateEffect (bool rotateMode)
	{
		if (!rotateMode) {
			HoldEffect ();
			return;
		}
		rotateEffect.SetActive (true);
		holdEffect.SetActive (false);
		correctEffect.SetActive (false);
	}

	public void CorrectEffect (bool playCorrect)
	{
		rotateEffect.SetActive (false);
		holdEffect.SetActive (false);
		if (playCorrect)
			correctEffect.SetActive (true);
	}

	public void DropEffect (bool playCorrect)
	{
		rotateEffect.SetActive (false);
		holdEffect.SetActive (false);
		if (playCorrect)
			correctEffect.SetActive (true);
		dropEffect.SetActive (true);
	}

	public void AimActive (bool activate)
	{
		aimActive.SetActive (activate);
	}

	public static void ShowTime (string time)
	{
		timeText.text = time;
	}

    public static void halfCorrect() {
        instance.correctEffect.SetActive(true);
        instance.correctEffect.GetComponent<ImageEffects>().SetTempColor(new Color(0.5f,0.5f,0,0.5f));
        instance.correctEffect.SetActive(false);
    }

}
