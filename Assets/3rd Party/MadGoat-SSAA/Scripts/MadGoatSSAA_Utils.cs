using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadGoat_SSAA
{

    public enum Mode
    {
        SSAA = 0,
        ResolutionScale = 1,
        Custom = 2
    }
    public enum SSAAMode
    {
        SSAA_OFF = 0,
        SSAA_HALF = 1,
        SSAA_X2 = 2,
        SSAA_X4 = 3
    }
    public enum Filter
    {
        NEAREST_NEIGHBOR,
        BILINEAR,
        BICUBIC
    }

    [System.Serializable]
    public class SsaaProfile
    {
        [HideInInspector] 
        public float multiplier;

        public bool useShader;
        [Tooltip("Which type of filtering to be used (only applied if useShader is true)")]
        public Filter FilterType = Filter.BILINEAR;
        [Tooltip("The sharpness of the filtered image (only applied if useShader is true)")]
        [Range(0,1)]
        public float sharpness;
        [Tooltip("The distance between the samples (only applied if useShader is true)")]
        [Range(0.5f,2f)]
        public float sampleDistance;

        public SsaaProfile(float mul, bool useDownsampling)
        {
            multiplier = mul;

            useShader = useDownsampling;
            sharpness = useDownsampling ? 0.85f : 0;
            sampleDistance = useDownsampling ? 0.65f : 0;
        }
        public SsaaProfile(float mul, bool useDownsampling, Filter filterType, float sharp, float sampleDist)
        {
            multiplier = mul;

            FilterType = filterType;
            useShader = useDownsampling;
            sharpness = useDownsampling ? sharp : 0;
            sampleDistance = useDownsampling ? sampleDist : 0;
        }
    }
    [System.Serializable]
    public class ScreenshotSettings
    {
        [HideInInspector]
        public bool TakeScreenshot = false;
        [Range(1,4)]
        public int ScreenshotMultiplier = 1;
        public string ScreenshotPath = "Assets/SuperSampledSceenshots/";
        public string NamePrefix = "GameName";
        public Vector2 OutputResolution = new Vector2(1920, 1080);

        public bool UseShader = true;
        [Range(0, 1)]
        public float Sharpness = 0.85f;
    }

    public static class MadGoatSSAA_Utils
    {
        public const string ssaa_version = "1.0"; // Don't forget to change me when pushing updates!

        /// <summary>
        /// Makes this camera's settings match the other camera and assigns a custom target texture
        /// </summary>
        public static void CopyFrom(this Camera current, Camera other, RenderTexture rt)
        {
            current.CopyFrom(other);
            current.targetTexture = rt;
        }
    }
}