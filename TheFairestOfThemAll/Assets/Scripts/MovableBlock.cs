using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : Movable {

	protected override bool WithinLimits(float whatever){
		return true;
	}

	public override bool LetGo (){
		myCollider.isTrigger = false;
		return false;
	}
}
