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
		{AbilityType.Eye_Strain, new Ability(AbilityType.Eye_Strain, 8f, AbilityElement.Paper, "Eye Strain")},
		{AbilityType.Roll_Eyes, new Ability(AbilityType.Roll_Eyes, 8f, AbilityElement.Paper, "Roll Eyes")},
		{AbilityType.Smirk, new Ability(AbilityType.Smirk, 8f, AbilityElement.Paper, "Smirk")},
		{AbilityType.Run, new Ability(AbilityType.Run, 8f, AbilityElement.Paper, "Run")}
	};

	public AbilityType type;
	public float damage;
	public AbilityElement element;
	public string name;
	public Action abilityAction;

	public Ability(AbilityType type, float damage, AbilityElement element, string name)
	{
		this.type = type;
		this.damage = damage;
		this.element = element;
		this.name = name;
		//list.Add (type, this);
	}

}
