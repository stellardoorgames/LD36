using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMapMovement25D : MonoBehaviour {

	//public float ego = 8;
	public float maxEgo = 8;

	public float walkSpeed = 0f;

	public bool isWalking = false;
	public bool isIdle = false;
	public float waitTime = 0f;
	float lastMoveTime = 0f;

	Rigidbody2D rb2d;
	Vector3 position;

	public FightController fightController;
	public HashSet<Item> items;
	public HashSet<ItemType> abilityList;

	//float weight = 0f;
	public Animator animatorFrontBack;
	public Animator animatorSide;
	public Animator animFrontShadow;
	public Animator animSideShadow;
	Animator currentAnimator;
	Animator currentShadow;
	public Transform flipObject;
	Quaternion startingRotation;
	Quaternion flippedRotation;
	Quaternion shadowStartingRotation;
	Quaternion shadowFlippedRotation;


	// Use this for initialization
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		position = rb2d.position;

		startingRotation = flipObject.rotation;
		flippedRotation = startingRotation * Quaternion.Euler (0f, 180f, 0f);

		shadowStartingRotation = animFrontShadow.transform.rotation;
		shadowFlippedRotation = shadowStartingRotation * Quaternion.Euler (0f, 180f, 0f);

		SetToSide ();
	}

	// Update is called once per frame
	void Update () 
	{
		Vector3 move = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

		isIdle = false;
		isWalking = false;

		if (move.magnitude > 0.1f)
		{
			isWalking = true;

			//transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (move, Vector3.up), turnSpeed);
			position = rb2d.position;
			position += (move * (walkSpeed * Time.deltaTime));

			//transform.position += (move * (walkSpeed * Time.deltaTime));

			flipObject.rotation = (move.y < 0f) ? flippedRotation : startingRotation;
			animFrontShadow.transform.rotation = (move.y < 0f) ? shadowFlippedRotation : shadowStartingRotation;

			/*if (move.y < 0f)
				flipObject.rotation = flippedRotation;
			else
				flipObject.rotation = startingRotation;*/

			animatorSide.transform.localScale = (move.x > 0f) ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
			animSideShadow.transform.localScale = (move.x > 0f) ? new Vector3(1f, 0.5f, 0.1f) : new Vector3(-1f, 0.5f, 0.1f);

			lastMoveTime = Time.time;
		}
		else
		{
			if (Time.time > lastMoveTime + waitTime)
				isIdle = true;
		}

		if (move.y > 0.1f)
			SetToFrontBack ();
		else if (move.x < -0.1f || move.x > 0.1f)
			SetToSide ();
		else if (move.y < -0.1f)
			SetToFrontBack ();
			
		currentAnimator.SetFloat ("Vertical", move.y * 8);
		currentShadow.SetFloat ("Vertical", move.y * 8);

		currentAnimator.SetFloat ("Horizontal", move.x * 8);
		currentShadow.SetFloat ("Horizontal", move.x * 8);

		//animator.SetBool ("IsWalking", isWalking);
		//animator.SetBool ("IsIdle", isIdle);
	}

	void SetToSide()
	{
		animatorSide.gameObject.SetActive (true);
		animSideShadow.gameObject.SetActive (true);

		animatorFrontBack.gameObject.SetActive (false);
		animFrontShadow.gameObject.SetActive (false);

		currentAnimator = animatorSide;
		currentShadow = animSideShadow;
	}
	void SetToFrontBack()
	{
		animatorSide.gameObject.SetActive (false);
		animSideShadow.gameObject.SetActive (false);

		animatorFrontBack.gameObject.SetActive (true);
		animFrontShadow.gameObject.SetActive (true);

		currentAnimator = animatorFrontBack;
		currentShadow = animFrontShadow;
	}

	void FixedUpdate()
	{
		rb2d.MovePosition (position);
	}


}
