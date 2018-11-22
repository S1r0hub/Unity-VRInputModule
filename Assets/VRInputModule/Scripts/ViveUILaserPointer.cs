using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


/**
 * Initial code by Wacki.
 * Code modified by S1r0hub (11.2018)
 */
namespace Wacki {

    public class ViveUILaserPointer : IUILaserPointer {

        public Hand controller;
        public SteamVR_Action_Boolean toggleButton;
        public SteamVR_Action_Boolean triggerButton;

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

    }

}