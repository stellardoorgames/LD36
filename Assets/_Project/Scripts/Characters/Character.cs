﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public string characterName;

	public float life = 20f;

	public List<Ability> abilities;

	// Use this for initialization
	protected virtual void Start () {
		abilities = new List<Ability> ();
	}

	public void AddAbility(AbilityType abilityType)
	{
		abilities.Add (Ability.list [abilityType]);
	}

	public void TakeDamage (float damage)
	{
		life -= damage;
		if (life <= 0f)
			Die ();
	}

	public void Die()
	{
		
	}
}
