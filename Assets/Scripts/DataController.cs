using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


// TODO music on/off when loading saved data
// TODO REMOVE 
	//persistentPath in my case, is at /Users/thordis/Library/Application Support/thordisCompany/Honeycomb/

public class DataController : MonoBehaviour {

	//private string jsonFolder = "JSONdata/";
	private string initDataLocation = "JSONdata/initdata";
	//private string initDataFile = "initdata.json";
	private string savedDataFile = "saveddata.json";
	
	// for static honey data
	[HideInInspector] public Dictionary<string, HoneyProperties> honeyDict;

	private string filePath;				//
	private GameData loadedData;			//
	private bool dataLoaded;				//
	private HoneyScore honeyScore;			//
	private HoneyCurrency honeyCurrency;	// 
	private AudioSource musicSource;		// music playing in the background

	// Use this for initialization
	void Start () {
		//print(Application.persistentDataPath);

		DontDestroyOnLoad(gameObject);
		// so multiple objects wont be created, delete newest
		if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }

		// TODO get music and sound
		musicSource = GameObject.Find("MenuAudio").GetComponent<AudioSource>();

		// get JSON information on honey types
		TextAsset file = Resources.Load("JSONdata/honeyinfo") as TextAsset;  // note: don't use .json extension
		string honeyInfoAsJson = file.ToString();
		HoneyCombInfo tempjson = JsonUtility.FromJson<HoneyCombInfo>(honeyInfoAsJson);
		// create dictionary so it will be simpler to access info on honey types
		honeyDict = new Dictionary<string, HoneyProperties>();
		for(int j = 0; j < tempjson.honeyProperties.Length; j++) {
			// key is name of fullSprite, e.g. time is stored in honeyDict["hexhoneyOrange"].time
			honeyDict.Add(tempjson.honeyProperties[j].fullSprite, tempjson.honeyProperties[j]);
		}

		// set saved data filePath
		filePath = Application.persistentDataPath + "/" + savedDataFile;
	}

	// TODO on scene load ?
	void Update() {
		// done here and not in LoadGameData since the scene name must first be loaded (.....)
		if (!dataLoaded && loadedData != null && SceneManager.GetActiveScene().name == "GameScene") {
			dataLoaded = true;  // so this wont be repeated
			// then load from file
			
			// get the objects that data will be loaded into
			//GameObject[] unitObjects = GameObject.FindGameObjectsWithTag("honeyUnit");
			honeyScore = GameObject.Find("HoneyScore").GetComponent<HoneyScore>();
			honeyCurrency = GameObject.Find("HoneyCurrency").GetComponent<HoneyCurrency>();

			for (int i = 0; i < loadedData.units.Length; i++) {
				// get honey unit and its name
				GameObject temp = GameObject.Find(loadedData.units[i].name);
				HoneyHandler honeyHandler = temp.GetComponent<HoneyHandler>();
				string currName = loadedData.units[i].honeyFullImage;
				// fill the unit's HoneyHandler with data from honeyDict about the honey type
				honeyHandler.fillTime = honeyDict[currName].time;
				honeyHandler.points = honeyDict[currName].points;
				honeyHandler.price = honeyDict[currName].price;
				honeyHandler.honeyEmpty.sprite = Resources.Load<Sprite>("Sprites/" + honeyDict[currName].emptySprite);
				honeyHandler.honeyFull.sprite = Resources.Load<Sprite>("Sprites/" + honeyDict[currName].fullSprite);
				honeyHandler.isActivated = loadedData.units[i].isActivated;

				if (!honeyHandler.isActivated) {
					// change sprite to the empty one and deactivate the gameobject
					honeyHandler.honeyEmpty.sprite = Resources.Load<Sprite>("Sprites/hexhoneyADD");
					temp.SetActive(false);
				}
				honeyHandler.honeyEmpty.enabled = true;
				honeyHandler.honeyFull.enabled = false;
			}
			honeyScore.harvestedHoney = loadedData.honeyScore;
			honeyCurrency.currency = loadedData.honeyCurrency;
			//musicSource.volume = loadedData.musicOn; // TODO won't work since data is loaded after music
		}
	}
	
	// called when GameScene is loaded
	public void LoadGameData(bool loadFromSave) {
		// assume using saved data

		if (!File.Exists(filePath)) {
			// if save file has not yet been created on the device, create it
			// load initial data as string
			TextAsset file = Resources.Load(initDataLocation) as TextAsset;  // note: don't use .json extension
			string contentsAsJson = file.ToString(); // the full text of the asset as a single string
			// create a save file at the path and write this initial data to it
			File.WriteAllText(filePath, contentsAsJson);

			// load data into loadedData
			// pass the json to JsonUtility, and tell it to create a GameData object from it
			loadedData = JsonUtility.FromJson<GameData>(contentsAsJson);
			dataLoaded = false;
			
			Debug.Log("Files created at persitentPath.");
			return;
		}

		if (loadFromSave) {
			// read the json from the file into a string, then load it into loadedData as GameData
			string dataAsJson = File.ReadAllText(filePath); 
			// pass the json to JsonUtility, and tell it to create a GameData object from it
			loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
			dataLoaded = false;
		}
		else {
			// load initial data, write it to saved file and load it into loadedData
			TextAsset file = Resources.Load(initDataLocation) as TextAsset; 
			string contentsAsJson = file.ToString();
			File.WriteAllText(filePath, contentsAsJson);
			loadedData = JsonUtility.FromJson<GameData>(contentsAsJson);
			dataLoaded = false;
		}
	}

	public void SaveData(string name) {
		// change loaded data
		loadedData.honeyScore = honeyScore.harvestedHoney;
		loadedData.honeyCurrency = honeyCurrency.currency;
		//loadedData.musicOn = musicSource.volume;
		loadedData.musicOn = false;
		if (musicSource.volume != 0) loadedData.musicOn = true;

		for (int i = 0; i < loadedData.units.Length; i++) {
			if (loadedData.units[i].name == name) {
				// called from HoneyHandler, so only change data for unit with name given as parameter
				GameObject temp = GameObject.Find(loadedData.units[i].name);
				HoneyHandler honeyHandler = temp.GetComponent<HoneyHandler>();

				loadedData.units[i].honeyFullImage = honeyHandler.honeyFull.sprite.name;
				loadedData.units[i].isActivated = honeyHandler.isActivated;
			}
		}
		// write loaded data to file
		string dataAsJson = JsonUtility.ToJson(loadedData);
		System.IO.File.WriteAllText(Application.persistentDataPath + "/" + savedDataFile, dataAsJson);
		//Debug.Log("DATA SAVED");
	}
}

// different types of honey, by color and flower
/*private string normalHoney = "honeyFullImage";  // yellow
private string poppyHoney = "";  				// red
private string mintHoney = "";  				// green ? although the flower is purple
private string forgetmenotHoney = "";  			// blue
private string blackHoney = "";  				// the bad honey ?
private string whitecloverHoney = "";  			// white
private string lavenderHoney = "";  			// purple
private string lilacsHoney = "";  				// light purple, some other color ?
private string sedumHoney = "";  				// pink
private string marigoldHoney = "";*/ 			// orange