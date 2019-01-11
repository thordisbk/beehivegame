using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    
	// for music
	private Slider musicSlider;
	private AudioSource musicSource;
	public GameObject musicOnText;
	public GameObject musicOffText;

	// for sounds
	private Slider soundsSlider;
	public GameObject soundsOnText;
	public GameObject soundsOffText;
	//private AudioSource soundsSource;

    void Start() {
		// for music
		musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
		musicSource = GameObject.Find("MenuAudio").GetComponent<AudioSource>();
		musicSource.volume = 1;
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

	/* 	// for longer slider:

	public void onMusicVolumeChange() {
		//Debug.Log("changing volume of music");
		musicSource.volume = volumeSlider.value;
	}

	public void onSoundsVolumeChange() {
		Debug.Log("NOT IMPLEMENTED: sounds volume");
	}*/
}