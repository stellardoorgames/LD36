using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
		{AbilityType.Eye_Strain, new Ability(AbilityType.Eye_Strain, 8f, ItemType.Camera, "Eye Strain", 0.8f)},
		{AbilityType.Roll_Eyes, new Ability(AbilityType.Roll_Eyes, 8f, ItemType.Camera, "Roll Eyes", 0.8f)},
		{AbilityType.Smirk, new Ability(AbilityType.Smirk, 8f, ItemType.Camera, "Smirk", 0.8f)},
		{AbilityType.Run, new Ability(AbilityType.Run, 8f, ItemType.Camera, "Run", 0.8f)}
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
