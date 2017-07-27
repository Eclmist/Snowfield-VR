using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Edwon.VR.Gesture
{
    public class VRGestureGalleryGrid : MonoBehaviour
    {
        public List<GestureExample> examples; // the actual gesture examples
        public List<VRGestureGalleryExample> galleryExamples; // the UI representations of the samples
        public List<int> lineNumbers;

        [HideInInspector]
        public VRGestureGallery gallery; // the vr gesture gallery that owns me

        public GameObject titlePrefab;

        RectTransform gridParent;
        RectTransform titleParent;

        public void Init(VRGestureGallery _gallery, List<GestureExample> _examples, List<int> _lineNumbers)
        {
            gallery = _gallery;
            examples = _examples;
            lineNumbers = _lineNumbers;

            titleParent = (RectTransform)transform.Find("Title");
            gridParent = (RectTransform)transform.Find("Grid");

            GenerateTitle();
            GenerateGestureGalleryGrid();
        }

        void GenerateTitle()
        {
            GameObject titleGO = Instantiate(titlePrefab) as GameObject;
            titleGO.transform.parent = titleParent;
            titleGO.transform.localPosition = Vector3.zero;
            titleGO.transform.localRotation = Quaternion.identity;
            titleGO.transform.forward = -titleGO.transform.forward;
            titleGO.transform.localScale = Vector3.one;
            titleGO.name = "Title";

            Text titleText = titleGO.GetComponentInChildren<Text>();
            if (!gallery.currentGesture.isSynchronous)
            {
                titleText.text = gallery.currentGesture.name + "\nSingle Handed";
            }
            else
            {
                titleText.text = gallery.currentGesture.name + "\n" + examples[0].hand.ToString() + " Hand";
            }
        }

        void GenerateGestureGalleryGrid()
        {
            // go through all the gesture examples and draw them in a grid
            for (int i = 0; i < examples.Count; i++)
            {
                GameObject galleryExampleGO = Instantiate(gallery.examplePrefab.gameObject) as GameObject;
                galleryExampleGO.transform.parent = gridParent;
                galleryExampleGO.transform.localPosition = Vector3.zero;
                galleryExampleGO.transform.localRotation = Quaternion.identity;
                galleryExampleGO.name = "Example " + i;

                VRGestureGalleryExample galleryExample = galleryExampleGO.GetComponent<VRGestureGalleryExample>();
                galleryExamples.Add(galleryExample);
                galleryExample.Init(this, examples[i], lineNumbers[i]);
            }

            gallery.galleryState = VRGestureGallery.GestureGalleryState.Visible;
        }

        public void DestroyThisGrid()
        {
            Destroy(gameObject);
        }
    }
}