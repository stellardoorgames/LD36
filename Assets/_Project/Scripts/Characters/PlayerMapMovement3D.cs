using UnityEngine;
using System.Collections;

public class PlayerMapMovement3D : MonoBehaviour {

	public float walkSpeed = 1.0f;
	public float turnSpeed = 0.3f;
	public float waitTime = 3f;

	bool isIdle = false;
	bool isWalking = false;

	float lastMoveTime;

	Animator animator;

	void Start () 
	{
		animator = GetComponent<Animator> ();
	}
	
	void Update () {
		if (FightController.isFighting)// || OptionMenuController.isMenuOpen)
			return;
		
		Vector3 move = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

		isIdle = false;
		isWalking = false;

		if (move.magnitude > 0.1f)
		{
			isWalking = true;

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (move, Vector3.up), turnSpeed);

			transform.position += (move * (walkSpeed * Time.deltaTime));

			lastMoveTime = Time.time;
		}
		else
		{
			if (Time.time > lastMoveTime + waitTime)
				isIdle = true;
		}

		animator.SetBool ("IsWalking", isWalking);
		animator.SetBool ("IsIdle", isIdle);
	}
}
