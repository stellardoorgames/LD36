using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ImageFader : MonoBehaviour {

	public GameObject nextObject;
	public float nextImageTime = 3f;

	public float duration = 3f;
	public float fadeInDuration = 0.5f;
	public float fadeOutDuration = 0.5f;

	public bool start = false;

	public Color startColor = Color.clear;
	public Color displayColor = Color.white;
	public Color endColor = Color.clear;

	public UnityEvent OnEnd;

	Image image;

	void Start ()
	{
		if (start)
			StartImage ();
	}

	/*void OnEnable()
	{
		StartImage ();
	}*/

	public void StartImage()
	{
		image = GetComponent<Image> ();

		image.color = Color.clear;
		gameObject.SetActive (true);

		StartCoroutine (ImageCoroutine ());

		if (nextObject != null)
			StartCoroutine (NextImageCoroutine ());
	}

	IEnumerator ImageCoroutine()
	{
		float startTime = Time.time;
		float endTime = startTime + fadeInDuration;

		while (Time.time <= endTime)
		{
			yield return null;

			float t = Mathf.InverseLerp (startTime, endTime, Time.time);
			image.color = Color.Lerp (startColor, displayColor, t);

		}

		float waitTime = duration - fadeInDuration - fadeOutDuration;
		yield return new WaitForSeconds (waitTime);

		startTime = Time.time;
		endTime = startTime + fadeOutDuration;

		while (Time.time <= endTime)
		{
			yield return null;

			float t = Mathf.InverseLerp (startTime, endTime, Time.time);
			image.color = Color.Lerp (displayColor, endColor, t);

		}

		OnEnd.Invoke ();

		foreach (Transform t in transform)
			t.gameObject.SetActive (false);
		
	}

	IEnumerator NextImageCoroutine()
	{
		yield return new WaitForSeconds (nextImageTime);

		if (nextObject != null)
		{
			nextObject.SetActive (true);//.StartImage ();
			ImageFader img = nextObject.GetComponent<ImageFader> ();
			if (img != null)
				img.StartImage ();
		}

	}
}
