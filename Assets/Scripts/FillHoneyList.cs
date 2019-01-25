using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FillHoneyList : MonoBehaviour {

	private DataController dataController;  // for accessing info on honey types

	// fill the honey list with information (price, description) from the datacontroller
	void Start () {

		dataController = GameObject.Find("DataController").GetComponent<DataController>();

		foreach (Transform child in transform) {
			// child is 'HoneyType(x)', its children are 'ChooseButton', 'TypeName', 'Price'

			// get the honey type			
			string honeyType = child.GetChild(0).GetComponent<Image>().sprite.name;
			// get description
			string description = (string) dataController.honeyDict[honeyType].description;
			child.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
			// get price
			string price = dataController.honeyDict[honeyType].price.ToString();
			child.GetChild(2).GetComponent<TextMeshProUGUI>().text = price; 

		}
	}
	
}
