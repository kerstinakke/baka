using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

	protected Vector3 offset = new Vector3();
	protected Collider myCollider;
	private bool isRotating;
	private float originalY;
	private Transform body;
	protected float speed = 0.5f;
	protected float slow = 4f;
	[SerializeField] protected Vector3 correctPos;
	[SerializeField] protected float posError = 0.2f;
	[SerializeField] protected float rotAngle = 45f;

	public void Start(){
		myCollider = GetComponentInChildren<Collider> ();
		isRotating = false;
		body = transform.Find ("Body");
		transform.position = correctPos;
	}

	public virtual bool Pickup (Vector3 pos){
		offset = transform.position-pos;
		originalY = offset.y;
		myCollider.isTrigger = true;
		return WithinLimits (slow);
	}

	public virtual bool LetGo (){
		myCollider.isTrigger = false;
		print (WithinLimits (posError)+" "+correctPos+" "+transform.position);
		if (WithinLimits (posError)) {
			transform.position = correctPos;
			return true;
		} else
			return false;
	}

	public virtual bool Follow(Vector3 pos){
		transform.position = pos+offset;
		return WithinLimits (slow);
	}

	public void Adjust(float horizontalRot, float vertical, float verticalRot){
		Vector3 rotDir = new Vector3 (0, horizontalRot * rotAngle, verticalRot * rotAngle);
		if(!isRotating && rotDir.magnitude!=0)
			StartCoroutine(ExecuteRotation( body.eulerAngles, body.eulerAngles + rotDir,0.5f));
		if(vertical!=0){
			Vector3 newOffset = offset + new Vector3 (0, vertical * speed * Time.deltaTime);
			if (Mathf.Abs(newOffset.y-originalY) <= 0.5f) {
				offset = newOffset;
			}
		}
	}

	protected virtual bool WithinLimits(float error){
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
