using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Code by github.com/S1r0hub.
 * 
 * Created: 2018/11/22
 * Updated: 2018/11/22
 */
namespace Wacki {

    abstract public class IUILaserPointerPickup : IUILaserPointer {

        protected override void Update() {
            // Don't do anything.
            // Done by "HandAttachedUpdate" function instead.
        }

    }

}
