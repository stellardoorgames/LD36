using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NodeCanvas.Tasks.Actions;

public enum EscBehavior
{
	Nothing,
	ExitScene,
	ExitGame,
	OptionMenuOpen,
	OptionMenuToggle
}

public class LevelChanger : MonoBehaviour {

	public string levelName;

	public bool fadeIn = true;
	//public Color fadeInColor = Color.black;
	//public Color fadeOutColor = Color.black;
	public float fadeInDuration = 1f;
	public float fadeOutDuration = 1f;

	public bool fadeOutMusic = false;
	AudioSource music = null;

	public EscBehavior escapeBehavior;

	public GameObject optionMenu;

	static LevelChanger instance;

	void Start()
	{
		instance = this;

		if (optionMenu != null)
			optionMenu.SetActive (false);

		if (fadeIn)
		{
			CameraFader.current.FadeIn (fadeInDuration);
		}

		if (fadeOutMusic)
			music = GetComponent<AudioSource> ();
	}

	void Update()
	{
		if (Input.GetButton ("Cancel"))
		{
			
			if (escapeBehavior == EscBehavior.ExitGame)
				Application.Quit ();
			else if (escapeBehavior == EscBehavior.ExitScene && levelName != "")
				ChangeLevel ();
			else if (optionMenu != null)
			{
				if (optionMenu.activeSelf == false && (escapeBehavior == EscBehavior.OptionMenuOpen || escapeBehavior == EscBehavior.OptionMenuToggle))
				{
					optionMenu.SetActive (true);
				}
				else if (escapeBehavior == EscBehavior.OptionMenuToggle)
				{
					optionMenu.SetActive(false);
				}
			}

		}
			
	}

	public static void ChangeScene(string levelName = "")
	{
		instance.ChangeLevel (levelName);
	}

	public void ChangeLevel(string levelName = "")
	{
		StartCoroutine (ChangeLevelCoroutine (levelName));
	}

	public IEnumerator ChangeLevelCoroutine(string levelName = "")
	{
		Debug.Log (levelName);

		float startTime = Time.time;
		float endTime = startTime + fadeOutDuration;

		CameraFader.current.FadeOut(fadeOutDuration);

		float startingVolume = 1f;
		if (music != null)
			startingVolume = music.volume;
		

		while (Time.time < endTime)
		{
			if (music != null)
			{
				float t = Mathf.InverseLerp (startTime, endTime, Time.time);
				music.volume = Mathf.Lerp (startingVolume, 0f, t);
			}
			//TODO my own camera fader
			yield return null;
		}

		if (levelName == "")
			levelName = this.levelName;


		SceneManager.LoadScene (levelName);
	}


}
