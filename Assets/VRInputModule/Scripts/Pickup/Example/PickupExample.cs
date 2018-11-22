using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


/**
 * Written by github.com/S1r0hub
 * 
 * Created: 2018/11/22
 * Updated: 2018/11/22
 * 
 * Used SteamVR Longbow Example code as reference.
 */
[RequireComponent(typeof(Interactable))]
public class PickupExample : MonoBehaviour {

    public SteamVR_Action_Boolean triggerButton;
	//private Hand hand; // uncomment if required for further actions


	//-------------------------------------------------
	private void OnAttachedToHand(Hand attachedHand) {
        Debug.Log("PickupExample attached to hand: " + attachedHand.name);
		//hand = attachedHand; // uncomment if required for further actions
	}

	//-------------------------------------------------
    // Equal to Update Method!
    // Will be called by "Hand.cs -> protected virtual void Update()" every frame.
    // Only called by the hand that this object is attached to!
	private void HandAttachedUpdate(Hand hand) {

		// Reset transform since we cheated it right after getting poses on previous frame
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

        // ToDo: Interaction (Click handling and so on...)
        if (triggerButton.GetStateDown(hand.handType)) {
            Debug.Log("Trigger button down!");
        }
        else if (triggerButton.GetStateUp(hand.handType)) {
            Debug.Log("Trigger button up!");
        }
    }

	//-------------------------------------------------
	private void ShutDown() {

        // we only have one thing in only one hand
        /*
		if (hand != null && hand.otherHand.currentAttachedObject != null) {
			if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>() != null) {
				if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>().itemPackage == arrowHandItemPackage) {
					hand.otherHand.DetachObject( hand.otherHand.currentAttachedObject );
				}
			}
		}
        */
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
        Debug.Log("PickupExample detached from hand!");
		Destroy(gameObject);
	}


	//-------------------------------------------------
	void OnDestroy() {
		ShutDown();
	}

}
