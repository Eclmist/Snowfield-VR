using UnityEngine;
using UnityEngine.EventSystems;

namespace Edwon.VR {
    abstract public class ILaserPointer : MonoBehaviour
    {

        public float laserThickness = 0.001f;
        public float laserHitScale = 0.01f;
        [HideInInspector]
        public bool laserAlwaysOn = false;
        public Color color;

        // the cursor that appears when pointing at the ui
        [HideInInspector]
        public GameObject hitPoint;
        // the laser line
        private GameObject pointer;
        // basically like raycast hit data when pointing at the ui
        private PointerEventData pointerData;

        private float _distanceLimit;

        // Use this for initialization
        void Start()
        {
            // todo:    let the user choose a mesh for laser pointer ray and hit point
            //          or maybe abstract the whole menu control some more and make the 
            //          laser pointer a module.
            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.transform.SetParent(transform, false);
            pointer.transform.localScale = new Vector3(laserThickness, laserThickness, 100.0f);
            pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);
            if (!laserAlwaysOn)
                pointer.SetActive(false);

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

        void OnDestroy()
        {
            if(LaserPointerInputModule.instance != null)
                LaserPointerInputModule.instance.RemoveController(this);
        }

        protected virtual void Initialize() { }
        public virtual void OnEnterControl(GameObject control)
        {
            if(!laserAlwaysOn)
                pointer.SetActive(true);
        }
        public virtual void OnUpdateControl(GameObject control, PointerEventData _pointerData)
        {
            pointerData = _pointerData;
        }
        public virtual void OnExitControl(GameObject control)
        {
            if (!laserAlwaysOn)
                pointer.SetActive(false);
        }
        abstract public bool ButtonDown();
        abstract public bool ButtonUp();

        public void Update()
        {
            //Ray ray = new Ray(transform.position, transform.forward);
            //RaycastHit hitInfo;
            //bool bHit = Physics.Raycast(ray, out hitInfo);

            float distance = 100.0f;
            bool bHit = false;

            // there is pointer data
            if (pointerData != null)
            {
                // if the pointer is pointing at something
                RaycastResult raycastResult = pointerData.pointerCurrentRaycast;
                if (raycastResult.distance > 0)
                {
                    distance = raycastResult.distance;

                    // ugly, but has to do for now
                    if (_distanceLimit > 0.0f)
                    {
                        distance = Mathf.Min(distance, _distanceLimit);
                        bHit = true;
                    }
                }
                else
                {
                    // if the ui is gone, stop rendering the laser pointer line
                    if (!laserAlwaysOn)
                        pointer.SetActive(false);
                }
            }

            // render normal laser pointer
            if (laserAlwaysOn)
            {
                pointer.transform.localScale = new Vector3(laserThickness, laserThickness, distance);
                pointer.transform.localPosition = new Vector3(0.0f, 0.0f, distance * 0.5f);
            }

            if(bHit)
            {
                // turn on the hit point
                hitPoint.SetActive(true);

                // set hit point position on ui
                hitPoint.transform.position = transform.position + (transform.forward * distance);

                // render ui laser pointer
                pointer.transform.localScale = new Vector3(laserThickness*2, laserThickness*2, distance);
                pointer.transform.position = transform.position + (transform.forward * (distance * 0.5f));
            }
            else
            {
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
    }

}