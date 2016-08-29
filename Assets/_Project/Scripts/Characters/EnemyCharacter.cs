using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCharacter : Character {

	public GameObject itemPrefab;

	public string introTextA;

	protected override void Start ()
	{
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Ability GetRandomAbility()
	{
		Ability[] list = new Ability[abilities.Count];
		abilities.CopyTo (list);

		return list [Random.Range (0, list.Length - 1)];
	}

	public override void Die ()
	{
		if (itemPrefab != null)
			Instantiate (itemPrefab, transform.position, Quaternion.identity);

		GameObject.Destroy (gameObject);
	}
}
