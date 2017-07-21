using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace Edwon.VR
{

    public class LaserPointerInputModule : BaseInputModule
    {

        public static LaserPointerInputModule instance { get { return Instance; } }
        private static LaserPointerInputModule Instance = null;
                
        // storage class for controller specific data
        public class ControllerData {
            public PointerEventData pointerEvent;
            public GameObject currentPoint;
            public GameObject currentPressed;
            public GameObject currentDragging;
        };

        [HideInInspector]
        public Camera _UICamera;
        public Camera UICamera
        {
            get
            {
                if (_UICamera == null)
                {
                    // Create a new camera that will be used for raycasts
                    _UICamera = new GameObject("UI Camera").AddComponent<Camera>();
                    _UICamera.transform.parent = transform;
                    _UICamera.clearFlags = CameraClearFlags.Nothing;
                    _UICamera.cullingMask = 0;
                    _UICamera.fieldOfView = .01f;
                    _UICamera.nearClipPlane = .001f;
                    return _UICamera;
                }
                return _UICamera;
            }
        }
        private HashSet<ILaserPointer> _controllers;
        [HideInInspector]
        public HashSet<ILaserPointer> _Controllers
        {
            set
            {
                _Controllers = value;
            }
            get
            {
                return _controllers; 
            }
        }

        // controller data
        private Dictionary<ILaserPointer, ControllerData> _controllerData = new Dictionary<ILaserPointer, ControllerData>();

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null)
            {
                Debug.LogWarning("Trying to instantiate multiple LaserPointerInputModule.");
                //DestroyImmediate(Instance);
            }

            Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            
            //// Find canvases in the scene and assign our custom
            //// UICamera to them
            Canvas canvas = GetComponent<Canvas>();
            canvas.worldCamera = UICamera;

        }

        public void OnLevelWasLoaded()
        {
            Start();
        }

        public void AddController(ILaserPointer controller)
        {
            _controllerData.Add(controller, new ControllerData());
        }

        public void RemoveController(ILaserPointer controller)
        {
            _controllerData.Remove(controller);
        }

        // get if the ILaserPointer (controller) pressing on some GameObject
        public ILaserPointer IsLaserPointerPressingAt(GameObject go)
        {
            for (int i = _controllerData.Count-1; i > 0; i--)
            {
                ILaserPointer laserPointer = _controllerData.ElementAt(i).Key;
                ControllerData data = _controllerData.ElementAt(i).Value;
                if (data.currentPressed == go)
                {
                    return laserPointer;
                }
            }
            return null;
        }

        // get if the ILaserPointer (controller) pointing at some GameObject
        public ILaserPointer IsLaserPointerPointingAt(GameObject go)
        {
            for (int i = _controllerData.Count-1; i > 0; i--)
            {
                ILaserPointer laserPointer = _controllerData.ElementAt(i).Key;
                ControllerData data = _controllerData.ElementAt(i).Value;
                if (data.currentPoint == go)
                {
                    return laserPointer;
                }
            }
            return null;
        }

        // get if the ILaserPointer (controller) pointing at a child of some transform
        public ILaserPointer IsLaserPointerPointingAtChildOF(Transform parent)
        {
            for (int i = _controllerData.Count-1; i > 0; i--)
            {
                ILaserPointer laserPointer = _controllerData.ElementAt(i).Key;
                ControllerData data = _controllerData.ElementAt(i).Value;
                List<GameObject> currentPointing = data.pointerEvent.hovered;
                foreach(GameObject go in currentPointing)
                {
                    if (go.transform.IsChildOf(parent))
                    {
                        return laserPointer;
                    }
                }
            }
            return null;
        }

        protected void UpdateCameraPosition(ILaserPointer controller)
        {
            UICamera.transform.position = controller.transform.position;
            UICamera.transform.rotation = controller.transform.rotation;
        }

        // clear the current selection
        public void ClearSelection()
        {
            if(base.eventSystem.currentSelectedGameObject) {
                base.eventSystem.SetSelectedGameObject(null);
            }
        }

        // select a game object
        private void Select(GameObject go)
        {
            ClearSelection();

            if(ExecuteEvents.GetEventHandler<ISelectHandler>(go)) {
                base.eventSystem.SetSelectedGameObject(go);
            }
        }

        public override void Process()
        {
            for (int i = _controllerData.Count-1; i >= 0; i--)
            {
                ILaserPointer controller = _controllerData.ElementAt(i).Key;
                ControllerData data = _controllerData.ElementAt(i).Value;

                // Test if UICamera is looking at a GUI element
                UpdateCameraPosition(controller);

                if(data.pointerEvent == null)
                    data.pointerEvent = new PointerEventData(eventSystem);
                else
                    data.pointerEvent.Reset();

                data.pointerEvent.delta = Vector2.zero;
                data.pointerEvent.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                data.pointerEvent.scrollDelta = Vector2.zero;

                // trigger a raycast
                eventSystem.RaycastAll(data.pointerEvent, m_RaycastResultCache);
                data.pointerEvent.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
                m_RaycastResultCache.Clear();

                // make sure our controller knows about the raycast result
                // we add 0.0001 because that is the near plane distance of our camera and we want the correct distance
                if(data.pointerEvent.pointerCurrentRaycast.distance > 0.0f)
                    controller.LimitLaserDistance(data.pointerEvent.pointerCurrentRaycast.distance + .001f);

                // stop if no UI element was hit
                //if(pointerEvent.pointerCurrentRaycast.gameObject == null)
                //return;

                // Send control enter and exit events to our controller
                var hitControl = data.pointerEvent.pointerCurrentRaycast.gameObject;

                if(data.currentPoint != hitControl)
                {
                    if(data.currentPoint != null)
                        controller.OnExitControl(data.currentPoint);

                    if(hitControl != null)
                        controller.OnEnterControl(hitControl);
                }

                if (hitControl != null)
                    controller.OnUpdateControl(hitControl, data.pointerEvent);

                data.currentPoint = hitControl;

                // Handle enter and exit events on the GUI controlls that are hit
                base.HandlePointerExitAndEnter(data.pointerEvent, data.currentPoint);

                // button down begin
                if ( controller.ButtonDown() )
                {

                    ClearSelection();

                    data.pointerEvent.pressPosition = data.pointerEvent.position;
                    data.pointerEvent.pointerPressRaycast = data.pointerEvent.pointerCurrentRaycast;
                    data.pointerEvent.pointerPress = null;

                    // update current pressed if the curser is over an element
                    if(data.currentPoint != null) {
                        data.currentPressed = data.currentPoint;

                        GameObject newPressed = ExecuteEvents.ExecuteHierarchy(data.currentPressed, data.pointerEvent, ExecuteEvents.pointerDownHandler);
                        if(newPressed == null) {
                            // some UI elements might only have click handler and not pointer down handler
                            newPressed = ExecuteEvents.ExecuteHierarchy(data.currentPressed, data.pointerEvent, ExecuteEvents.pointerClickHandler);
                            if(newPressed != null) {
                                data.currentPressed = newPressed;
                            }
                        }
                        else {
                            data.currentPressed = newPressed;
                            // we want to do click on button down at same time, unlike regular mouse processing
                            // which does click when mouse goes up over same object it went down on
                            // reason to do this is head tracking might be jittery and this makes it easier to click buttons
                            ExecuteEvents.Execute(newPressed, data.pointerEvent, ExecuteEvents.pointerClickHandler);
                        }

                        if(newPressed != null) {
                            data.pointerEvent.pointerPress = newPressed;
                            data.currentPressed = newPressed;
                            Select(data.currentPressed);
                        }

                        ExecuteEvents.Execute(data.currentPressed, data.pointerEvent, ExecuteEvents.beginDragHandler);
                        data.pointerEvent.pointerDrag = data.currentPressed;
                        data.currentDragging = data.currentPressed;
                    }
                }
                // button down end

                // button up begin
                if( controller.ButtonUp() )
                {
                    if(data.currentDragging != null) {
                        ExecuteEvents.Execute(data.currentDragging, data.pointerEvent, ExecuteEvents.endDragHandler);
                        if(data.currentPoint != null) {
                            ExecuteEvents.ExecuteHierarchy(data.currentPoint, data.pointerEvent, ExecuteEvents.dropHandler);
                        }
                        data.pointerEvent.pointerDrag = null;
                        data.currentDragging = null;
                    }
                    if(data.currentPressed) {
                        ExecuteEvents.Execute(data.currentPressed, data.pointerEvent, ExecuteEvents.pointerUpHandler);
                        data.pointerEvent.rawPointerPress = null;
                        data.pointerEvent.pointerPress = null;
                        data.currentPressed = null;
                    }
                }
                // button up end

                // drag handling
                if(data.currentDragging != null) {
                    ExecuteEvents.Execute(data.currentDragging, data.pointerEvent, ExecuteEvents.dragHandler);
                }
            }
        }

        protected override void OnDisable()
        {
            //Instance = null;
            base.OnDisable();

        }
    }
}