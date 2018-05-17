using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Save and quit */
public class ExitGame : MonoBehaviour
{

	public void QuitGame ()
	{
		PlayerPrefs.Save ();
		Application.Quit ();
	}
}
