using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class Glitch2 : MonoBehaviour
{
    float glitchup;
    float glitchdown;

    float glitchupTime = 0.05f;
    float glitchdownTime = 0.05f;

    public Texture2D displacementMap;

    [Range(0.0f, 1.0f)]
    public float _intensity = 0.5f;
    [Range(0.0f, 10.0f)]
    public float _amplitude = 2.0f;

    Camera cam;

    private Shader glitchShader = null;
    private Material glitchMaterial = null;
    bool isSupported = true;

    void Start()
    {
        CheckResources();
    }

    public bool CheckResources()
    {
        glitchShader = Shader.Find("MyShaders/Glitch");
        glitchMaterial = CheckShader(glitchShader, glitchMaterial);

        return isSupported;
    }

    protected Material CheckShader(Shader s, Material m)
    {
        if (s == null)
        {
            Debug.Log("Missing shader on " + ToString());
            this.enabled = false;
            return null;
        }

        if (s.isSupported == false)
        {
            Debug.Log("The shader " + s.ToString() + " is not supported on this platform");
            this.enabled = false;
            return null;
        }

        cam = GetComponent<Camera>();
        cam.renderingPath = RenderingPath.UsePlayerSettings;

        m = new Material(s);
        m.hideFlags = HideFlags.DontSave;

        if (s.isSupported && m && m.shader == s)
            return m;

        return m;
    }

    void OnDestroy()
    {
#if UNITY_EDITOR
        DestroyImmediate(glitchMaterial);
#else
        Destroy(glitchMaterial);
#endif
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float _aberration = Random.value / 10f;

        glitchup += Time.deltaTime * _intensity;
        glitchdown += Time.deltaTime * _intensity;

        glitchMaterial.SetFloat("_intensity", _intensity);
        glitchMaterial.SetTexture("_dispTex", displacementMap);

		if (glitchup > glitchupTime)
		{
			if (Random.value < 0.1f * _intensity)
				glitchMaterial.SetFloat("flip_up", Random.Range(0, 1f));
			else
				glitchMaterial.SetFloat("flip_up", 0);

			glitchup = 0;
			glitchupTime = Random.value / 10f;
		}

		if (glitchdown > glitchdownTime)
		{
			if (Random.value < 0.1f * _intensity)
				glitchMaterial.SetFloat("flip_down", 1 - Random.Range(0, 1f));
			else
				glitchMaterial.SetFloat("flip_down", 1);


			glitchdown = 0;
			glitchdownTime = Random.value / 10f;
		}

		if (Random.value < 0.05 * _intensity)
        {
            glitchMaterial.SetFloat("displace", Random.value * _intensity);
            glitchMaterial.SetFloat("scale", 1 - Random.value * _intensity);
            glitchMaterial.SetFloat("_aberration", _aberration * _amplitude * _intensity);
        }
        else
            glitchMaterial.SetFloat("displace", 0);

        Graphics.Blit(source, destination, glitchMaterial);
    }
}