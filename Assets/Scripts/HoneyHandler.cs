using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// one for each honeycomb unit
public class HoneyHandler : MonoBehaviour {

	[HideInInspector] public float fillTime = 5f;		// time it takes unit to fill up
	public float readyTime = 10f;	// time honey can be in a unit before ?(it goes bad)?

	private bool isEmpty;		// true if no honey is in the unit, true if honey is in the unit
	private float emptyTime;
	private float fullTime;

	[HideInInspector] public int points;  // the points the honey gives when harvested
	[HideInInspector] public int price;	// price of this honey color

	[HideInInspector] public Image honeyEmpty;
	[HideInInspector] public Image honeyFull;

	private HoneyScore honeyScore;		  // for increasing honeyHarvested in HoneyScore when honey is harvested
	private HoneyCurrency honeyCurrency;  // for increasing currency in HoneyCurrency when honey is harvested

	private DataController dataController;  // for saving on click

	private GameManager gameManager;

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

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		honeyScore = GameObject.Find("HoneyScore").GetComponent<HoneyScore>();
		honeyCurrency = GameObject.Find("HoneyCurrency").GetComponent<HoneyCurrency>();

		dataController = GameObject.Find("DataController").GetComponent<DataController>();

		isEmpty = true;
		emptyTime = 0f;
		fullTime = 0f;

		foreach (Transform child in transform) {
			if (child.gameObject.name == "honeyEmptyButton") {
				honeyEmpty = child.gameObject.GetComponent<Image>();
			}
			else if (child.gameObject.name == "honeyFullImage") {
				honeyFull = child.gameObject.GetComponent<Image>();
			}
		}
		/*honeyEmpty.enabled = true;
		honeyFull.enabled = false;

		points = 1;
		price = 0;*/
	}
	
	// Update is called once per frame
	void Update () {
		if (isEmpty) {
			// then add to empty time
			emptyTime += Time.deltaTime;
			if (emptyTime >= fillTime) {
				//Debug.Log("FILL");
				// the honey appears
				isEmpty = false;
				emptyTime = 0f;
				honeyFull.enabled = true;
			}
		}
		else {
			fullTime += Time.deltaTime;
			if (fullTime >= readyTime) {
				//Debug.Log("GONE BAD");
				// honey goes bad
				// TODO
			}
		}
	}

	// called when honeyEmptyButton is clicked (honeyFullImage has Raycast Target disabled)
	public void OnUnitClick() {
		// TODO sound

		// if foggy background is active, change the the unit that is clicked on
		if (!gameManager.foggyBackground.activeSelf) {
			if (!isEmpty) {
				//Debug.Log("HARVEST");
				isEmpty = true;
				fullTime = 0f;
				honeyFull.enabled = false;

				honeyScore.harvestedHoney++; // += points; // TODO
				honeyCurrency.currency += points;
			}
			else {
				//Debug.Log("Unit is empty");
			}
		}
		else {
			if (honeyCurrency.currency >= gameManager.currentPrice) {

				// for undo
				gameManager.lastUnitUpgraded = this;
				gameManager.lastSprite = honeyFull.sprite;
				gameManager.lastTransparentSprite = honeyEmpty.sprite;
				gameManager.lastPrice = price;
				gameManager.lastTime = fillTime;
				gameManager.lastPoints = points;
				gameManager.foggyUndoButton.interactable = true;  // activate the undo button

				// then honey color is being changed
				honeyFull.sprite = gameManager.currentSprite;
				honeyEmpty.sprite = gameManager.currentTransparentSprite;
				fillTime = gameManager.currentTime; // TODO change for time, not price
				points = gameManager.currentPoints;
				price = gameManager.currentPrice;
				// reset filling time
				isEmpty = true;
				fullTime = 0f;
				honeyFull.enabled = false;
				// decrease honeycurrency
				honeyCurrency.currency -= (int) gameManager.currentPrice;
			}
			else {
				// not enough harvested honey to buy this color
				gameManager.ActivateInfoText("Not enough honey");
			}
		}

		// save data if dataController is being used (used when going through mainmenu scene first)
		if (dataController != null) {
			dataController.SaveData(this.name);
		}
	}
}
