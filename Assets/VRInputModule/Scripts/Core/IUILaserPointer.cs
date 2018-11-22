using UnityEngine;


/**
 * Initial code by github.com/wacki.
 * 
 * Modified by github.com/S1r0hub.
 * Updated: 2018/11/22
 */
namespace Wacki {

    abstract public class IUILaserPointer : MonoBehaviour {

        public Transform laserOrigin;
        public Color color;
        public float laserThickness = 0.005f;
        public float laserHitScale = 0.02f;
        public bool laserAlwaysOn = false;

        private GameObject hitPoint;
        private GameObject pointer;
        private bool laserActive = false;

        private float _distanceLimit;

        // Use this for initialization
        void Start()
        {
            laserActive = laserAlwaysOn;

            // todo:    let the user choose a mesh for laser pointer ray and hit point
            //          or maybe abstract the whole menu control some more and make the 
            //          laser pointer a module.
            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.transform.SetParent(transform, false);
            pointer.transform.localScale = new Vector3(laserThickness, laserThickness, 100.0f);
            pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);
            pointer.SetActive(laserActive);

            hitPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hitPoint.transform.SetParent(transform, false);
            hitPoint.transform.localScale = new Vector3(laserHitScale, laserHitScale, laserHitScale);
            hitPoint.transform.localPosition = new Vector3(0.0f, 0.0f, 100.0f);
            hitPoint.SetActive(false);

            // remove the colliders on our primitives
            Object.DestroyImmediate(hitPoint.GetComponent<SphereCollider>());
            Object.DestroyImmediate(pointer.GetComponent<BoxCollider>());
            
            Material newMaterial = new Material(Shader.Find("Wacki/LaserPointer"));

            newMaterial.SetColor("_Color", color);
            pointer.GetComponent<MeshRenderer>().material = newMaterial;
            hitPoint.GetComponent<MeshRenderer>().material = newMaterial;

            // initialize concrete class
            Initialize();
            
            // register with the LaserPointerInputModule
            if(LaserPointerInputModule.instance == null) {
                new GameObject().AddComponent<LaserPointerInputModule>();
            }
            
            LaserPointerInputModule.instance.AddController(this);
        }

        private void OnDrawGizmos() {
         
            // show the laser origin
            if (laserOrigin) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(laserOrigin.position, 0.01f);
                Gizmos.DrawRay(laserOrigin.position, transform.forward * 0.1f);
            }
        }

        void OnDestroy()
        {
            if(LaserPointerInputModule.instance != null)
                LaserPointerInputModule.instance.RemoveController(this);
        }

        protected virtual void Initialize() { }

        public virtual void OnEnterControl(GameObject control) { }
        public virtual void OnExitControl(GameObject control) { }
        
        public abstract bool ButtonDown();
        public abstract bool ButtonUp();
        public abstract bool ButtonToggleClicked();

        protected virtual void Update() {
            UpdateCall();
        }

        /**
         * Performs laser update.
         * Using raycast to detect hit and showing it.
         * Moved to a separate method to be called from other methods as well.
         */
        protected virtual void UpdateCall() {

            // check if user turns laser on/off and react accordingly
            if (!laserAlwaysOn && ButtonToggleClicked()) {
                if (laserActive) { hideLaser(); }
                else { showLaser(); }
                Debug.Log("Laser pointer " + (laserActive ? "enabled" : "disabled"));
            }

            // don't do anything if the laser is disabled
            if (!laserActive) { return; }

            // use the origin transform position if provided
            Vector3 origin_pos = transform.position;
            if (laserOrigin) { origin_pos = laserOrigin.position; }

            // create and cast the ray that hits colliders (does not hit UI elements)
            Ray ray = new Ray(origin_pos, transform.forward);
            RaycastHit hitInfo;
            bool bHit = Physics.Raycast(ray, out hitInfo);

            float distance = 100.0f;
            if (bHit) { distance = hitInfo.distance; }

            // ugly, but has to do for now
            if (_distanceLimit > 0.0f) {
                distance = Mathf.Min(distance, _distanceLimit);
                bHit = true;
            }

            // scale and position the laser "ray"
            pointer.transform.localScale = new Vector3(laserThickness, laserThickness, distance);
            pointer.transform.position = ray.origin + distance * 0.5f * ray.direction;

            // position the hit point
            if (bHit) {
                hitPoint.SetActive(true);
                hitPoint.transform.position = ray.origin + distance * ray.direction;
            }
            else {
                hitPoint.SetActive(false);
            }

            // reset the previous distance limit
            _distanceLimit = -1.0f;
        }

        // limits the laser distance for the current frame
        public virtual void LimitLaserDistance(float distance)
        {
            if(distance < 0.0f)
                return;

            if(_distanceLimit < 0.0f)
                _distanceLimit = distance;
            else
                _distanceLimit = Mathf.Min(_distanceLimit, distance);
        }

        public void showLaser() {
            pointer.SetActive(true);
            laserActive = true;
        }

        public void hideLaser() {
            hitPoint.SetActive(false);
            pointer.SetActive(false);
            laserActive = false;
        }

        public bool isLaserActive() {
            return laserActive;
        }

    } // end of namespace
}
