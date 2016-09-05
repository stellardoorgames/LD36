using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerCharacter : Character {

	public float maxEgo = 8f;

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

	public HashSet<ItemType> abilityList;

	public Item item1;
	public Item item2;
	public Item item3;
	public Item item4;
	public Item item5;

	//public Dictionary<ItemType, GameObject> attachments;

	protected override void Start ()
	{
		base.Start ();

		items = new HashSet<Item> ();
		abilityList = new HashSet<ItemType> ();

		//AddAbility (AbilityType.Eye_Strain);
		//AddAbility (AbilityType.Smirk);
		//AddAbility (AbilityType.Run);

		animator = GetComponent<Animator> ();

	}

	void Update () 
	{
		if (FightController.isFighting)// || OptionMenuController.isMenuOpen)
			return;

		if (Input.GetKeyDown (KeyCode.Alpha1))
			AddItem (item1);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			AddItem (item2);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			AddItem (item3);
		if (Input.GetKeyDown (KeyCode.Alpha4))
			AddItem (item4);
		if (Input.GetKeyDown (KeyCode.Alpha5))
			AddItem (item5);
		
	}

	/*public void UpdateItems()
	{
		//if (character.)
		if ()
	}*/

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			Debug.Log ("Encounter Enemy");
			EnemyCharacter enemy = other.GetComponent<EnemyCharacter> ();
			fightController.StartFight (this, enemy);
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

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			Debug.Log ("Encounter Enemy");
			EnemyCharacter enemy = other.GetComponent<EnemyCharacter> ();
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
		if (abilityList.Contains (item.type))
			return;

		items.Add (item);
		abilityList.Add(item.type);

		//foreach(ItemType type in abilityList)
			

		/*foreach(AbilityType abilityType in item.abilities)
		{
			AddAbility (abilityType);
		}*/

		foreach(Item i in items)
		{
			if (i.type == ItemType.reciever)
				antenna.SetActive (true);
			else if (i.type == ItemType.camera)
				cam.SetActive (true);
			else if (i.type == ItemType.screen)
				magnifier.SetActive (true);
			else if (i.type == ItemType.printer)
				printer.SetActive (true);
			else if (i.type == ItemType.speakers)
				speakers.SetActive (true);
			
		}

		maxEgo += 2f;

		weight = Mathf.InverseLerp (0, 5, items.Count);
		animator.SetFloat ("Weight", weight);
	}

	public override void Die ()
	{
		ego = maxEgo;
	}
}
