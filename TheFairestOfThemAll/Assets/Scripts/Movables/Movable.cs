using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Class for movable objects */
public class Movable : MonoBehaviour
{

	protected Vector3 offset = new Vector3 ();
	public Collider myCollider;
	protected Vector3 colliderOffset;
	private bool isRotating;
	private float originalY;
	protected Transform body;
	protected float speed = 0.5f;
	[SerializeField]protected Vector3 correctPos;
	protected float posError = 0.5f;
	protected float slow = 4f;
	protected float rotAngle = 45f;

	public void Start ()
	{
		myCollider = GetComponentInChildren<Collider> ();
		colliderOffset = myCollider.bounds.center - transform.position;
		isRotating = false;
		body = transform.Find ("Body");
		GameObject propertiesObject = GameObject.FindWithTag ("Properties");
		if (propertiesObject != null) {
			LevelMovableProps properties = propertiesObject.GetComponent<LevelMovableProps> ();
			posError = properties.AllowedPosError;
			slow = properties.slow;
			rotAngle = properties.rotAngle;
		}
		
	}

	public virtual bool Pickup (Vector3 pos)
	{
		offset = transform.position - pos;
		originalY = offset.y;
		myCollider.gameObject.layer = LayerMask.NameToLayer ("Movable");
		return WithinLimits (slow);
	}

	public virtual bool LetGo ()
	{
		myCollider.gameObject.layer = LayerMask.NameToLayer ("Default");
		//print (WithinLimits (posError) + " " + correctPos + " " + transform.position);
		if (WithinLimits (posError)) {
			transform.position = correctPos;
			return true;
		} else
			return false;
	}

	public virtual bool Follow (Vector3 pos)
	{
		if (!Physics.CheckBox (pos + offset + colliderOffset, myCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask ("Default")))
			transform.position = pos + offset;
		else
			offset = transform.position - pos;
		return WithinLimits (slow);
	}

	public virtual void Adjust (float horizontalRot, float vertical, float verticalRot)
	{
		Vector3 rotDir = new Vector3 (0, horizontalRot * rotAngle, verticalRot * rotAngle);
		if (!isRotating && rotDir.magnitude != 0)
			StartCoroutine (ExecuteRotation (body.eulerAngles, body.eulerAngles + rotDir, rotAngle / 90f));
		if (vertical != 0) {
			Vector3 newOffset = offset + new Vector3 (0, vertical * speed * Time.deltaTime);
			if (!Physics.CheckBox (transform.position + colliderOffset + new Vector3 (0, vertical * speed * Time.deltaTime), myCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask ("Default"))) {
				offset = newOffset;
			}
		}
	}

	protected virtual bool WithinLimits (float error)
	{
		if ((transform.position - correctPos).magnitude <= error) {
			return true;
		}
		return false;
	}

	protected IEnumerator ExecuteRotation (Vector3 from, Vector3 to, float duration)
	{
		if (duration < float.Epsilon) {
			body.eulerAngles = to;
			yield break;
		}

		isRotating = true;
		float agregate = 0;
		while (agregate < 1f) {
			agregate += Time.deltaTime / duration;
			body.eulerAngles = Vector3.Lerp (from, to, agregate);
			yield return null;
		}
		isRotating = false;
	}

}
