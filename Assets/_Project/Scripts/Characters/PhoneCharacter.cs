using UnityEngine;
using System.Collections;

public class PhoneCharacter : Character {

	protected override void Start ()
	{
		base.Start ();

		AddAbility (AbilityType.Roll_Eyes);
		AddAbility (AbilityType.Smirk);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
