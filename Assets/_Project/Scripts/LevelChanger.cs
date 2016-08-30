using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NodeCanvas.Tasks.Actions;

public class LevelChanger : MonoBehaviour {

	public string levelName;

	public bool fadeIn = true;
	//public Color fadeInColor = Color.black;
	//public Color fadeOutColor = Color.black;
	public float fadeInDuration = 1f;
	public float fadeOutDuration = 1f;

	public bool fadeOutAudio = false;
	AudioSource audio = null;

	public bool exitOnEsc = false;

	static LevelChanger instance;

	void Start()
	{
		instance = this;

		if (fadeIn)
		{
			CameraFader.current.FadeIn (fadeInDuration);
		}

		if (fadeOutAudio)
			audio = GetComponent<AudioSource> ();
	}

	void Update()
	{
		if (exitOnEsc && Input.GetButton ("Cancel"))
			ChangeLevel ();
			
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
		if (audio != null)
			startingVolume = audio.volume;
		

		while (Time.time < endTime)
		{
			if (audio != null)
			{
				float t = Mathf.InverseLerp (startTime, endTime, Time.time);
				audio.volume = Mathf.Lerp (startingVolume, 0f, t);
			}
			//TODO my own camera fader
			yield return null;
		}

		if (levelName == "")
			levelName = this.levelName;


		SceneManager.LoadScene (levelName);
	}


}
