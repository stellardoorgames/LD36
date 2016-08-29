using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public string characterName;

	public float life = 8f;

	public HashSet<Ability> abilities;

	public Sprite texture;
	public Sprite textureHurt;

	// Use this for initialization
	protected virtual void Start () {
		abilities = new HashSet<Ability> ();
	}

	public void AddAbility(AbilityType abilityType)
	{
		abilities.Add (Ability.list [abilityType]);
	}

	public virtual bool TakeAttack(Ability ability)
	{
		if (Random.value > ability.chance)
			return false;

		//if ()

		TakeDamage (ability.damage);

		return true;
	}

	public void TakeDamage (float damage)
	{
		life -= damage;
		//if (life <= 0f)
		//	Die ();
	}

	public virtual void Die()
	{
		
	}
}
