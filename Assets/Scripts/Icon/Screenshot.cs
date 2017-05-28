using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Screenshot : MonoBehaviour {

    [SerializeField]
    protected int imageResolution;

    protected Camera camera;

    // Update is called once per frame
    void Start() {   
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        //-----------------------------------------------------------------------------Screenshot-------------------------------------------------------------------------------------------//
        if (Input.GetKeyDown(KeyCode.K))
        {
            RenderTexture rt = new RenderTexture(imageResolution, imageResolution, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(imageResolution, imageResolution, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, imageResolution, imageResolution), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(imageResolution, imageResolution);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/Icon/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
