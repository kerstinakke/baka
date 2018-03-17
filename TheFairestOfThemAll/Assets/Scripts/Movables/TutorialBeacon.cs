using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBeacon : MovableBeacon {

	private Tutorial tutorial;

	void Start(){
		tutorial = GameObject.Find ("TutorialStuff").GetComponent<Tutorial> ();
		base.Start ();
	}

	public override bool Pickup (Vector3 pos){
		tutorial.CarryBeacon ();
		return base.Pickup (pos);
	}

	public override bool LetGo(Vector3 pos){
		bool success = base.LetGo (pos);
		if (success) {
			tutorial.LeadToPiece ();
		}
		return success;
	}
}
