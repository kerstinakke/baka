using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** To guarantee cursor is visible and not locked when the scene starts */
public class CursorVisible : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

}
