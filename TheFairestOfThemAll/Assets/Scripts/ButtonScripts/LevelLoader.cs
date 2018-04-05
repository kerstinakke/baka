using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	[SerializeField] private int index;

	void Start(){
		gameObject.GetComponent<Button> ().interactable = PlayerPrefs.HasKey ( "Level"+(index-2)) || index<=1; //previous level done or is tutorial level
	}

	public void LoadLevel(){
		SceneManager.LoadScene (index);
	}

	public string GetLevelName(){
		return "Level" + (index - 1);
	}

	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
