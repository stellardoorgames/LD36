using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemType
{
	camera,
	printer,
	reciever,
	screen,
	speakers
}

public class Item : MonoBehaviour {

	public ItemType type;

	public GameObject prefabObject;


	//public List<AbilityType> abilities;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
