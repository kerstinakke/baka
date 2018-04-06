using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendScores : MonoBehaviour {

	public Button submitButton;
	public Text submitText;

	UnityWebRequest request;

	public void OnSubmit()
	{

		submitButton.interactable = false;

		string url = "https://script.google.com/macros/s/AKfycbxEJ5ijo0DzvYKSUAAt8yvO-ek2Yib6MmDx5zxjfXOyNB7gDd0/exec";
		//UnityWebRequest request = UnityWebRequest.Get(url);
		Dictionary<string, string> data = new Dictionary<string, string>();
		for (int i = 0; i < 5; i++) {
			string key = "Level" + i;
			if(PlayerPrefs.HasKey(key))
				data.Add (key, string.Format("{0:00}:{1:00}",Mathf.Floor(PlayerPrefs.GetFloat (key)/60),PlayerPrefs.GetFloat(key)%60));
		}
		//data.Add("id", PlayerPrefs.GetInt("id").ToString());
		data.Add("id_hash", PlayerPrefs.GetString("idHash"));
		//data.Add("first_date", PlayerPrefs.GetString("firstDate"));

		request = UnityWebRequest.Post(url, data);
		request.SendWebRequest();
		StartCoroutine("WaitForResponse");
		StartCoroutine("Loading");
	}

	IEnumerator Loading()
	{
		int i = 0;
		while (true)
		{
			submitText.text = "Sending" + ("".PadRight(i, '.'));
			i = (i + 1) % 4;
			yield return new WaitForSeconds(0.3f);
		}
	}

	void LoadingDone()
	{
		submitText.text = "Done";
		StopCoroutine("Loading");
	}

	IEnumerator WaitForResponse()
	{
		while(!request.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}

		Debug.Log(request.responseCode);
		Debug.Log(request.downloadHandler.text);
		LoadingDone();
	}
}
