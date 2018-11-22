using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wacki;
using Valve.VR;
using Valve.VR.InteractionSystem;


/**
 * Written by github.com/S1r0hub
 * 
 * Created: 2018/11/22
 * Updated: 2018/11/22
 * 
 * Some methods are from Wacki as mentioned in
 * the default ViveUILaserPointer script.
 */
 [RequireComponent(typeof(Interactable))]
public class ViveUILaserPointerPickup : IUILaserPointerPickup {

    public SteamVR_Action_Boolean toggleButton;
    public SteamVR_Action_Boolean triggerButton;

    // now assigned by pickup event
    protected Hand controller;
    private bool lastToggleState = false;

    private bool available() {
        return controller && isLaserActive();
    }

    protected override void Initialize() {
        base.Initialize();
        Debug.Log("Initialize ViveUILaserPointer");
    }

    public override bool ButtonDown() {
        if (!available()) { return false; }
        bool state = triggerButton.GetStateDown(controller.handType);
        //Debug.Log("ButtonDown event (" + state + ")");
        return state;
    }

    public override bool ButtonUp() {
        if (!available()) { return false; }
        bool state = triggerButton.GetStateUp(controller.handType);
        //Debug.Log("ButtonUp event (" + state + ")");
        return state;
    }
        
    public override void OnEnterControl(GameObject control) {
        if (!available()) { return; }
        // ToDo: haptic pulse
    }

    public override void OnExitControl(GameObject control) {
        if (!available()) { return; }
        // ToDo: haptic pulse
    }

    public override bool ButtonToggleClicked() {
        if (!controller) { return false; }
            
        // get the current button state and check if it changed from true to false
        bool stateChangedToTrue = false;
        bool toggleState = toggleButton.GetStateDown(controller.handType);
        if (!toggleState && toggleState != lastToggleState) { stateChangedToTrue = true; }
        lastToggleState = toggleState;
        return stateChangedToTrue;
    }

	//-------------------------------------------------
	private void OnAttachedToHand(Hand attachedHand) {
        controller = attachedHand;
        Debug.Log("Pickup Laser Pointer attached to hand: " + attachedHand.name);
	}

	//-------------------------------------------------
    // Equal to Update Method!
    // Will be called by "Hand.cs -> protected virtual void Update()" every frame.
    // Only called by the hand that this object is attached to!
	private void HandAttachedUpdate(Hand hand) {

		// Reset transform since we cheated it right after getting poses on previous frame
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

        // perform a laser update call
        UpdateCall();
    }

	//-------------------------------------------------
	private void OnHandFocusLost(Hand hand) {
		gameObject.SetActive(false);
	}


	//-------------------------------------------------
	private void OnHandFocusAcquired(Hand hand) {
		gameObject.SetActive(true);
		OnAttachedToHand(hand);
	}


	//-------------------------------------------------
	private void OnDetachedFromHand(Hand hand) {
        Debug.Log("Pickup Laser Pointer detached from hand!");
		Destroy(gameObject);
	}


	//-------------------------------------------------
	void OnDestroy() {
		ShutDown();
	}

	//-------------------------------------------------
	private void ShutDown() {
        
        // remove the controller from the input module
        LaserPointerInputModule.instance.RemoveController(this);
    }

}
