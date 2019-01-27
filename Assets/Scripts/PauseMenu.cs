using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour {

	// for music
	private AudioSource musicSource;
	/*private Slider musicSlider;
	public GameObject musicOnText;
	public GameObject musicOffText;*/
	private Scrollbar musicScrollbar;
	private Image musicScrollbarBackground;

	// for sounds
	private AudioSource soundsSource;
	/*private Slider soundsSlider;
	public GameObject soundsOnText;
	public GameObject soundsOffText;*/
	private Scrollbar soundsScrollbar;
	private Image soundsScrollbarBackground;

	// colors for scrollbar background
	private Color onColor = new Color(0.9176f,0.5215f,0.1333f,1f);  // orangeish
	private Color offColor = new Color(0.9921f,0.8588f,0.1921f,1f);  // yellowish

	void Start() {
		musicSource = GameObject.Find("MenuAudio").GetComponent<AudioSource>();
		soundsSource = GameObject.Find("SoundsAudio").GetComponent<AudioSource>();

		// music scrollbar
		musicScrollbar = GameObject.Find("MusicScrollbar").GetComponent<Scrollbar>();
		musicScrollbarBackground = GameObject.Find("MusicScrollbar").GetComponent<Image>();
		if (musicSource.volume == 0) {
			// music is off and background is yellowish
			musicScrollbar.value = 0;
			musicScrollbarBackground.color = offColor;
		} 
		else {
			// music is on and background is brownish
			musicScrollbar.value = 1;
			musicScrollbarBackground.color = onColor;
		}
		// sounds scrollbar
		soundsScrollbar = GameObject.Find("SoundsScrollbar").GetComponent<Scrollbar>();
		soundsScrollbarBackground = GameObject.Find("SoundsScrollbar").GetComponent<Image>();
		if (soundsSource.volume == 0) {
			// sounds is off and background is yellowish
			soundsScrollbar.value = 0;
			soundsScrollbarBackground.color = offColor;
		} 
		else {
			// sounds is on and background is brownish
			soundsScrollbar.value = 1;
			soundsScrollbarBackground.color = onColor;
		}

		// for music
		/*musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
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
		}*/

		// for sounds
		/*soundsSlider = GameObject.Find("SoundsSlider").GetComponent<Slider>();
		soundsSlider.value = soundsSource.volume;
		if (soundsSlider.value != 0) {
			soundsOffText.SetActive(false);
			soundsOnText.SetActive(true);
			soundsSlider.value = 1;
		}
		else {
			soundsOffText.SetActive(true);
			soundsOnText.SetActive(false);
			soundsSlider.value = 0;
		}*/
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

	// for scrollbar

	public void onMusicScrollbarChange() {
		musicSource.volume = musicScrollbar.value;
		if (musicSource.volume == 0) {
			// music is off and background is yellowish
			musicScrollbar.value = 0;
			musicScrollbarBackground.color = offColor;
		} 
		else {
			// music is on and background is brownish
			musicScrollbar.value = 1;
			musicScrollbarBackground.color = onColor;
		}
	}

	public void onSoundsScrollbarChange() {
		soundsSource.volume = soundsScrollbar.value;
		if (soundsSource.volume == 0) {
			// sounds is off and background is yellowish
			soundsScrollbar.value = 0;
			soundsScrollbarBackground.color = offColor;
		} 
		else {
			// sounds is on and background is brownish
			soundsScrollbar.value = 1;
			soundsScrollbarBackground.color = onColor;
		}
	}

	// for slider
	
	/*public void onMusicVolumeChange() {
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
		//Debug.Log("NOT IMPLEMENTED: sounds volume");
		// TODO
		soundsSource.volume = soundsSlider.value;
		if (soundsSlider.value != 0) {
			soundsOffText.SetActive(false);
			soundsOnText.SetActive(true);
			soundsSlider.value = 1;
		}
		else {
			soundsOffText.SetActive(true);
			soundsOnText.SetActive(false);
			soundsSlider.value = 0;
		}

	}*/
}
