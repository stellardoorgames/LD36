using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EnemyCharacter : Character {

	public TextAsset characterDataString;

	public GameObject itemPrefab;

	//public string introTextA;
	//public string introTextB;

	/*public List<string> talkTiers;
	public List<string> replyTeirs;

	public List<string> attackQuips;*/

	//public List<string> attacks;

	public CharacterData data;

	void Awake()
	{
		if (characterDataString != null)
		{
			CharacterHolder holder = JsonConvert.DeserializeObject<CharacterHolder> (characterDataString.text);
			data = holder.character;
			//data = JsonConvert.DeserializeObject<CharacterData> (characterDataString.text);
		}
	}

	protected override void Start ()
	{
		characterName = data.name;
		ego = data.ego;
		damage = data.damage;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public AttackData GetRandomAttack()
	{
		//Ability[] list = new Ability[abilities.Count];
		//abilities.CopyTo (list);

		return data.attacks[Random.Range (0, data.attacks.Length - 1)];
	}

	public virtual string[] TakeAbility(ItemType item)
	{
		string[] returnVal = new string[1] {". . ."};

		ItemConversationData icd = null;
		foreach(ItemConversationData i in data.items)
		{
			//Debug.Log (item.ToString ());
			if (i.item == item.ToString ())
				icd = i;
		}

		//Debug.Log (icd.item);
		if (icd != null)
		{
			returnVal = icd.replies;
			TakeDamage (icd.damage);
		}

		Debug.Log (returnVal [0]);
		return returnVal;
	}

	public override void Die ()
	{
		if (itemPrefab != null)
			Instantiate (itemPrefab, transform.position, Quaternion.identity);

		GameObject.Destroy (gameObject);
	}
}
