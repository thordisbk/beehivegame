using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// TODO sounds

public class PauseMenu : MonoBehaviour {

	// for music
	private Slider musicSlider;
	private AudioSource musicSource;
	public GameObject musicOnText;
	public GameObject musicOffText;

	// for sounds
	private Slider soundsSlider;
	public GameObject soundsOnText;
	public GameObject soundsOffText;

	void Start() {
		// for music
		musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
		musicSource = GameObject.Find("MenuAudio").GetComponent<AudioSource>();
		musicSlider.value = musicSource.volume;
		if (musicSource.volume != 0) {
			musicOffText.SetActive(false);
			musicOnText.SetActive(true);
			musicSlider.value = 1;
		}
		else {
			musicOffText.SetActive(true);
			musicOnText.SetActive(false);
			musicSlider.value = 0;
		}

		// for sounds
		soundsSlider = GameObject.Find("SoundsSlider").GetComponent<Slider>();
		if (soundsSlider.value != 0) {
			soundsOffText.SetActive(false);
			soundsOnText.SetActive(true);
		}
		else {
			soundsOffText.SetActive(true);
			soundsOnText.SetActive(false);
		}


	}

	void OnEnable() {
		// stop time when pause is enabled
		Time.timeScale = 0.0001f; //1.0f;
		//musicSource.Stop();
	}

	public void ResumeGame() {
		// make time run as before
		Time.timeScale = 1.0f;
	}

	// quit current game and go to main menu
	public void QuitGame() {
		//Debug.Log("QUIT TO MAIN MENU");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		Time.timeScale = 1.0f;
	}
	
	public void onMusicVolumeChange() {
		//Debug.Log("changing volume of music");
		musicSource.volume = musicSlider.value;
		// slider value can be either 0 or 1
		if (musicSource.volume != 0) {
			musicOffText.SetActive(false);
			musicOnText.SetActive(true);
			musicSlider.value = 1;
		}
		else {
			musicOffText.SetActive(true);
			musicOnText.SetActive(false);
			musicSlider.value = 0;
		}
	}
	
	public void onSoundsVolumeChange() {
		Debug.Log("NOT IMPLEMENTED: sounds volume");
		// TODO
		if (soundsSlider.value != 0) {
			soundsOffText.SetActive(false);
			soundsOnText.SetActive(true);
		}
		else {
			soundsOffText.SetActive(true);
			soundsOnText.SetActive(false);
		}

	}
}
