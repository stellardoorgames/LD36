using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {

	ItemType type;
	ReplyData reply;

	TMP_Text text;
	FightController fightController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitButton(string label, ItemType type, FightController fightController)
	{
		this.type = type;
		this.fightController = fightController;

		text = GetComponentInChildren<TMP_Text> ();
		text.text = label;

		Button button = GetComponent<Button> ();
		button.onClick.AddListener(OnClickAbility);
	}

	public void InitButton(string label, ReplyData reply, FightController fightController)
	{
		this.reply = reply;
		this.fightController = fightController;

		text = GetComponentInChildren<TMP_Text> ();
		text.text = label;
		text.fontSize = 12f;

		Button button = GetComponent<Button> ();
		button.onClick.AddListener(OnClickTalk);
	}

	public void OnClickAbility()
	{
		fightController.UseAbility (type);
	}

	public void OnClickTalk()
	{
		fightController.UseTalk (reply);
	}
}
