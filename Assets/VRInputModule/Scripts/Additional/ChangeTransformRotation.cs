using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * The purpose of this script is to
 * change the transform rotation value of the directional light
 * according to the slider value. This results in a change of the suns position.
 * 
 * Attach it to the directional light so that it can access its Transform component.
 * 
 * Author: github.com/S1r0hub
 */
public class ChangeTransformRotation : MonoBehaviour {

    public Slider slider;
    public Vector3 rotationAxis = new Vector3(1, 0, 0);
    public float mapSliderMinValueTo = 0;
    public float mapSliderMaxValueTo = 360;

    private Transform lightTransform;
    private bool active = true;

    void Start () {

        if (!slider) {
            Debug.LogError("Missing slider!");
            active = false;
        }

        lightTransform = GetComponent<Transform>();
        if (!lightTransform) {
            Debug.LogError("Unable to find Transform component!");
            active = false;
        }
    }

    /**
     * Called if the slider value changed.
     * The value can be anything between the min and max value set for the slider.
     */
    void Update () {
        if (active) {
            float value = slider.value;
            float valPercentage = slider.minValue + value / (slider.maxValue - slider.minValue); // you can also user slider.normalizedValue
            float rotationValue = mapSliderMinValueTo + valPercentage * (mapSliderMaxValueTo - mapSliderMinValueTo);
            lightTransform.localRotation = Quaternion.Euler(rotationAxis * rotationValue);
        }
    }
}
