using UnityEngine;
using System.Collections;
using TMPro;
using NodeCanvas.Tasks.Actions;

public class FightController : MonoBehaviour {

	public TMP_Text textObject;

	public GameObject menuObject;
	public GameObject buttonPrefab;

	public GameObject camera;
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
		anim.SetTrigger ("Attack1");
		textObject.text = string.Format ("{0} used {1}.", player.characterName, ability.name);

		yield return new WaitForSeconds(2.5f);

		FightOptions ();
	}

	public void OnRun()
	{
		
	}

	public void EndFight()
	{
		gameObject.SetActive (false);
	}
}
