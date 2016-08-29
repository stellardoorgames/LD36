using UnityEngine;
using System.Collections;
using TMPro;
using NodeCanvas.Tasks.Actions;
using UnityEngine.UI;

public class FightController : MonoBehaviour {

	public TMP_Text textObject;

	public GameObject menuObject;
	public GameObject buttonPrefab;
	public Image enemyImage;
	public Image background;
	public Color normalBackgroundColor = Color.white;
	public Color blackBackgroundColor = Color.black;
	public Color fodeOutBackgroundColor = Color.clear;
	public float fadeDuration = 1f;

	public Sprite normalFightBackground;
	public Sprite postFightBackground;
	public Sprite finalBossBackground;

	public GameObject cam;
	public GameObject antenne;
	public GameObject speakers;
	public GameObject magnifier;
	public GameObject printer;

	public AudioClip OpenSound;
	public AudioClip SelectSound;
	public AudioClip AttackSound;
	public AudioClip MissSound;
	public AudioClip victorySound;
	public AudioClip defeatSound;

	public float fadeInTime = 0.5f;
	public float fadeOutTime = 0.5f;
	public float messageTime = 2.5f;

	public bool isHit = false;

	public GameObject canvas;

	PlayerCharacter player;
	EnemyCharacter enemy;

	Animator anim;

	public bool isFighting = false;

	// Use this for initialization
	void Start () {
		menuObject.SetActive (false);
		anim = GetComponent<Animator> ();
		canvas.SetActive (false);
		background.sprite = normalFightBackground;
		normalBackgroundColor = background.color;
	}
	
	// Update is called once per frame
	void Update () {
	
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
		
		player = characterA;
		enemy = characterB;

		DisplayItems (player);

		enemyImage.sprite = enemy.texture;

		foreach(Ability ability in player.abilities)
		{
			Debug.Log (ability.type);
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (menuObject.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (ability, this);
		}

		textObject.text = string.Format ("A {0} appears!", enemy.characterName);

		AudioSource.PlayClipAtPoint (OpenSound, transform.position);

		CameraFader.current.FadeIn(fadeInTime);

		yield return new WaitForSeconds (1f);
		
		//textObject.text += "\n\n" + enemy.introTextA;
		textObject.text += "\n\n\"" + enemy.introTextA + "\"";
		
		yield return new WaitForSeconds(3f);

		FightOptions ();
	}

	public void FightOptions()
	{
		menuObject.SetActive (true);
		textObject.text = "";
	}

	public void UseAbility(Ability ability)
	{
		StartCoroutine (UseAbilityCoroutine (ability));
	}

	public IEnumerator UseAbilityCoroutine(Ability ability)
	{
		AudioSource.PlayClipAtPoint (SelectSound, transform.position);

		menuObject.SetActive (false);

		if (ability.type == AbilityType.Run)
		{
			textObject.text = string.Format ("{0} ran away!", player.characterName);

			EndFight ();
		}
		else
		{
			isHit = enemy.TakeAttack (ability);
			anim.SetBool ("IsHit", isHit);
			Debug.Log (isHit ? "hit" : "miss");

			string attackText = string.Format ("{0} used {1}.", player.characterName, ability.name);

			if (!isHit)
			{
				attackText += "\n\nIt missed!";
			}
			else 
			{
				attackText += "\n\nIt hit!";
			}

			textObject.text = attackText;
			anim.SetTrigger ("Attack1");

			yield return new WaitForSeconds(messageTime);

			if (enemy.life > 0f)
			{
				EnemyTurn ();
			}
			else
			{
				Victory ();
			}
			
		}
	}

	void EnemyTurn()
	{
		StartCoroutine (EnemyTurnCoroutine ());
	}

	IEnumerator EnemyTurnCoroutine()
	{
		Ability enemyAbility = enemy.GetRandomAbility ();

		isHit = player.TakeAttack (enemyAbility);
		anim.SetBool ("IsHit", isHit);

		anim.SetTrigger ("EnemyAttack1");

		string attackText = string.Format ("{0} used {1}!", enemy.name, enemyAbility.name);

		if (!isHit)
		{
			attackText += "\n\nIt missed!";
		}
		else
		{
			attackText += "\n\nIt hit!";

		}

		textObject.text = attackText;

		yield return new WaitForSeconds(messageTime);


		if (player.life > 0f)
		{
			FightOptions ();
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

		string endingText = string.Format ("{0} was defeated!\n\nYou wins!", enemy.name);

		StartCoroutine (PlayEnding (endingText));

		enemy.Die ();
	}

	void Defeat()
	{
		string endingText = string.Format ("You lost.\n\n{0} wins.", enemy.name);

		StartCoroutine (PlayEnding (endingText));
	}

	IEnumerator PlayEnding(string endingText)
	{
		float startTime = Time.time;
		float endTime = startTime + fadeDuration;
		while (Time.time < endTime)
		{
			float t = Mathf.InverseLerp (startTime, endTime, Time.time);
			background.color = Color.Lerp (normalBackgroundColor, blackBackgroundColor, t);
			yield return null;
		}

		textObject.text = endingText;

		yield return new WaitForSeconds (messageTime);

		EndFight();
	}

	public void EndFight()
	{
		StartCoroutine (EndFightCoroutine ());
	}

	public IEnumerator EndFightCoroutine()
	{
		//Destroy Buttons
		ButtonController[] buttons = menuObject.GetComponentsInChildren<ButtonController> ();
		for (int i = 0; i < buttons.Length; i++)
			GameObject.Destroy (buttons [i].gameObject);

		//Fade out
		CameraFader.current.FadeOut (fadeOutTime);
		yield return new WaitForSeconds (fadeOutTime);
		//Reset camera
		CameraFader.current.FadeIn (0f);

		//Turn off
		canvas.SetActive (false);
		isFighting = false;

		background.color = normalBackgroundColor;
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
