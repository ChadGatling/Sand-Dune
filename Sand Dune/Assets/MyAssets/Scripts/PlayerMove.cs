using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	private CharacterController charController;
	private bool isJumping;

	[HideInInspector] public bool hasParent;
	public float movementSpeed;
	public AnimationCurve jumpFallOff;
	public float jumpMultiplier;
	[SerializeField] Rigidbody shipPhysics;

	Vector3 moveDirection;

	void Awake () {
		charController = GetComponent<CharacterController>();
	}

	void Update() {
		JumpInput();
		PlayerMovement();
	}

	void PlayerMovement() {
		bool isGrounded = charController.isGrounded;

		if (isGrounded) {
			float vertInput = Input.GetAxis("Vertical");
			float horInput = Input.GetAxis("Horizontal");

			Vector3 forwardMovement = transform.forward * vertInput;
			Vector3 rightMovement = transform.right * horInput;
			
            moveDirection = rightMovement + forwardMovement;
            moveDirection *= movementSpeed;	
			// moveDirection += shipPhysics.velocity;
		} else {			
			moveDirection.y -= 20 * Time.deltaTime;
		}
		
		if (Input.GetButton("Vertical") || Input.GetButton("Horizontal") || !isGrounded) // still need to figure out how to move the player with the ship at all times.
			if (hasParent)
				charController.Move((moveDirection + (shipPhysics.velocity)) * Time.deltaTime);
			else
				charController.Move(moveDirection * Time.deltaTime);
	}
	
	void JumpInput() {
		if (Input.GetButtonDown("Jump")) {
 			if (!isJumping){
				isJumping = true;
				StartCoroutine(JumpEvent());
			}
		}
	}

	IEnumerator JumpEvent() {
		charController.slopeLimit = 90.0f;
		float timeInAir = 0.0f;

		do {
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;
			yield return null;
		} while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

		charController.slopeLimit = 45.0f;
		isJumping = false;
	}
}
