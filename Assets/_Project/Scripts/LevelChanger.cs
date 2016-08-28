using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	public string levelName;

	public void ChangeLevel(string levelName = "")
	{
		if (levelName == "")
			levelName = this.levelName;
		
		SceneManager.LoadScene (levelName);
	}
}
