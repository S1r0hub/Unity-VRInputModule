using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Simple script to make an object rotate towards another object.
 * Author: github.com/S1r0hub
 */
public class FaceCamera : MonoBehaviour {

    public bool active = false;
    public Transform lookAt;
    public Transform lookAtAlternative; // look at this if the other is not assigned or available

    public Vector3 rotationOffset = new Vector3(0, 180, 0);
    public float waitBeforeActive = 0;
    private float startTime = 0;

    private void Start() {
        startTime = Time.time;
    }

    void Update () {
		
        if (!active) { return; }
        if (!lookAt && !lookAtAlternative) { return; }
        if (Time.time < startTime + waitBeforeActive) { return; }

        // use lookAt or alternative transform
        Transform lookingAt = lookAt;
        if (!lookingAt || !lookAt.gameObject.activeInHierarchy) { lookingAt = lookAtAlternative; }
        if (!lookingAt || !lookingAt.gameObject.activeInHierarchy) { return; }

        Vector3 v = lookingAt.transform.position - transform.position;
        v.x = v.z = 0;
        transform.LookAt(lookingAt.transform.position - v);
        //transform.rotation = lookAtNow.transform.rotation;
        transform.Rotate(rotationOffset);
	}

    public void Enable() {
        active = true;
    }

    public void Disable() {
        active = false;
    }

    /** Switch current active status. */
    public void Toggle() {
        active = !active;
    }
}
