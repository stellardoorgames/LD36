using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
	Antenne,
	Camera,
	Magnifier,
	Printer,
	Speakers
}

public class Item : MonoBehaviour {

	public ItemType type;

	public List<AbilityType> abilities;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
