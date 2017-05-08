using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace MadGoat_SSAA
{
    [CustomEditor(typeof(MadGoatSSAA))]
    public class MadGoatSSAA_Editor : Editor
    {
        SerializedObject serObj;
        SerializedProperty Mode;
        SerializedProperty FilterType;
        SerializedProperty Sharpness;
        SerializedProperty sampleDistance;
        SerializedProperty Multiplier;
        SerializedProperty UseShader;

        SerializedProperty SSAA_HALF;
        SerializedProperty SSAA_X2;
        SerializedProperty SSAA_X4;
        private string[] ssaaModes = new string[] { "Off", "0.5x", "2x", "4x" };
        ScreenshotSettings settings = new ScreenshotSettings();
        int tab;
        private bool Extend;
        private int scale;
        private int mode;
        void OnEnable()
        {
            serObj = new SerializedObject(target);
            Mode = serObj.FindProperty("renderMode");
            FilterType = serObj.FindProperty("filterType");
            Sharpness = serObj.FindProperty("sharpness");
            sampleDistance = serObj.FindProperty("sampleDistance");
            Multiplier = serObj.FindProperty("multiplier");
            UseShader = serObj.FindProperty("useShader");

            SSAA_HALF = serObj.FindProperty("SSAA_HALF");
            SSAA_X2 = serObj.FindProperty("SSAA_X2");
            SSAA_X4 = serObj.FindProperty("SSAA_X4");
        }
        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serObj.Update();

            GUIStyle s = new GUIStyle();
            s.normal.textColor = new Color(0.5f, 0.1f, 0.1f);
            s.fontSize = 16;
            EditorGUILayout.Separator();
            GUILayout.Label("MadGoat SuperSampling", s);
            EditorGUILayout.Separator();
            tab = GUILayout.Toolbar(tab, new string[] { "SSAA", "Screenshot" });
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            switch (tab)
            {
                case 0:
                    EditorGUILayout.PropertyField(Mode, new GUIContent("Operation mode"), true);
                    if (Mode.intValue == 1) // Resolution scale
                    {
                        EditorGUILayout.HelpBox("Rise or lower the render resolution by percent", MessageType.Info);
                        Multiplier.floatValue = EditorGUILayout.Slider("Resolution Scale (%)", Multiplier.floatValue * 100f, 50, 200) / 100f;
                    }
                    else if (Mode.intValue == 0) // SSAA presets
                    {
                        mode = getmode();
                        EditorGUILayout.HelpBox("Conventional SSAA settings. Higher settings produces better quality at the cost of performance. x0.5 boosts the performance, but halvens the resolution.", MessageType.Info);
                        mode = EditorGUILayout.Popup("SSAA Mode", mode, ssaaModes);
                        switch (mode)
                        {
                            case 0: // off
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_OFF);
                                break;
                            case 1: // x0.5
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_HALF);
                                break;
                            case 2: // x2
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_X2);
                                break;
                            case 3: // x4
                                (target as MadGoatSSAA).SetAsSSAA(SSAAMode.SSAA_X4);
                                break;
                        }
                        EditorGUILayout.Separator();
                        s.fontSize = 12;
                        GUILayout.Label("Edit SSAA Presets", s);
                        EditorGUILayout.Separator();
                        EditorGUILayout.PropertyField(SSAA_HALF, new GUIContent("SSAA x0.5"), true);
                        EditorGUILayout.PropertyField(SSAA_X2, new GUIContent("SSAA x2"), true);
                        EditorGUILayout.PropertyField(SSAA_X4, new GUIContent("SSAA x4"), true);

                        if (GUILayout.Button("Reset SSAA preset to defaults"))
                        {
                            // Reset
                            (target as MadGoatSSAA).SSAA_X2 = new SsaaProfile(1.5f, true, Filter.BILINEAR, 0.8f, 0.5f);
                            (target as MadGoatSSAA).SSAA_X4 = new SsaaProfile(2f, true, Filter.BICUBIC, 0.725f, .95f);
                            (target as MadGoatSSAA).SSAA_HALF = new SsaaProfile(.5f, false);
                        }
                    }
                    else // Custom 
                    {
                        EditorGUILayout.HelpBox("Experimental. Only use if you know what you're doing.\nValues over 4 not recommended, higher values (depending on current screen size) may cause system instability or engine crashes.", MessageType.Warning);

                        Extend = EditorGUILayout.Toggle("Don't limit the multiplier", Extend);
                        if (Extend) EditorGUILayout.PropertyField(Multiplier, new GUIContent("Resolution Multiplier"), true);
                        else Multiplier.floatValue = EditorGUILayout.Slider("Resolution Multiplier", Multiplier.floatValue, 0.2f, 4f);
                    }
                    // Draw the shader stuff
                    if (Mode.intValue != 0)
                    {
                        EditorGUILayout.Separator();
                        s.fontSize = 12;
                        GUILayout.Label("Downsampling", s);
                        EditorGUILayout.Separator();

                        EditorGUILayout.HelpBox("If chosen to use shader based downsampling, the render image will be passed through a custom shader. If not, it will be resized as is.", MessageType.Info);
                        UseShader.boolValue = EditorGUILayout.Toggle("Use downsampling shader", UseShader.boolValue);
                        if (UseShader.boolValue)
                        {
                            EditorGUILayout.PropertyField(FilterType);
                            Sharpness.floatValue = EditorGUILayout.Slider("Downsample Sharpness", Sharpness.floatValue, 0f, 1f);
                            sampleDistance.floatValue = EditorGUILayout.Slider("Distance between samples", sampleDistance.floatValue, 0.5f, 2f);
                        }
                    }
                    break;
                case 1:
                    // the screenshot module
                    settings.OutputResolution = EditorGUILayout.Vector2Field("Screenshot Resolution", settings.OutputResolution);
                    settings.ScreenshotMultiplier = EditorGUILayout.IntSlider("Render Resolution Multiplier", settings.ScreenshotMultiplier, 1, 4);
                    settings.ScreenshotPath = EditorGUILayout.TextField("Save path", settings.ScreenshotPath);
                    settings.NamePrefix = EditorGUILayout.TextField("File Name Prefix", settings.NamePrefix);
                    settings.UseShader = EditorGUILayout.Toggle("Use downsampling shader", settings.UseShader);
                    if (settings.UseShader)
                    {
                        settings.Sharpness = EditorGUILayout.Slider("   Sharpness", settings.Sharpness, 0, 1);
                        //settings.SampleDistance = EditorGUILayout.Slider("   Sample Distance", settings.SampleDistance, 0, 2);
                    }
                    if (GUILayout.Button(Application.isPlaying ? "Take Screenshot" : "Only available in play mode"))
                    {
                        if (Application.isPlaying)
                            (target as MadGoatSSAA).TakeScreenshot(
                                settings.ScreenshotPath,
                                settings.OutputResolution,
                                settings.ScreenshotMultiplier,
                                settings.Sharpness
                                );
                    }
                    break;
            }
            s.fontSize = 8;
            EditorGUILayout.Separator();
            GUILayout.Label("Version: " + MadGoatSSAA_Utils.ssaa_version, s);
            // Apply modifications
            serObj.ApplyModifiedProperties();
        }
        private int getmode()
        {
            return (int)(target as MadGoatSSAA).ssaaMode;
        }
    }
}