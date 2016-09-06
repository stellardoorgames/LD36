using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityCommon;

public class FightController : MonoBehaviour {

	public float dialogDelay = 4f;

	//public Text textObject;
	public TextDisplay textDisplayObject;

	public GameObject fightMenuObject;
	public GameObject abilityMenuObject;
	public GameObject talkMenuObject;
	public GameObject talkMenuButtonsObject;
	//public Text talkMenuText;
	public TextDisplay talkMenuTextDisplay;
	public GameObject buttonPrefab;
	public Image enemyImage;
	public Image background;
	public Color normalBackgroundColor = Color.white;
	public Color blackBackgroundColor = Color.black;
	//public Color fodeOutBackgroundColor = Color.clear;
	public float fadeDuration = 1f;

	public Text egoText;
	public Text egoEnemyText;

	public Sprite normalFightBackground;
	//public Sprite postFightBackground;
	public Sprite finalBossBackground;

	public GameObject cam;
	public GameObject antenne;
	public GameObject speakers;
	public GameObject magnifier;
	public GameObject printer;

	public AudioClip OpenSound;
	public AudioClip SelectSound;
	public AudioClip attackSound;
	public AudioClip missSound;
	public AudioClip victorySound;
	public AudioClip defeatSound;
	public AudioClip runSound;

	public AudioClip battleMusic;
	public AudioClip previousMusic;

	public AudioSource music;

	public float fadeInTime = 0.5f;
	public float fadeOutTime = 0.5f;
	public float messageTime = 2.5f;

	public bool isHit = false;

	public GameObject canvas;

	public SceneField endingScene;

	int talkTier = 0;

	PlayerCharacter player;
	EnemyCharacter enemy;

	Animator anim;

	public static bool isFighting = false;

	void Start () 
	{
		fightMenuObject.SetActive (false);
		abilityMenuObject.SetActive (false);
		talkMenuObject.SetActive (false);

		anim = GetComponent<Animator> ();
		canvas.SetActive (false);
		background.sprite = normalFightBackground;
		normalBackgroundColor = background.color;
	}
	
	void Update () 
	{
		if (Input.GetButtonDown("Cancel"))
		{
			DisplayFightMenu ();
		}
	}

	public void StartFight(PlayerCharacter characterA, EnemyCharacter characterB)
	{
		StartCoroutine (StartFightCoroutine (characterA, characterB));
	}

