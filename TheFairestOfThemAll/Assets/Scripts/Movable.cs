using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

	private Vector3 offset;
	private Collider collider;
	private bool isRotating;
	private Transform body;
	private float speed = 0.5f;
	private float slow = 4f;
	[SerializeField] protected Vector3 correctPos;
	[SerializeField] protected float posError = 0.2f;
	[SerializeField] protected float horRotAngle = 90f;

	public void Start(){
		collider = GetComponentInChildren<Collider> ();
		isRotating = false;
		body = transform.Find ("Body");
	}

	public bool Pickup (Vector3 pos){
		offset = transform.position-pos;
		collider.isTrigger = true;
		return WithinLimits (slow);
	}

	public bool LetGo (){
		collider.isTrigger = false;
		print (WithinLimits (posError)+" "+correctPos+" "+transform.position);
		if (WithinLimits (posError)) {
			transform.position = correctPos;
			return true;
		} else
			return false;
	}

	public bool Follow(Vector3 pos){
		transform.position = pos+offset;
		return WithinLimits (slow);
	}

	public void Adjust(float horizontal, float vertical){
		Vector3 rotDir = new Vector3 (0, horizontal * horRotAngle,0);
		if(!isRotating && rotDir.magnitude!=0)
			StartCoroutine(ExecuteRotation( body.eulerAngles, body.eulerAngles + rotDir,0.5f));
		if(vertical!=0){
			Vector3 newOffset = offset + new Vector3 (0, vertical * speed * Time.deltaTime);
			if (newOffset.magnitude < 2f) {
				offset = newOffset;
				print (offset);
			}
		}
	}

	protected bool WithinLimits(float error){
		if ((transform.position-correctPos).magnitude <= error) {
			return true;
		}
		return false;
	}

	protected IEnumerator ExecuteRotation(Vector3 from, Vector3 to, float duration)
	{
		if (duration < float.Epsilon)
		{
			body.eulerAngles = to;
			yield break;
		}

		isRotating = true;
		float agregate = 0;
		while (agregate < 1f)
		{
			agregate += Time.deltaTime / duration;
			body.eulerAngles = Vector3.Lerp(from, to, agregate);
			yield return null;
		}
		isRotating = false;
	}
		

}
