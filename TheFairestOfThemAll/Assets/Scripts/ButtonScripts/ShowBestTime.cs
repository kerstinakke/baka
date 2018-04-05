using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBestTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string level = GetComponentInParent<LevelLoader>(). GetLevelName ();
		if (PlayerPrefs.HasKey (level)) {
			GetComponent<Text> ().text = string.Format ("{0:00}:{1:00}", Mathf.Floor(PlayerPrefs.GetFloat (level)/60),PlayerPrefs.GetFloat (level)%60);
		}
	}
	

}
