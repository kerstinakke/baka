using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPiece : MovablePiece {

	private Tutorial tutorial;

	void Start(){
		tutorial = GameObject.Find ("TutorialStuff").GetComponent<Tutorial> ();
		base.Start ();
	}

	public override bool Pickup (Vector3 pos){
		tutorial.CarryPiece ();
		return base.Pickup (pos);
	}
		
		
}
