using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueText : MonoBehaviour
{
	private TextMeshProUGUI tmproText;
	
    // Start is called before the first frame update
    void Start()
    {
	    tmproText = GetComponent<TextMeshProUGUI>();
	    GetComponentInParent<Slider>().onValueChanged.AddListener(HandleValueChanged);
    }

	private void HandleValueChanged(float value)
	{
		tmproText.text = value.ToString();
	}
}
