using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	private AudioSource musicSource;
	private AudioSource soundsSource;
	private DataController dataController;

	void Start() {

		Screen.orientation = ScreenOrientation.Portrait;

		// for music
		musicSource = GameObject.Find("MenuAudio").GetComponent<AudioSource>();
		// play music, making sure not to destroy between loads
		if (!musicSource.isPlaying) {
			musicSource.volume = 1f;  // 0.5f; // set to half volume, nice for slider functionality
			musicSource.Play(0);
		}  // also, Audio Source will Loop
		DontDestroyOnLoad(musicSource.gameObject);

		soundsSource = GameObject.Find("SoundsAudio").GetComponent<AudioSource>();
		soundsSource.volume = 1f;
		DontDestroyOnLoad(soundsSource.gameObject);


		dataController = GameObject.Find("DataController").GetComponent<DataController>();
	}

	// play game
	public void PlayGame() {
		// load game scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		// load saved data
		dataController.LoadGameData(true);
	}

	public void StartNewGame() {
		// load game scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		// load initial data (new game)
		dataController.LoadGameData(false);
	}

	// quit application
	public void QuitGame() {
		Debug.Log("QUIT GAME");
		Application.Quit();
	}

}
