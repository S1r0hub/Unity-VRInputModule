using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


/**
 * Code modified by S1r0hub.
 * Initial code by Wacki.
 */
namespace Wacki {

    public class ViveUILaserPointer : IUILaserPointer {

        //public EVRButtonId button = EVRButtonId.k_EButton_SteamVR_Trigger;

        public Hand controller;
        public SteamVR_Action_Boolean triggerButton;

        protected override void Initialize() {
            base.Initialize();
            Debug.Log("Initialize ViveUILaserPointer");
        }

        public override bool ButtonDown() {
            return triggerButton.GetStateDown(controller.handType);
        }

        public override bool ButtonUp() {
            return triggerButton.GetStateUp(controller.handType);
        }
        
        public override void OnEnterControl(GameObject control) {
            // ToDo: haptic pulse
        }

        public override void OnExitControl(GameObject control) {
            // ToDo: haptic pulse
        }

    }

}