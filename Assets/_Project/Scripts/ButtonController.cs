using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

	Ability ability;
	TMP_Text text;
	FightController fightController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitButton(Ability ability, FightController fightController)
	{
		this.ability = ability;
		this.fightController = fightController;

		text = GetComponentInChildren<TMP_Text> ();
		text.text = ability.name;

		Button button = GetComponent<Button> ();
		button.onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		fightController.UseAbility (ability);
	}
}
