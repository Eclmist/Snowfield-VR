using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MadGoat_SSAA
{
    public class MadGoatSSAA_InternalRenderer : MonoBehaviour
    {

        [HideInInspector()]
        public float multiplier;

        // Shader Pramaters
        [HideInInspector()]
        public float sharpness;
        [HideInInspector()]
        public bool useShader;
        [HideInInspector()]
        public float sampleDistance;

        // Cameras
        [HideInInspector()]
        public Camera main;
        [HideInInspector()]
        public Camera current;

        // Shader Setup
        [SerializeField]
        private Shader _bilinearshader;
        public Shader bilinearshader
        {
            get
            {
                if (_bilinearshader == null)
                    _bilinearshader = Shader.Find("Hidden/SSAA_Bilinear");

                return _bilinearshader;
            }
        }
        [SerializeField]
        private Shader _bicubicshader;
        public Shader bicubicshader
        {
            get
            {
                if (_bicubicshader == null)
                    _bicubicshader = Shader.Find("Hidden/SSAA_Bicubic");

                return _bicubicshader;
            }
        }
        [SerializeField]
        private Shader _neighborshader;
        public Shader neighborshader
        {
            get
            {
                if (_neighborshader == null)
                {
                   _neighborshader = Shader.Find("Hidden/SSAA_Nearest");
                }
                return _neighborshader;
            }
        }

        private Material material_bl; // Bilinear Material
        private Material material_bc; // Bicubic
        private Material material_nn; // Nearest Neighbor

        private Material material_current;

        MadGoatSSAA mainComponent;
        private void Start()
        {
            mainComponent = main.GetComponent<MadGoatSSAA>();
            material_bl = new Material(bilinearshader);
            material_bc = new Material(bicubicshader);
            material_nn = new Material(neighborshader);
            material_current = material_bc;
        }
        /// <summary>
        /// Change the shader to use in the internal renderer
        /// </summary>
        public void ChangeMaterial(Filter Type)
        {
            // Point material_current to the given material
            switch (Type)
            {
                case Filter.NEAREST_NEIGHBOR:
                    material_current = material_nn;
                    break;
                case Filter.BILINEAR:
                    material_current = material_bl;
                    break;
                case Filter.BICUBIC:
                    material_current = material_bc;
                    break;
            }
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // is a screenshot queued?
            if (mainComponent.Settings.TakeScreenshot)
            {
                Material material = new Material(bicubicshader);
                // buffer to store texture
                RenderTexture buff = new RenderTexture((int)mainComponent.Settings.OutputResolution.x, (int)mainComponent.Settings.OutputResolution.y, 24, RenderTextureFormat.ARGB32);

                // setup shader
                if (mainComponent.Settings.UseShader)
                {
                    material.SetFloat("_ResizeWidth", (int)mainComponent.Settings.OutputResolution.x);
                    material.SetFloat("_ResizeHeight", (int)mainComponent.Settings.OutputResolution.y);
                    material.SetFloat("_Sharpness", 0.85f);
                    Graphics.Blit(main.targetTexture, buff, material, 0);
                }
                else // or blit as it is
                {
                    Graphics.Blit(main.targetTexture, buff);
                }
                DestroyImmediate(material);
                RenderTexture.active = buff;

                Texture2D screenshotBuffer = new Texture2D(RenderTexture.active.width, RenderTexture.active.height, TextureFormat.RGB24, false);
                screenshotBuffer.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);

                (new FileInfo(mainComponent.Settings.ScreenshotPath)).Directory.Create();
                File.WriteAllBytes(mainComponent.Settings.ScreenshotPath + getName, screenshotBuffer.EncodeToPNG());

                RenderTexture.active = null;
                buff.Release();

                DestroyImmediate(screenshotBuffer);
                mainComponent.Settings.TakeScreenshot = false;
            }

            // Effect is disabled or we don't use custom downsample shader
            if (!useShader || multiplier == 1f)
            {
                Graphics.Blit(main.targetTexture, destination);
            }
            else // Setup the custom downsampler and output
            {
                material_current.SetFloat("_ResizeWidth", Screen.width);
                material_current.SetFloat("_ResizeHeight", Screen.height);
                material_current.SetFloat("_Sharpness", sharpness);
                material_current.SetFloat("_SampleDistance", sampleDistance);
                Graphics.Blit(main.targetTexture, destination, material_current, 0);
            }
        }
        private string getName // generate a string for the filename of the screenshot
        {
            get
            {
                return mainComponent.Settings.NamePrefix + "_" +
                    DateTime.Now.Year.ToString() +
                    DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + "_" +
                    DateTime.Now.Hour.ToString() +
                    DateTime.Now.Minute.ToString() +
                    DateTime.Now.Second.ToString() +
                    DateTime.Now.Millisecond.ToString() + "_" +
                    mainComponent.Settings.OutputResolution.y.ToString() + "p.png";
            }
        }
    }
}