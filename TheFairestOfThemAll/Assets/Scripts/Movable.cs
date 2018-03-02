using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

	private Vector3 offset;
	private Collider collider;
	private bool isRotating;
	[SerializeField] protected Vector3 correctPos;
	[SerializeField] protected float PosError = 0.2f;
	[SerializeField] protected float RotAngle = 90f;

	public void Start(){
		collider = GetComponent<Collider> ();
		isRotating = false;
	}

	public void Pickup (Vector3 pos){
		offset = transform.position-pos;
		collider.isTrigger = true;
	}

	public void LetGo (){
		collider.isTrigger = false;
	}

	public void Follow(Vector3 pos){
		transform.position = pos+offset;
	}

	public void Rotate(float horizontal, float vertical){
		print (horizontal + " " + vertical);
		transform.eulerAngles += new Vector3(0,horizontal*RotAngle,vertical*RotAngle);
	}

	protected IEnumerator ExecuteRotation(Vector3 from, Vector3 to, float duration)
	{
		if (duration < float.Epsilon)
		{
			transform.eulerAngles = to;
			yield break;
		}

		isRotating = true;
		float agregate = 0;
		while (agregate < 1f)
		{
			agregate += Time.deltaTime / duration;
			transform.eulerAngles = Vector3.Lerp(from, to, agregate);
			yield return null;
		}
		isRotating = false;
	}
}
