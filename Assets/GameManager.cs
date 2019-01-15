using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
	
	// for upgrade menu
	public GameObject foggyBackground;
	public Image foggyUpgradeType;
	public Button foggyUndoButton;

	// for ugrading
	[HideInInspector] public Sprite currentSprite; // the sprite of the honey color currently being used
	[HideInInspector] public Sprite currentTransparentSprite;  // the background honey color
	[HideInInspector] public int currentPrice;	 // how much the honey costs
	[HideInInspector] public float currentTime;	 // how long it takes for the honey to be ready
	[HideInInspector] public int currentPoints;  // how many points the honey gives
	[HideInInspector] public int undoPrice;	 	// how much is given back on undo

	//public GameObject honeyTypeMenu;  // used when setting to inactive in PurchaseColor()

	//private HoneyScore honeyScore;	// for decreasing honeyHarvested in HoneyScore when colors are bought
	private HoneyCurrency honeyCurrency;	// for decreasing totalPoints in HoneyCurrency when colors are bought

	// for information text
	private TextMeshProUGUI infoText;
	private float infoTextTime;

	private DataController dataController;  // for saving on undo

	// for undo button
	[HideInInspector] public HoneyHandler lastUnitUpgraded;
	[HideInInspector] public Sprite lastSprite;
	[HideInInspector] public Sprite lastTransparentSprite;
	[HideInInspector] public int lastPrice;
	[HideInInspector] public float lastTime;
	[HideInInspector] public int lastPoints;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;

		//foggyBackground = GameObject.Find("TypeChooseFog");  // only when private

		infoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
		infoText.text = "";
		infoText.enabled = false;
		infoTextTime = 0f;

		//honeyScore = GameObject.Find("HoneyScore").GetComponent<HoneyScore>();
		honeyCurrency = GameObject.Find("HoneyCurrency").GetComponent<HoneyCurrency>();

		dataController = GameObject.Find("DataController").GetComponent<DataController>();

		// TODO set prices for honeytypes
	}

	void OnEnable() {
		// undo button is set to inactive, but activated when something has been purchased
		foggyUndoButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject != null) {
			//Debug.Log(EventSystem.current.currentSelectedGameObject.name);
		}

		// add to text time and deactivate if it has been active for too long
		if (infoText.enabled) {
			infoTextTime += Time.deltaTime;
			if (infoTextTime > 1f) {
				infoText.enabled = false;
				infoTextTime = 0f;
			}
		}

		// TODO if certain color patterns are achieved, give extra points
	}

	public void ActivateInfoText(string str) {
		infoText.text = str;
		infoText.enabled = true;
	}

	public void PurchaseColor(GameObject obj) {
		// called on button click, parameter is the object of the button that was pressed

		// set upgrade type sprite, as a reminder which color is being used
		foggyUpgradeType.sprite = obj.GetComponent<Image>().sprite;

		// Resources looks in folder Assets/Resources
		currentSprite = obj.GetComponent<Image>().sprite;
		string spritePath = "Sprites/" + currentSprite.name.ToString() + "Empty";
		currentTransparentSprite = Resources.Load<Sprite>(spritePath);

		currentPrice = GetPriceForColors(obj.GetComponent<Image>().sprite.name);
		currentTime = GetTimeForColors(obj.GetComponent<Image>().sprite.name);
		currentPoints = (int) currentTime - (5 - 1);  // TODO make 5 a constant

		// close the window
		//honeyTypeMenu.SetActive(false);

		// activate fog
		foggyBackground.SetActive(true);
	}

	public void CancelHoneyTypePurchase() {
		// done in Unity
		/* 
		// deactivate fog
		foggyBackground.SetActive(false);
		// open window again
		honeyTypeMenu.SetActive(true);
		*/
	}

	public void UndoLastPurchase() {
		// give back honey
		honeyCurrency.currency += GetPriceForColors(lastUnitUpgraded.honeyFull.sprite.name);
		// revert unit to former
		lastUnitUpgraded.honeyFull.sprite = lastSprite;
		lastUnitUpgraded.honeyEmpty.sprite = lastTransparentSprite;
		lastUnitUpgraded.points = lastPoints;
		lastUnitUpgraded.price = lastPrice;
		lastUnitUpgraded.fillTime = lastTime;
		// deactivate the undo button
		foggyUndoButton.interactable = false;

		// save data if dataController is being used (used when going through mainmenu scene first)
		if (dataController != null) {
			dataController.SaveData(lastUnitUpgraded.name);
		}
	}

	// TODO make times shorter
	private float GetTimeForColors(string name) {
		// takes in the name of a sprite
		// returns the time for how long it takes honey of this type to get ready
		if (name == "hexhoneyYellow") {
			return 6f;
		}
		else if (name == "hexhoneyRed") {
			return 7f;
		}
		else if (name == "hexhoneyPink") {
			return 8f;
		}
		else if (name == "hexhoneyGreen") {
			return 9f;
		}
		else if (name == "hexhoneyPurple") {
			return 10f;
		}
		else if (name == "hexhoneyBlue") {
			return 11f;
		}
		else if (name == "hexhoneyWhite") {
			return 12f;
		}
		else if (name == "hexhoneyBlack") {
			return 13f;
		}
		return 0f;
	}

	// TODO make more expensive
	private int GetPriceForColors(string name) {
		// takes in the name of a sprite
		// returns the price of honey
		if (name == "hexhoneyYellow") {
			return 5;
		}
		else if (name == "hexhoneyRed") {
			return 10;
		}
		else if (name == "hexhoneyPink") {
			return 15;
		}
		else if (name == "hexhoneyGreen") {
			return 20;
		}
		else if (name == "hexhoneyPurple") {
			return 25;
		}
		else if (name == "hexhoneyBlue") {
			return 30;
		}
		else if (name == "hexhoneyWhite") {
			return 35;
		}
		else if (name == "hexhoneyBlack") {
			return 40;
		}
		return 0;
	}

}
