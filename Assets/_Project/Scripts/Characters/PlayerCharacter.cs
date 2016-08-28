using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : Character {

	public float walkSpeed = 1.0f;
	public float turnSpeed = 0.3f;
	public float waitTime = 3f;

	public GameObject cam;
	public GameObject magnifier;
	public GameObject antenna;
	public GameObject speakers;
	public GameObject printer;

	public FightController fightController;

	public HashSet<Item> items;
	//public List<Item> items;

	bool isWalking = false;
	bool isIdle = false;
	float lastMoveTime;
	float weight = 0f;

	Animator animator;



	protected override void Start ()
	{
		base.Start ();

		items = new HashSet<Item> ();

		//AddAbility (AbilityType.Eye_Strain);
		AddAbility (AbilityType.Smirk);
		AddAbility (AbilityType.Run);

		animator = GetComponent<Animator> ();

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

	/*public void UpdateItems()
	{
		//if (character.)
		if ()
	}*/

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			Debug.Log ("Encounter Enemy");
			Character enemy = other.GetComponent<Character> ();
			fightController.StartFight (this, enemy);
			/*if (!fightController.gameObject.activeSelf)
			{
				fightController.gameObject.SetActive (true);
			}*/
		}
		else if (other.tag == "Item")
		{
			Debug.Log ("Pickup Item");
			Item item = other.GetComponent<Item> ();
			AddItem (item);
			GameObject.Destroy (other.gameObject);
			Debug.Log (items.Count);
		}
	}

	void AddItem(Item item)
	{
		items.Add (item);
		foreach(AbilityType abilityType in item.abilities)
		{
			AddAbility (abilityType);
		}

		foreach(Item i in items)
		{
			if (i.type == ItemType.Antenne)
				antenna.SetActive (true);
			else if (i.type == ItemType.Camera)
				cam.SetActive (true);
			else if (i.type == ItemType.Magnifier)
				magnifier.SetActive (true);
			else if (i.type == ItemType.Printer)
				printer.SetActive (true);
			else if (i.type == ItemType.Speakers)
				speakers.SetActive (true);
			
		}

		weight = Mathf.InverseLerp (0, 5, items.Count);
		animator.SetFloat ("Weight", weight);
	}
}
