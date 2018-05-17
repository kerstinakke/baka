/* Based on code by Raimond Tunnel */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/** Automatcally collect player scores for test purposes */
public class SendFeedback : MonoBehaviour
{

	public Text submitText;
	public bool done = false;

	UnityWebRequest request;

	public void SendTime (string levelName, float time)
	{
		string url = "https://script.google.com/macros/s/AKfycbxEJ5ijo0DzvYKSUAAt8yvO-ek2Yib6MmDx5zxjfXOyNB7gDd0/exec";
		Dictionary<string, string> data = new Dictionary<string, string> ();

		string key = levelName;
		if (PlayerPrefs.HasKey (key))
			data.Add (key, string.Format ("{0:00}:{1:00}", Mathf.Floor (time / 60), time % 60));

		data.Add ("id_hash", PlayerPrefs.GetString ("idHash"));

		request = UnityWebRequest.Post (url, data);
		request.SendWebRequest ();
		StartCoroutine ("WaitForResponse");
		StartCoroutine ("Loading");
	}


	IEnumerator Loading ()
	{
		int i = 0;
		while (true) {
			submitText.text = "Loading" + ("".PadRight (i, '.'));
			i = (i + 1) % 4;
			yield return new WaitForSeconds (0.3f);
		}
	}

	void LoadingDone ()
	{
		StopCoroutine ("Loading");
		done = true;
	}

	IEnumerator WaitForResponse ()
	{
		while (!request.isDone) {
			yield return new WaitForSeconds (0.1f);
		}

		Debug.Log (request.responseCode);
		Debug.Log (request.downloadHandler.text);
		LoadingDone ();
	}

	/** Opens related questionnaire in browser */
	public void SendToQuestions ()
	{
		Application.OpenURL ("https://goo.gl/forms/hdlK757lEWyKNPPJ3");
	}
}
