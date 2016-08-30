using UnityEngine;
using System.Collections;

public class LastBossTrigger : MonoBehaviour {

	public PlayerCharacter player;

	public GameObject marker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.items.Count >= 5)
			marker.SetActive (true);
	}
}
