using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoneyCurrency : MonoBehaviour {
	// similar to HoneyScore, but is decreased when honey is bought

	private int MAX_CURRENCY = 999999999;	// to avoid overflow

	[HideInInspector] public int currency;
	private string currencyText = " "; //"Currency: ";
	private TextMeshProUGUI tmpu;

	// Use this for initialization
	void Start () {
		tmpu = GetComponent<TextMeshProUGUI>();
		currency = 0;
	}
	
	// Update is called once per frame
	void Update () {
		tmpu.text = currencyText + currency.ToString();
	}

	// outside classes use this to increment/decrement the currency value
	public void ChangeCurrencyValue(int value) {
		// value can be positive or negative
		if (currency < MAX_CURRENCY - value) {
			currency += value;
		}
	}
}
