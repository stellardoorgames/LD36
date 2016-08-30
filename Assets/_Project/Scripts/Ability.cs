using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Attack
{
	
}

public enum AbilityType
{
	Run,
	Eye_Strain,
	Roll_Eyes,
	Smirk
}

public enum AbilityElement
{
	Paper,
	Rock,
	Scissors
}

public class Ability
{
	public static Dictionary<AbilityType, Ability> list = new Dictionary<AbilityType, Ability> {
		{AbilityType.Eye_Strain, new Ability(AbilityType.Eye_Strain, 2f, ItemType.camera, "Eye Strain", 0.8f)},
		{AbilityType.Roll_Eyes, new Ability(AbilityType.Roll_Eyes, 2f, ItemType.camera, "Roll Eyes", 0.8f)},
		{AbilityType.Smirk, new Ability(AbilityType.Smirk, 1f, ItemType.camera, "Smirk", 0.8f)},
		{AbilityType.Run, new Ability(AbilityType.Run, 0f, ItemType.camera, "Run", 0.8f)}
	};

	public AbilityType type;
	public float damage;
	public ItemType element;
	public string name;
	public Action abilityAction;
	public float chance;

	public Ability(AbilityType type, float damage, ItemType element, string name, float chance)
	{
		this.type = type;
		this.damage = damage;
		this.element = element;
		this.name = name;
		this.chance = chance;
		//list.Add (type, this);
	}

}
