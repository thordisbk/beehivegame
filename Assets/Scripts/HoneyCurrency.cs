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

	private GameObject addedValueFade;
	private float addedValueFade_origYpos;

	// Use this for initialization
	void Start () {
		tmpu = GetComponent<TextMeshProUGUI>();
		currency = 0;
		addedValueFade = this.gameObject.transform.GetChild(0).gameObject;
		addedValueFade_origYpos = addedValueFade.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		tmpu.text = currencyText + currency.ToString();
	}

	// outside classes use this to increment/decrement the currency value
	public void ChangeCurrencyValue(int value) {
		// value can be positive or negative
		if ((value >= 0 && currency < MAX_CURRENCY - value) || (value < 0 && currency + value >= 0)) {
			// if value does not make currency less than 0 or more than MAX_currency
			currency += value;

			// TODO added value text appear and fade out

			// create a string of what should be displayed
			string str = "+" + value.ToString();
			if (value < 0) str = value.ToString();
			// create a gameobject for the text that will be displayed
			//Instantiate(addedValueFade, transform.position, Quaternion.identity);
			//Destroy(addedValueFade, 1.5f);
			TextMeshProUGUI txt = addedValueFade.GetComponent<TextMeshProUGUI>();
			Vector2 v2 = addedValueFade.transform.position;
			v2.y = addedValueFade_origYpos;
			addedValueFade.transform.position = v2;

			txt.text = str;
			addedValueFade.SetActive(true);
			StartCoroutine(FadeTextToZeroAlpha(addedValueFade, 1.5f, txt, addedValueFade.transform));
		}
	}

	private IEnumerator FadeTextToZeroAlpha(GameObject go, float time, TextMeshProUGUI i, Transform transform)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / time));
			Vector2 v2 = transform.position;
			v2.y += Time.deltaTime * time * 10;  // how far it rises up
			transform.position = v2;
            yield return null;
        }
    }
}
