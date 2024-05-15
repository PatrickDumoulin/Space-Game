using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicPlayer : MonoBehaviour
{
	void Awake()
	{
		string currentScene = SceneManager.GetActiveScene().name;

		int numMusicPlayers = FindObjectsOfType<musicPlayer>().Length;

		if (numMusicPlayers > 1)
		{
			Destroy(gameObject);
		}
		else if (currentScene == "Level 1")
		{
			DontDestroyOnLoad(gameObject);
		}




	}
}