	public IEnumerator StartFightCoroutine(PlayerCharacter characterA, EnemyCharacter characterB)
	{
		if (isFighting)
			yield break;
		
		isFighting = true;
		canvas.SetActive (true);

		previousMusic = music.clip;
		music.clip = battleMusic;
		music.Play ();

		talkTier = 0;

		player = characterA;
		enemy = characterB;

		egoText.text = player.ego.ToString ();
		egoEnemyText.text = enemy.ego.ToString ();
		
		DisplayItems (player);

		enemyImage.sprite = enemy.texture;

		GameObject panel = abilityMenuObject.GetComponentInChildren<GridLayoutGroup> ().gameObject;

		foreach(ItemType ability in player.abilityList)
		{
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (panel.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (ability.ToString(), ability, this);
		}

		//textObject.text = string.Format ("You pick a fight with {0}!", enemy.characterName);
		textDisplayObject.SetText (string.Format ("You pick a fight with {0}!", enemy.characterName));

		if (OpenSound != null)
			AudioSource.PlayClipAtPoint (OpenSound, Camera.main.transform.position);

		Fader.FadeIn(Color.black, fadeInTime);

		yield return WaitForInput ();//new WaitForSeconds (2f);

		yield return StartCoroutine (textDisplayObject.DisplayTextCoroutine (enemy.data.intros, enemy.color));

		DisplayFightMenu ();
	}

	public void DisplayFightMenu()
	{
		//talkMenuObject.SetActive (false);
		//abilityMenuObject.SetActive (false);
		fightMenuObject.SetActive (true);
		foreach (Transform t in talkMenuButtonsObject.transform)
			Destroy (t.gameObject);
		//textObject.text = "";
		textDisplayObject.ClearText ();
	}

	public void SelectAttack()
	{
		StartCoroutine (UseAttackCoroutine ());
	}

	public IEnumerator UseAttackCoroutine()
	{
		isHit = enemy.TakeAttack (1);
		anim.SetBool ("IsHit", isHit);
		anim.SetTrigger ("Attack1");

		string hitMiss;
		if (isHit)
		{
			hitMiss = "It hit!";
			AudioSource.PlayClipAtPoint (attackSound, Camera.main.transform.position);
		}
		else
		{
			hitMiss = "It missed!";
			AudioSource.PlayClipAtPoint (missSound, Camera.main.transform.position);
		}

		//textObject.text = string.Format ("{0} used {1}.\n{2}", player.characterName, "Attack", hitMiss);
		textDisplayObject.SetText (string.Format ("{0} used {1}.\n{2}", player.characterName, "Attack", hitMiss));
		
		egoEnemyText.text = enemy.ego.ToString ();
		
		yield return StartCoroutine (WaitForInput ());
		
		EndPlayerTurn ();
	}

	public void SelectTalk()
	{
		StartCoroutine (SelectTalkCoroutine ());
	}

	public IEnumerator SelectTalkCoroutine()
	{
		List<ReplyData> replies = new List<ReplyData> (enemy.data.conversations [talkTier].replies);

		replies.Shuffle ();

		//talkMenuText.text = "";
		talkMenuTextDisplay.ClearText ();
		talkMenuObject.SetActive (true);

		yield return StartCoroutine(talkMenuTextDisplay.DisplayTextCoroutine (enemy.data.conversations [talkTier].prompt, enemy.color));


		foreach(ReplyData reply in replies)
		{
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (talkMenuButtonsObject.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (reply.text, reply, this);
		}
	}

	public void UseTalk(ReplyData reply)
	{
		StartCoroutine (UseTalkCoroutine (reply));
	}

	public IEnumerator UseTalkCoroutine(ReplyData replyData)
	{
		talkMenuObject.SetActive (false);

		isHit = (replyData.damage > 1);
		enemy.TakeDamage (replyData.damage);

		anim.SetBool ("IsHit", isHit);
		anim.SetTrigger ("Attack1");

		egoEnemyText.text = enemy.ego.ToString ();

		if (isHit)
			AudioSource.PlayClipAtPoint (attackSound, Camera.main.transform.position);


		yield return StartCoroutine(textDisplayObject.DisplayTextCoroutine (replyData.followup, enemy.color));


		if (replyData.progress && talkTier < enemy.data.conversations.Length - 1)
			talkTier++;

		yield return StartCoroutine (WaitForInput ());

		EndPlayerTurn ();

	}

	public void UseAbility(ItemType ability)
	{
		StartCoroutine (UseAbilityCoroutine (ability));
	}

	public IEnumerator UseAbilityCoroutine(ItemType ability)
	{
		AudioSource.PlayClipAtPoint (SelectSound, Camera.main.transform.position);

		abilityMenuObject.SetActive (false);
		fightMenuObject.SetActive (false);
		
		//textObject.text = string.Format ("{0} used {1}.", player.characterName, ability.ToString());
		textDisplayObject.SetText (string.Format ("{0} used {1}.", player.characterName, ability.ToString ()));

		ItemConversationData reply = enemy.TakeAbility (ability);

		string[] replyText = new string[1] {". . ."};
		isHit = false;

		if (reply != null)
		{
			isHit = (reply.damage > 0);
			replyText = reply.replies;
		}

		anim.SetBool ("IsHit", isHit);
		anim.SetTrigger ("Attack1");
		egoEnemyText.text = enemy.ego.ToString ();
		

		yield return StartCoroutine (WaitForInput ());

		yield return StartCoroutine (textDisplayObject.DisplayTextCoroutine (replyText, enemy.color));

		EndPlayerTurn ();

	}

	public void SelectRun()
	{
		//textObject.text = string.Format ("{0} ran away!", player.characterName);
		textDisplayObject.SetText (string.Format ("{0} ran away!", player.characterName));

		AudioSource.PlayClipAtPoint (runSound, Camera.main.transform.position);

		EndFight ();
	}

	void EndPlayerTurn()
	{
		talkMenuObject.SetActive (false);

		if (enemy.ego > 0f)
		{
			EnemyTurn ();
		}
		else
		{
			Victory ();
		}
	}

	void EnemyTurn()
	{
		StartCoroutine (EnemyTurnCoroutine ());
	}

	IEnumerator EnemyTurnCoroutine()
	{
		textDisplayObject.ClearText ();
		yield return new WaitForSeconds (0.5f);

		AttackData enemyAttack = enemy.GetRandomAttack ();

		isHit = player.TakeAttack (enemyAttack.damage);

		egoText.text = player.ego.ToString ();
		
		anim.SetBool ("IsHit", isHit);
		anim.SetTrigger ("EnemyAttack1");

		string attackText;

		if (!isHit)
		{
			attackText = "It missed!";
			AudioSource.PlayClipAtPoint (missSound, Camera.main.transform.position);
		}
		else
		{
			attackText = "It hit!";
			AudioSource.PlayClipAtPoint (attackSound, Camera.main.transform.position);
		}

		//textObject.text = string.Format ("{0} used {1}!\n{2}", enemy.data.name, enemyAttack.name, attackText);
		textDisplayObject.SetText (string.Format ("{0} used {1}!\n{2}", enemy.data.name, enemyAttack.name, attackText));

		yield return null;
		yield return StartCoroutine (WaitForInput ());

		yield return null;
		yield return StartCoroutine (textDisplayObject.DisplayTextCoroutine (enemyAttack.dialog, enemy.color));


		if (player.ego > 0f)
		{
			DisplayFightMenu ();
		}
		else
		{
			player.Die ();
			Defeat ();
		}

	}

	void Victory()
	{
		EnemyTakeHit (true);

		//string endingText = string.Format ("{0} was defeated!\n\nYou win!", enemy.name);
		
		StartCoroutine (PlayEnding (enemy.data.win));

		enemy.Die ();
	}

	void Defeat()
	{
		StartCoroutine (PlayEnding (enemy.data.lose));
	}

	IEnumerator PlayEnding(string[] endingText)
	{
		float startTime = Time.time;
		float endTime = startTime + fadeDuration;
		while (Time.time < endTime)
		{
			float t = Mathf.InverseLerp (startTime, endTime, Time.time);
			background.color = Color.Lerp (normalBackgroundColor, blackBackgroundColor, t);
			yield return null;
		}


		yield return StartCoroutine (textDisplayObject.DisplayTextCoroutine (endingText, enemy.color));


		if (enemy.data.name == "Konia" && enemy.ego <= 0)
		{
			SceneController.ChangeScene (endingScene);
		}

		EndFight();
	}

	public void EndFight()
	{
		StartCoroutine (EndFightCoroutine ());
	}

	public IEnumerator EndFightCoroutine()
	{
		//Destroy Buttons
		ButtonController[] buttons = fightMenuObject.GetComponentsInChildren<ButtonController> ();
		for (int i = 0; i < buttons.Length; i++)
			GameObject.Destroy (buttons [i].gameObject);

		//Fade out
		Fader.FadeOut (Color.black, fadeOutTime);
		yield return new WaitForSeconds (fadeOutTime);//StartCoroutine (WaitForPress ());//

		player.ego = player.maxEgo;

		//Turn off
		fightMenuObject.SetActive (false);
		canvas.SetActive (false);
		isFighting = false;

		background.color = normalBackgroundColor;

		music.clip = previousMusic;
		music.Play ();
	}

	IEnumerator WaitForInput()
	{
		while(! Input.anyKeyDown)
		{
			yield return null;
		}
	}

	//Because unity animation events can't do bools for some reason!
	void EnemyTakeHit(int hit)
	{
		EnemyTakeHit (hit == 1);
	}
	void EnemyTakeHit(bool hit)
	{
		enemyImage.sprite = hit ? enemy.textureHurt : enemy.texture;
	}

	void DisplayItems(PlayerCharacter character)
	{
		antenne.SetActive (character.antenna.activeSelf);
		cam.SetActive (character.cam.activeSelf);
		magnifier.SetActive (character.magnifier.activeSelf);
		printer.SetActive (character.printer.activeSelf);
		speakers.SetActive (character.speakers.activeSelf);

	}


}
