using UnityEngine;
using System.Collections;

public class OptionMenuController : MonoBehaviour {

	public static bool isMenuOpen = false;

	public GameObject menuObject;

	// Use this for initialization
	void Start () {
	
	}

	void Update () 
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (isMenuOpen)
				CloseOptionMenu ();
			else
				OpenOptionMenu ();
		}
	}

	public void OpenOptionMenu()
	{
		isMenuOpen = true;
		menuObject.SetActive (true);
	}

	public void CloseOptionMenu()
	{
		menuObject.SetActive (false);
		isMenuOpen = false;
	}

}
