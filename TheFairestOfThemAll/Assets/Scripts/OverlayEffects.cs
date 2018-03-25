using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayEffects : MonoBehaviour {

	[SerializeField]private GameObject rotateEffect;
	[SerializeField]private GameObject holdEffect;
	[SerializeField]private GameObject correctEffect;

	public void HoldEffect(){
		rotateEffect.SetActive (false);
		holdEffect.SetActive (true);
		correctEffect.SetActive (false);

	}

	public void RotateEffect(bool rotateMode){
		if (!rotateMode) {
			HoldEffect ();
			return;
		}
		rotateEffect.SetActive (true);
		holdEffect.SetActive (false);
		correctEffect.SetActive (false);
	}

	public void CorrectEffect(bool playCorrect){
		rotateEffect.SetActive (false);
		holdEffect.SetActive (false);
		if(playCorrect)
			correctEffect.SetActive (true);
	}
		

}
