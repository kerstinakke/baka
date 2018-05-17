using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** change properties of movables for level */
public class LevelMovableProps : MonoBehaviour
{

	[SerializeField] public float AllowedPosError = 0.5f;
	[SerializeField] public float slow = 4f;
	[SerializeField] public float rotAngle = 45f;


}
