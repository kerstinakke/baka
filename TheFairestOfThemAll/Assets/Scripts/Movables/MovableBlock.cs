using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Doesn't have right position */
public class MovableBlock : Movable
{

	protected override bool WithinLimits (float whatever)
	{
		return false;
	}

	public override bool LetGo ()
	{
		myCollider.isTrigger = false;
		myCollider.gameObject.layer = LayerMask.NameToLayer ("Default");
		return false;
	}
}
