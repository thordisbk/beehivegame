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

	private GameObject beehive; 
	// indicates whether the unactivated beehive additions are visible or not
	[HideInInspector] public bool additionsVisible;  

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;

		//foggyBackground = GameObject.Find("TypeChooseFog");  // only when private an fogg.. is active

		infoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
		infoText.text = "";
		infoText.enabled = false;
		infoTextTime = 0f;

		//honeyScore = GameObject.Find("HoneyScore").GetComponent<HoneyScore>();
		honeyCurrency = GameObject.Find("HoneyCurrency").GetComponent<HoneyCurrency>();

		dataController = GameObject.Find("DataController").GetComponent<DataController>();

		beehive = GameObject.Find("Beehive");
		additionsVisible = false;

	}

	// TODO let this happen when window is closed
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

	/* For changing color of a honey unit */

	public void PurchaseColor(GameObject obj) {
		// called on button click, parameter is the object of the button that was pressed

		// set upgrade type sprite, as a reminder which color is being used
		foggyUpgradeType.sprite = obj.GetComponent<Image>().sprite;

		// Resources looks in folder Assets/Resources
		currentSprite = obj.GetComponent<Image>().sprite;
		string spritePath = "Sprites/" + currentSprite.name.ToString() + "Empty";
		currentTransparentSprite = Resources.Load<Sprite>(spritePath);

		//currentPrice = GetPriceForColors(obj.GetComponent<Image>().sprite.name);
		//currentTime = GetTimeForColors(obj.GetComponent<Image>().sprite.name);
		currentPrice = dataController.honeyDict[obj.GetComponent<Image>().sprite.name].price;
		currentTime = dataController.honeyDict[obj.GetComponent<Image>().sprite.name].time;
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
		honeyCurrency.ChangeCurrencyValue(dataController.honeyDict[lastUnitUpgraded.honeyFull.sprite.name].price);
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

	/* For expanding the honey comb */

	public void DisplayUnpurchasedHoney() {
		// display honeyUnits that can be bought, called by button and when a unit is bought

		// check all units
		foreach (Transform child in beehive.transform) {
			HoneyHandler temp = child.gameObject.GetComponent<HoneyHandler>();
			// check if surrounding units are active, if any of them is then display 
			for (int i = 0; i < temp.otherSix.Count; i++) {
				if (temp.otherSix[i].isActivated) {
					child.gameObject.SetActive(true);
				}
			}
		}

		additionsVisible = true;
	}

	public void HideUnpurchasedHoney() {

		// for all units, if it has not been activated, then hide it
		foreach (Transform child in beehive.transform) {
			if (!child.GetComponent<HoneyHandler>().isActivated) {
				child.gameObject.SetActive(false);
			}
		}
		additionsVisible = false;
	}

}
