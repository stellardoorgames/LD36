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

	Character player;
	Character enemy;

	Animator anim;



	// Use this for initialization
	void Start () {
		menuObject.SetActive (false);
		anim = GetComponent<Animator> ();

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
		player = characterA;
		enemy = characterB;

		enemyImage.sprite = enemy.texture;

		foreach(Ability ability in characterA.abilities)
		{
			GameObject go = Instantiate (buttonPrefab);
			go.transform.SetParent (menuObject.transform);
			ButtonController bc = go.GetComponent<ButtonController> ();
			bc.InitButton (ability, this);
		}

		textObject.text = string.Format ("A shiney new {0} appears!", enemy.characterName);

		AudioSource.PlayClipAtPoint (OpenSound, transform.position);

		CameraFader.current.FadeIn(fadeInTime);

		yield return new WaitForSeconds(2.5f);

		textObject.text = "";

		menuObject.SetActive (true);
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
/*
	public void OnRun()
	{
		
	}
*/
	public void EndFight()
	{
		StartCoroutine (EndFightCoroutine ());
	}

	public IEnumerator EndFightCoroutine()
	{
		CameraFader.current.FadeOut (fadeOutTime);

		yield return new WaitForSeconds (fadeOutTime);

		CameraFader.current.FadeIn (0f);

		gameObject.SetActive (false);
	}

	void DisplayItems(Character character)
	{
		
	}
}
