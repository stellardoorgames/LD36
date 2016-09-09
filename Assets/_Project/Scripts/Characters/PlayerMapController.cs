using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMapController : Character {

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

	float weight = 0f;
	Animator animator;

	// Use this for initialization
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		position = rb2d.position;
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

			lastMoveTime = Time.time;
		}
		else
		{
			if (Time.time > lastMoveTime + waitTime)
				isIdle = true;
		}

		//animator.SetBool ("IsWalking", isWalking);
		//animator.SetBool ("IsIdle", isIdle);
	}

	void FixedUpdate()
	{
		rb2d.MovePosition (position);
	}


}
