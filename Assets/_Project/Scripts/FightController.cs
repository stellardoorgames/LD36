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

	public GameObject cam;
	public GameObject antenne;
	public GameObject speakers;
	public GameObject magnifier;
	public GameObject printer;

	public AudioClip OpenSound;
	public AudioClip SelectSound;
	public AudioClip AttackSound;
	public AudioClip MissSound;
	//AudioClip 
	public float fadeInTime = 0.5f;
	public float fadeOutTime = 0.5f;

	public bool isHit = false;

	public GameObject canvas;

	Character player;
	Character enemy;

	Animator anim;

	public bool isFighting = false;

	// Use this for initialization
	void Start () {
		menuObject.SetActive (false);
		anim = GetComponent<Animator> ();
		canvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartFight(Character characterA, Character characterB)
	{
		StartCoroutine (StartFightCoroutine (characterA, characterB));
	}

	public IEnumerator StartFightCoroutine(Character characterA, Character characterB)
	{
		if (isFighting)
			yield break;
		
		isFighting = true;
		canvas.SetActive (true);
		
		player = characterA;
		enemy = characterB;

		DisplayItems (player as PlayerCharacter);

		enemyImage.sprite = enemy.texture;

		foreach(Ability ability in player.abilities)
		{
			Debug.Log (ability.type);
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (menuObject.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (ability, this);
		}

		textObject.text = string.Format ("A shiney new {0} appears!", enemy.characterName);

		AudioSource.PlayClipAtPoint (OpenSound, transform.position);

		CameraFader.current.FadeIn(fadeInTime);

		yield return new WaitForSeconds(2.5f);

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
		menuObject.SetActive (false);

		isHit = (Random.value <= ability.chance);
		anim.SetBool ("IsHit", isHit);
		Debug.Log (isHit ? "hit" : "miss");

		if (ability.type == AbilityType.Run)
		{
			textObject.text = string.Format ("{0} ran away!", player.characterName);

			EndFight ();
		}
		else
		{
			textObject.text = string.Format ("{0} used {1}.", player.characterName, ability.name);
			anim.SetTrigger ("Attack1");

			yield return new WaitForSeconds(2.5f);

			FightOptions ();
			
		}
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
