using UnityEngine;
using System.Collections;

public class PlayerMapController : MonoBehaviour {
/*
	public float walkSpeed = 1.0f;
	public float turnSpeed = 0.3f;
	public float waitTime = 3f;

	public GameObject camera;
	public GameObject magnifier;
	public GameObject antenna;
	public GameObject Speakers;
	public GameObject Printer;

	public FightController fightController;

	bool isWalking = false;
	bool isIdle = false;
	float lastMoveTime;
	float weight = 0f;
	
	Animator animator;

	void Start () 
	{
		animator = GetComponent<Animator> ();

		fightController.gameObject.SetActive (false);
		//camera.SetActive (false);
		//magnifier.SetActive (false);

	}
	
	void Update () 
	{
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

	public void UpdateItems(Character character)
	{
		//if (character.)
		animator.SetFloat ("Weight", weight);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			Character enemy = other.GetComponent<Character> ();
			fightController.gameObject.SetActive (true);
			fightController.StartFight (this, enemy);
		}
	}*/
}
