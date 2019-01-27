using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// one for each honeycomb unit
public class HoneyHandler : MonoBehaviour {

	private int COMB_PRICE = 300;  // the price of a new comb unit
	// TODO increases when a unit is bought?

	[HideInInspector] public Image honeyEmpty;	// a honey unit's image when it's empty
	[HideInInspector] public Image honeyFull;	// a honey unit's image when it's full
	[HideInInspector] public bool isActivated;  // false if unit hasn't been bought yet
	[HideInInspector] public int points;  		// the currency points the honey gives when harvested
	[HideInInspector] public int price;			// the price of the current honey color
	[HideInInspector] public float fillTime;	// time it takes the unit to fill up and stop being empty
	
	private bool isEmpty;			// true if no honey is in the unit, true if honey is in the unit
	private float readyTime;		// TODO? time honey can be in a unit before ?(it goes bad)?
	private float emptyTime;		// how long the unit has been empty
	private float fullForTime;		// how long the unit has been full
	private AudioSource pushSound;	// sound when honey is being harvested

	private HoneyScore honeyScore;		  // for increasing honeyHarvested in HoneyScore when honey is harvested
	private HoneyCurrency honeyCurrency;  // for increasing currency in HoneyCurrency when honey is harvested
	private GameManager gameManager;		// for accessing variables and functions
	private DataController dataController;  // for saving changes

	// the other units that could possibly touch this unit (most six), used by ExpandHoneyComb
	//[HideInInspector] public HoneyHandler[] otherSix;// = new HoneyHandler[6];
	[HideInInspector] public List<HoneyHandler> otherSix = new List<HoneyHandler>();

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		dataController = GameObject.Find("DataController").GetComponent<DataController>();
		honeyScore = GameObject.Find("HoneyScore").GetComponent<HoneyScore>();
		honeyCurrency = GameObject.Find("HoneyCurrency").GetComponent<HoneyCurrency>();

		isEmpty = true;
		emptyTime = 0f;
		fullForTime = 0f;

		foreach (Transform child in transform) {
			if (child.gameObject.name == "honeyEmptyButton") {
				honeyEmpty = child.gameObject.GetComponent<Image>();
			}
			else if (child.gameObject.name == "honeyFullImage") {
				honeyFull = child.gameObject.GetComponent<Image>();
			}
		}
		readyTime = 10f;

		// the units surrounding this one
		FillotherSix();

		//pushSound = GetComponent<AudioSource>();
		pushSound = GameObject.Find("SoundsAudio").GetComponent<AudioSource>();
	}

	private void FillotherSix() {

		int offset1 = 1;
		int offset2 = 6;
		int offset3 = 7;

		// get the number of this unit
		string strNum = this.name.Split('(')[1].Split(')')[0]; // name is honeyUnit (x)
		int x = int.Parse(strNum);
		//print("Number: " + strNum);

		GameObject temp;
		if (x % (offset2 + offset3) == 0) { // left side, longer row
			// do not check x-1, x+6, x-7
			temp = GameObject.Find("honeyUnit (" + (x + offset1).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
		}
		else if ((x - offset3) % (offset2 + offset3) == 0) {  // left side, shorter row
			// do not check x-1
			temp = GameObject.Find("honeyUnit (" + (x + offset1).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
		}
		else if ((x - offset2) % (offset2 + offset3) == 0) {  // right side, longer row
			// do not check x+1, x-6, x+7
			temp = GameObject.Find("honeyUnit (" + (x - offset1).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
		}
		else if ((x - 2 * offset2) % (offset2 + offset3) == 0) {  // right side, shorter row
			// do not check x+1
			temp = GameObject.Find("honeyUnit (" + (x - offset1).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
		}
		else {
			temp = GameObject.Find("honeyUnit (" + (x - offset1).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset1).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset2).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x - offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
			temp = GameObject.Find("honeyUnit (" + (x + offset3).ToString() + ")");
			if (temp != null) otherSix.Add(temp.GetComponent<HoneyHandler>());
		}
		
		//print(otherSix.Count);
	}
	
	// Update is called once per frame
	void Update () {
		if (isActivated && isEmpty) {
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
		else if (isActivated && !isEmpty) {
			fullForTime += Time.deltaTime;
			if (fullForTime >= readyTime) {
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

			// if unit is being bought
			if (!isActivated) {
				// then the empty sprite is being displayed
				//Debug.Log(this.name + " has not been activated");
				// TODO if enough money, buy
				if (honeyCurrency.currency >= COMB_PRICE) {
					honeyCurrency.ChangeCurrencyValue(-1 * COMB_PRICE);
					honeyEmpty.sprite = Resources.Load<Sprite>("Sprites/" + honeyFull.sprite.name + "Empty");
					isActivated = true;
					gameManager.DisplayUnpurchasedHoney();
				}
				else {
					gameManager.ActivateInfoText("Not enough money");
				}
			}
			// if unit is being harvested
			else if (!isEmpty) {
				//Debug.Log("HARVEST");
				isEmpty = true;
				fullForTime = 0f;
				honeyFull.enabled = false;

				honeyScore.ChangeHarvestedHoneyValue(1);
				honeyCurrency.ChangeCurrencyValue(points);

				pushSound.Play();
			}
			else if (isEmpty) {
				//Debug.Log("Unit is empty");
			}
		}
		else if (gameManager.foggyBackground.activeSelf && isActivated) {
			if (honeyCurrency.currency >= gameManager.currentPrice) {

				// make so that color cannot be bought if unit is already of this color
				if (honeyFull.sprite != gameManager.currentSprite) {
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
					fullForTime = 0f;
					honeyFull.enabled = false;
					// decrease honeycurrency
					honeyCurrency.ChangeCurrencyValue(-1 * (int) gameManager.currentPrice);
				}
				else Debug.Log("ABORT: you have already bought this");
			}
			else {
				// not enough harvested honey to buy this color
				gameManager.ActivateInfoText("Not enough money");
			}
		}

		// save data if dataController is being used (used when going through mainmenu scene first)
		if (dataController != null) {
			dataController.SaveData(this.name);
		}
	}
}
