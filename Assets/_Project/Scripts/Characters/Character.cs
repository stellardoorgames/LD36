using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public string characterName;

	public float maxEgo = 8f;
	public float ego = 8f;

	public float damage;

	//public HashSet<Ability> abilities;

	public Sprite texture;
	public Sprite textureHurt;

	// Use this for initialization
	protected virtual void Start () {
		//abilities = new HashSet<Ability> ();
	}

	/*public void AddAbility(AbilityType abilityType)
	{
		abilities.Add (Ability.list [abilityType]);
	}*/

	public virtual bool TakeAttack(float damage)
	{
		if (Random.value > 0.8f)
			return false;
		
		TakeDamage (damage);

		return true;
	}

	public void TakeDamage (float damage)
	{
		ego -= damage;
		//if (life <= 0f)
		//	Die ();
	}

	public virtual void Die()
	{
		
	}
}
