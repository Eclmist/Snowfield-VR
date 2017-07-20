using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Edwon.VR.Gesture
{
    public class VRGestureGalleryExample : MonoBehaviour
    {

        public CanvasGroup trash;

        public VRGestureGalleryGrid grid;

        public Button button;

        public GestureExample example;
        public int lineNumber;

        public GameObject lineDrawing;

        public void Init(VRGestureGalleryGrid _grid, GestureExample _example, int _lineNumber)
        {
            grid = _grid;
            example = _example;
            lineNumber = _lineNumber;

            // draw the line
            lineDrawing = DrawGesture(example.data, _lineNumber);

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            // set the trash icon position
            RectTransform trashTF = (RectTransform)trash.transform;
            trashTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            trashTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25);

            // add the button listener function
            button.onClick.AddListener(() => grid.gallery.DeleteGestureExample(example, lineNumber));
        }

        public void OnPointerEnter()
        {
            Utils.ToggleCanvasGroup(trash, false, 1);
        }

        public void OnPointerExit()
        {
            Utils.ToggleCanvasGroup(trash, false, 0);
        }

        public GameObject DrawGesture(List<Vector3> capturedLine, int gestureExampleNumber)
        {
            // spawn a game object
            GameObject lineGO = new GameObject();
            lineGO.name = "Gesture Example " + gestureExampleNumber;
            lineGO.transform.SetParent(transform);

            // position the game object to the corner of the UI
            RectTransform rt = (RectTransform)transform;
            Vector3[] canvasCorners = new Vector3[4];
            rt.GetWorldCorners(canvasCorners);
            lineGO.transform.position = canvasCorners[3];
            lineGO.transform.forward = -transform.forward;
            // offset the line position in front of the frame
            lineGO.transform.position += -lineGO.transform.forward * (grid.gallery.gestureDrawSize * 1.2f);

            // get the list of points in capturedLine and modify positions based on gestureDrawSize
            List<Vector3> capturedLineAdjusted = new List<Vector3>();
            foreach (Vector3 point in capturedLine)
            {
                Vector3 pointScaled = point * grid.gallery.gestureDrawSize;
                capturedLineAdjusted.Add(pointScaled);
            }

            LineRenderer lineRenderer = lineGO.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.SetColors(Color.blue, Color.green);
            //Add a taper to the line
            lineRenderer.SetWidth
                (
                    grid.gallery.lineWidth - (grid.gallery.lineWidth * .5f), 
                    grid.gallery.lineWidth + (grid.gallery.lineWidth * .5f)
                );
            lineRenderer.SetVertexCount(capturedLineAdjusted.Count);
            lineRenderer.SetPositions(capturedLineAdjusted.ToArray());

            return lineGO;
        }

    }
}