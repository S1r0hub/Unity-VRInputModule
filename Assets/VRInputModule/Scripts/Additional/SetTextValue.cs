using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Sets the value of a slider as text.
 * Attach this script to a UI text element.
 */
public class SetTextValue : MonoBehaviour {

    public Slider slider;

    private Text text;
    private bool active = true;

    void Start () {
        
        if (!slider) {
            Debug.LogError("Missing slider!");
            active = false;
        }

        text = GetComponent<Text>();
        if (!text) {
            Debug.LogError("Unable to find Text component!");
            active = false;
        }
    }
    
    void Update () {
        
        if (active) {
            text.text = (Mathf.Round(slider.value * 10000.0f) / 10000.0f).ToString();
        }
    }
}
