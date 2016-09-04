using UnityEngine;
using System.Collections;

public class OptionMenuController : MonoBehaviour {

	public static bool isMenuOpen = false;
	// Use this for initialization
	void Start () {
	
	}

	void Update () 
	{
		
	}

	void OnEnable()
	{
		Debug.Log ("Pause");

		isMenuOpen = true;
		Time.timeScale = 0f;
	}

	void OnDisable()
	{
		Time.timeScale = 1f;
		isMenuOpen = false;

		Debug.Log ("Unpause");
	}
}
