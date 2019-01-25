using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandHoneyComb : MonoBehaviour {

	private GameObject beehiveAdditions;  	// additions
	private GameObject beehive; 			// original

	// get a list of all the honey units in BeehiveAdditions
	//private HoneyHandler[] honeyUnits = new HoneyHandler[30];
	
	// indicates whether the unactivated beehive additions are visible or not
	[HideInInspector] public bool additionsVisible;  
	
	// Use this for initialization
	void Start () {
		beehiveAdditions = this.gameObject;
		beehive = GameObject.Find("Beehive");

		//foreach (Transform child in beehiveAdditions.transform) {
  			// for each unit, set sprite
			//child.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/hexhoneyADD");
			/*if (!child.GetComponent<HoneyHandler>().isActivated) {
				child.gameObject.SetActive(false);
			}*/
		//}

		additionsVisible = false;
		// hide the additions
		// TODO only hide the inactive ones
		//beehiveAdditions.SetActive(false);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PurchaseNewCombSlot() {
		// called when button is pressed

		// show all inactive units with dotted lines ()
		beehiveAdditions.SetActive(true);

	}

	public void DisplayUnpurchasedHoney() {
		foreach (Transform child in beehiveAdditions.transform) {
			child.gameObject.SetActive(true);
		}
		additionsVisible = true;

		foreach (Transform child in beehive.transform) {
			child.gameObject.SetActive(true);
		}
	}

	public void HideUnpurchasedHoney() {
		foreach (Transform child in beehiveAdditions.transform) {
			if (!child.GetComponent<HoneyHandler>().isActivated) {
				child.gameObject.SetActive(false);
			}
		}
		additionsVisible = false;

		foreach (Transform child in beehive.transform) {
			if (!child.GetComponent<HoneyHandler>().isActivated) {
				child.gameObject.SetActive(false);
			}
		}
	}
}
