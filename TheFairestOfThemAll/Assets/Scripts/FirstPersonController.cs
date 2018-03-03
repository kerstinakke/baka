 using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent (typeof(CharacterController))]
	[RequireComponent (typeof(AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField] private bool IsWalking;
		[SerializeField] private float WalkSpeed;
		[SerializeField] private float RunSpeed;
		[SerializeField] [Range (0f, 1f)] private float RunstepLenghten;
		[SerializeField] private float JumpSpeed;
		[SerializeField] private float StickToGroundForce;
		[SerializeField] private float GravityMultiplier;
		[SerializeField] private MouseLook MouseLook;
		[SerializeField] private bool UseFovKick;
		[SerializeField] private FOVKick FovKick = new FOVKick ();
		[SerializeField] private bool UseHeadBob;
		[SerializeField] private CurveControlledBob HeadBob = new CurveControlledBob ();
		[SerializeField] private LerpControlledBob JumpBob = new LerpControlledBob ();
		[SerializeField] private float StepInterval;
		[SerializeField] private AudioClip[] FootstepSounds;
		// an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip JumpSound;
		// the sound played when character leaves the ground.
		[SerializeField] private AudioClip LandSound;
		// the sound played when character touches back on ground.

		private float OriginalSpeed;
		private Camera Camera;
		private bool Jump;
		private Movable Holding;
		private bool WasHoldingEDown;
		private bool WasHoldingRDown;
		private bool RotateMode;
		private float YRotation;
		private Vector2 input;
		private Vector3 MoveDir = Vector3.zero;
		private CharacterController CharacterController;
		private CollisionFlags CollisionFlags;
		private bool PreviouslyGrounded;
		private Vector3 OriginalCameraPosition;
		private float StepCycle;
		private float NextStep;
		private bool Jumping;
		private AudioSource AudioSource;
		private GameObject HoldEffect;

		// Use this for initialization
		private void Start ()
		{
			CharacterController = GetComponent<CharacterController> ();
			Camera = GetComponentInChildren<Camera> ();
			Camera.enabled=true;
			OriginalCameraPosition = Camera.transform.localPosition;
			FovKick.Setup (Camera);
			HeadBob.Setup (Camera, StepInterval);
			StepCycle = 0f;
			NextStep = StepCycle / 2f;
			Jumping = false;
			WasHoldingEDown = false;
			WasHoldingRDown = false;
			RotateMode = false;
			AudioSource = GetComponent<AudioSource> ();
			MouseLook.Init (transform, Camera.transform);
			OriginalSpeed = WalkSpeed;
			HoldEffect = transform.Find ("Canvas").gameObject;
			HoldEffect.SetActive (false);
		
		}


		// Update is called once per frame
		private void Update ()
		{
			RotateView ();

			if ((Holding as MovablePiece) != null) {
				bool RDown = CrossPlatformInputManager.GetButtonDown ("ToggleRotate");
				if (!WasHoldingRDown && RDown) {
					RotateMode = !RotateMode;
					HoldEffect.GetComponent<OverlayEffects> ().SwitchSprite ();
					print("rotate Toggle!");
				}
				WasHoldingRDown = RDown;
			}

			// the jump state needs to read here to make sure it is not missed
			if (!Jump) {
				Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
			}

			if (!PreviouslyGrounded && CharacterController.isGrounded) {
				StartCoroutine (JumpBob.DoBobCycle ());
				PlayLandingSound ();
				MoveDir.y = 0f;
				Jumping = false;
			}
			if (!CharacterController.isGrounded && !Jumping && PreviouslyGrounded) {
				MoveDir.y = 0f;
			}

			PreviouslyGrounded = CharacterController.isGrounded;


			bool EDown = CrossPlatformInputManager.GetButtonDown ("Hold");
			//if not pressing e previous frame, but is now
			if (!WasHoldingEDown && EDown) {
				if (Holding == null) {
					RaycastHit hitInfo;
					if (Physics.Raycast (Camera.transform.position, Camera.transform.forward, out hitInfo, 2)) {
						GameObject aming = hitInfo.collider.gameObject;
						Movable movingScript = aming.GetComponentInParent<Movable> ();
						if (movingScript != null) {
							WalkSpeed = 1f;
							Holding = movingScript;
							Holding.Pickup (transform.position);
							MovableBeacon beacon = Holding as MovableBeacon;
							if (beacon != null) {
								beacon.Inactivate();
								beacon.SetCameraActive (true);
								Camera.enabled = false;
							}
							HoldEffect.SetActive (true);
							print ("Pickup");
						}
					}
				} else {
					WalkSpeed = OriginalSpeed;
					MovableBeacon beacon = Holding as MovableBeacon;
					if (beacon != null) {
						beacon.LetGo (transform.position);
						Camera.enabled = true;
						beacon.SetCameraActive (false);
					}else 
						Holding.LetGo ();
					Holding = null;
					RotateMode = false;
					HoldEffect.SetActive (false);
					print ("LetDown");
				}
			}

			WasHoldingEDown = EDown;
		}

		private void PlayLandingSound ()
		{
			AudioSource.clip = LandSound;
			AudioSource.Play ();
			NextStep = StepCycle + .5f;
		}

		private void FixedUpdate ()
		{	
			float speed;
			if (!RotateMode) {
				GetInput (out speed);
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

				// get a normal for the surface that is being touched to move along it
				RaycastHit hitInfo;
				Physics.SphereCast (transform.position, CharacterController.radius, Vector3.down, out hitInfo,
					CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
				desiredMove = Vector3.ProjectOnPlane (desiredMove, hitInfo.normal).normalized;

				MoveDir.x = desiredMove.x * speed;
				MoveDir.z = desiredMove.z * speed;
			} else {
				Holding.Rotate (CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw ("Vertical"));
				speed = 0;
				MoveDir.x = 0;
				MoveDir.z = 0;
			}
				if (CharacterController.isGrounded) {
					MoveDir.y = -StickToGroundForce;

					if (Jump) {
						MoveDir.y = JumpSpeed;
						PlayJumpSound ();
						Jump = false;
						Jumping = true;
					}
				} else {
					MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
				}
				CollisionFlags = CharacterController.Move (MoveDir * Time.fixedDeltaTime);
				if (Holding != null) {
					Holding.Follow (transform.position);
				}

				ProgressStepCycle (speed);
				UpdateCameraPosition (speed);

			MouseLook.UpdateCursorLock ();
			

		}


		private void PlayJumpSound ()
		{
			AudioSource.clip = JumpSound;
			AudioSource.Play ();
		}


		private void ProgressStepCycle (float speed)
		{
			if (CharacterController.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0)) {
				StepCycle += (CharacterController.velocity.magnitude + (speed * (IsWalking ? 1f : RunstepLenghten))) *
				Time.fixedDeltaTime;
			}

			if (!(StepCycle > NextStep)) {
				return;
			}

			NextStep = StepCycle + StepInterval;

			PlayFootStepAudio ();
		}


		private void PlayFootStepAudio ()
		{
			if (!CharacterController.isGrounded) {
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			int n = Random.Range (1, FootstepSounds.Length);
			AudioSource.clip = FootstepSounds [n];
			AudioSource.PlayOneShot (AudioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			FootstepSounds [n] = FootstepSounds [0];
			FootstepSounds [0] = AudioSource.clip;
		}


		private void UpdateCameraPosition (float speed)
		{
			Vector3 newCameraPosition;
			if (!UseHeadBob) {
				return;
			}
			if (CharacterController.velocity.magnitude > 0 && CharacterController.isGrounded) {
				Camera.transform.localPosition =
                    HeadBob.DoHeadBob (CharacterController.velocity.magnitude +
				(speed * (IsWalking ? 1f : RunstepLenghten)));
				newCameraPosition = Camera.transform.localPosition;
				newCameraPosition.y = Camera.transform.localPosition.y - JumpBob.Offset ();
			} else {
				newCameraPosition = Camera.transform.localPosition;
				newCameraPosition.y = OriginalCameraPosition.y - JumpBob.Offset ();
			}
			Camera.transform.localPosition = newCameraPosition;
		}



		private void GetInput (out float speed)
		{
			// Read input
			float horizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			float vertical = CrossPlatformInputManager.GetAxis ("Vertical");

			bool waswalking = IsWalking;

#if !MOBILE_INPUT
			// On standalone builds, walk/run speed is modified by a key press.
			// keep track of whether or not the character is walking or running
			IsWalking = !Input.GetKey (KeyCode.LeftShift);
#endif
			// set the desired speed to be walking or running
			speed = IsWalking ? WalkSpeed : RunSpeed;
			input = new Vector2 (horizontal, vertical);

			// normalize input if it exceeds 1 in combined length:
			if (input.sqrMagnitude > 1) {
				input.Normalize ();
			}

			// handle speed change to give an fov kick
			// only if the player is going to a run, is running and the fovkick is to be used
			if (IsWalking != waswalking && UseFovKick && CharacterController.velocity.sqrMagnitude > 0) {
				StopAllCoroutines ();
				StartCoroutine (!IsWalking ? FovKick.FOVKickUp () : FovKick.FOVKickDown ());
			}
		}


		private void RotateView ()
		{
			MouseLook.LookRotation (transform, Camera.transform);
		}


		private void OnControllerColliderHit (ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
			//dont move the rigidbody if the character is on top of it
			if (CollisionFlags == CollisionFlags.Below) {
				return;
			}

			if (body == null || body.isKinematic) {
				return;
			}
			body.AddForceAtPosition (CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
		}
			
	}
		
}
