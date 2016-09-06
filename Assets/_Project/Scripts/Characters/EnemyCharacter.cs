using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EnemyCharacter : Character {
	
	public Color color;

	public TextAsset characterDataString;

	public GameObject itemPrefab;

	public CharacterData data;

	float damage;


	void Awake()
	{
		if (characterDataString != null)
		{
			CharacterHolder holder = JsonConvert.DeserializeObject<CharacterHolder> (characterDataString.text);
			data = holder.character;
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
		return data.attacks[Random.Range (0, data.attacks.Length - 1)];
	}

	/*public TierData TakeTalk(ReplyData data)
	{
		
	}*/

	public virtual ItemConversationData TakeAbility(ItemType item)
	{
		//string[] returnVal = new string[1] {". . ."};

		ItemConversationData icd = null;
		foreach(ItemConversationData i in data.items)
		{
			//Debug.Log (item.ToString ());
			if (i.item == item.ToString ())
				icd = i;
		}

		if (icd != null)
		{
			//returnVal = icd.replies;
			TakeDamage (icd.damage);
		}

		return icd;
		//Debug.Log (returnVal [0]);
		//return returnVal;
	}

	public override void Die ()
	{
		if (itemPrefab != null)
			Instantiate (itemPrefab, transform.position, Quaternion.identity);

		GameObject.Destroy (gameObject);
	}
}
