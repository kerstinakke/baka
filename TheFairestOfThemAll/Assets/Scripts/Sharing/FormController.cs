using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FormController : MonoBehaviour {

    public InputField nameField;
    public InputField scoreField;
    public Button submitButton;
    public Text submitText;

    UnityWebRequest request;
    Coroutine loadingCoroutine;

    // Use this for initialization
    void Start () {
        submitButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTextChange()
    {
        if ("" != nameField.text && "" != scoreField.text) //I'm sure there is a better way to do this
        {
            submitButton.interactable = true;
        }
    }

    public void OnSubmit()
    {

        submitButton.interactable = false;

        string url = "https://script.google.com/macros/s/AKfycbxi_4rxgO15fp_I4Rb0613olf_PHhMofQ5Vk4gOdQS5ZqBVjzLF/exec";
        //UnityWebRequest request = UnityWebRequest.Get(url);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("name", nameField.text);
        data.Add("score", scoreField.text);
        data.Add("id", PlayerPrefs.GetInt("id").ToString());
        data.Add("id_hash", PlayerPrefs.GetString("idHash"));
        data.Add("first_date", PlayerPrefs.GetString("firstDate"));

        request = UnityWebRequest.Post(url, data);
        request.SendWebRequest();
        StartCoroutine("WaitForResponse");
        loadingCoroutine = StartCoroutine("Loading");
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
        StopCoroutine(loadingCoroutine);
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
