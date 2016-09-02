using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	private Texture2D fadeTexture;

	Color fadeColor;

	bool isFading = false;

	static Fader _instance;
	static Fader instance {
		get
		{
			if (_instance == null)
				_instance = new GameObject("_Fader").AddComponent<Fader>();
			return _instance;          
		}
	}

	/*void Start()
	{
		//Only to allow static methods to access this instance
		if (_instance == null)
			_instance = this;
	}*/

	void CreateTexture(Color color)
	{
		fadeTexture = new Texture2D(1, 1);
		fadeTexture.SetPixel(1, 1, color);
		fadeTexture.Apply();

	}
	public static void FadeIn(Color startColor, float duration)
	{ 
		instance.CreateTexture (startColor);
		instance.StartCoroutine(instance.FadeCoroutine(startColor, Color.clear, duration)); 
	}
	public static void FadeOut(Color endColor, float duration)
	{ 
		instance.CreateTexture (endColor);
		instance.StartCoroutine(instance.FadeCoroutine(Color.clear, endColor, duration)); 
	}
	public static void SetToClear()
	{
		instance.isFading = false;
	}

	IEnumerator FadeCoroutine(Color startColor, Color endColor, float duration)
	{
		isFading = true;
		fadeColor = startColor;

		float startTime = Time.time;
		float endTime = startTime + duration;

		while (Time.time < endTime)
		{ 
			yield return null; 

			float t = Mathf.InverseLerp (startTime, endTime, Time.time);
			fadeColor = Color.Lerp (startColor, endColor, t);
		}

		fadeColor = endColor;

		yield return null;

		isFading = false;
	}

	void OnGUI()
	{
		if (! isFading)
			return;
		
		GUI.color = fadeColor;
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), fadeTexture);
		GUI.color = Color.white;
	}
}
