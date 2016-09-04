using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityCommon;

public class FightController : MonoBehaviour {

	public float dialogDelay = 4f;

	public Text textObject;

	public GameObject fightMenuObject;
	public GameObject abilityMenuObject;
	public GameObject talkMenuObject;
	public GameObject talkMenuButtonsObject;
	public Text talkMenuText;
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

	public bool isFighting = false;

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
		Debug.Log (player.abilityList);
		foreach(ItemType ability in player.abilityList)
		{
			//Debug.Log (ability.type);
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (panel.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (ability.ToString(), ability, this);
		}

		textObject.text = string.Format ("You pick a fight with {0}!", enemy.characterName);

		AudioSource.PlayClipAtPoint (OpenSound, player.transform.position);

		Fader.FadeIn(Color.black, fadeInTime);

		yield return WaitForPress ();//new WaitForSeconds (2f);

		yield return StartCoroutine (DisplayMonologue (enemy.data.intros));

		DisplayFightMenu ();
	}

	IEnumerator WaitForPress(float maxTime = 0f)
	{
		float endTime = (maxTime > 0) ? Time.time + maxTime : float.MaxValue;

		while (Time.time < endTime)
		{
			if (Input.anyKeyDown)
				endTime = Time.time;

			yield return null;
		}
	}

	IEnumerator DisplayMonologue(string[] monologue)
	{
		foreach(string text in monologue)
		{
			textObject.text = text;// + "\n";

			//float endTime = Time.time + dialogDelay;

			yield return StartCoroutine (WaitForPress ());
			/*while (Time.time < endTime)
			{
				if (Input.anyKeyDown)
					endTime = Time.time;
				
				yield return null;
			}*/

		}
	}

	public void DisplayFightMenu()
	{
		//talkMenuObject.SetActive (false);
		//abilityMenuObject.SetActive (false);
		fightMenuObject.SetActive (true);
		foreach (Transform t in talkMenuButtonsObject.transform)
			Destroy (t.gameObject);
		textObject.text = "";
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

		//string reply = enemy.TakeAbility (ability);

		textObject.text = string.Format ("{0} used {1}.", player.characterName, "Attack");
		
		egoEnemyText.text = enemy.ego.ToString ();

		yield return StartCoroutine (WaitForPress (1f));//new WaitForSeconds (1f);

		if (isHit)
		{
			textObject.text += "\n\nIt hit!";
			AudioSource.PlayClipAtPoint (attackSound, player.transform.position);
		}
		else
		{
			textObject.text += "\n\nIt missed!";
			AudioSource.PlayClipAtPoint (missSound, player.transform.position);
		}

		EndPlayerTurn ();
		//StartCoroutine (EndPlayerTurn ());
	}

	public void SelectTalk()
	{
		List<ReplyData> replies = new List<ReplyData> (enemy.data.conversations [talkTier].replies);

		replies.Shuffle ();

		foreach(ReplyData reply in replies)
		{
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (talkMenuButtonsObject.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (reply.text, reply, this);
		}

		talkMenuText.text = enemy.data.conversations [talkTier].prompt;

		talkMenuObject.SetActive (true);
		//StartCoroutine (SelectTalkCoroutine());
	}

	/*public IEnumerator SelectTalkCoroutine()
	{
		


	}*/

	public void SelectRun()
	{
		textObject.text = string.Format ("{0} ran away!", player.characterName);

		EndFight ();
	}

	public void UseAbility(ItemType ability)
	{
		StartCoroutine (UseAbilityCoroutine (ability));
	}

	public IEnumerator UseAbilityCoroutine(ItemType ability)
	{
		AudioSource.PlayClipAtPoint (SelectSound, player.transform.position);

		abilityMenuObject.SetActive (false);
		fightMenuObject.SetActive (false);
		
		textObject.text = string.Format ("{0} used {1}.", player.characterName, ability.ToString());

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
		

		yield return StartCoroutine (WaitForPress (1f));//new WaitForSeconds (1f);

		yield return StartCoroutine(DisplayMonologue (replyText));

		//yield return new WaitForSeconds (dialogDelay);

		EndPlayerTurn ();
		//StartCoroutine (EndPlayerTurn ());

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
			AudioSource.PlayClipAtPoint (attackSound, player.transform.position);

		yield return StartCoroutine(DisplayMonologue (replyData.followup));

		if (replyData.progress && talkTier < enemy.data.conversations.Length - 1)
			talkTier++;
		
		EndPlayerTurn ();
		//StartCoroutine(EndPlayerTurn ());

	}

	void EndPlayerTurn()
	{
		//yield return new WaitForSeconds(messageTime);

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
		AttackData enemyAttack = enemy.GetRandomAttack ();

		isHit = player.TakeAttack (enemyAttack.damage);

		egoText.text = player.ego.ToString ();
		
		anim.SetBool ("IsHit", isHit);

		anim.SetTrigger ("EnemyAttack1");

		string attackText = string.Format ("{0} used {1}!", enemy.data.name, enemyAttack.name);

		if (!isHit)
		{
			attackText += "\n\nIt missed!";
			AudioSource.PlayClipAtPoint (missSound, player.transform.position);
		}
		else
		{
			attackText += "\n\nIt hit!";
			AudioSource.PlayClipAtPoint (attackSound, player.transform.position);
		}

		textObject.text = attackText;

		yield return StartCoroutine (WaitForPress ());//new WaitForSeconds(messageTime);

		textObject.text = "";

		yield return StartCoroutine (DisplayMonologue (enemyAttack.dialog));


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

		//StartCoroutine (DisplayMonologue (enemy.data.lose));

		enemy.Die ();
	}

	void Defeat()
	{
		//string endingText = string.Format ("You lost.\n\n{0} wins.", enemy.name);

		StartCoroutine (PlayEnding (enemy.data.lose));

		//StartCoroutine (DisplayMonologue (enemy.data.win));
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

		textObject.text = "";

		//yield return new WaitForSeconds (messageTime);
		yield return StartCoroutine(DisplayMonologue (endingText));


		if (enemy.data.name == "Konia" && enemy.ego <= 0)
		{
			//LevelChanger lc = GameObject.Find ("SceneController").GetComponent<LevelChanger> ();
			//lc.ChangeLevel ("EndScene01");
			SceneController.ChangeScene (endingScene);//("EndScene01");
		}
		//SceneManager.LoadScene ("EndScene01");
		

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
		//Reset camera
		//Fader.SetToClear ();
		//Fader.FadeIn (Color.black, 0f);

		player.ego = player.maxEgo;

		//Turn off
		fightMenuObject.SetActive (false);
		canvas.SetActive (false);
		isFighting = false;

		background.color = normalBackgroundColor;

		music.clip = previousMusic;
		music.Play ();
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
