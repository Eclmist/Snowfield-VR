using UnityEngine;

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

    public Shader glitchShader = null;
    private Material glitchMaterial = null;

    void Start()
    {
		glitchMaterial = new Material(glitchShader);
		glitchMaterial.SetFloat("flip_up", 0);

		glitchMaterial.SetFloat("flip_down", 1);

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
				glitchMaterial.SetFloat("flip_up", Random.Range(0, 0.3f));
			else
				glitchMaterial.SetFloat("flip_up", 0);

			glitchup = 0;
			glitchupTime = Random.value / 10f;
		}

		if (glitchdown > glitchdownTime)
		{
			if (Random.value < 0.1f * _intensity)
				glitchMaterial.SetFloat("flip_down", 1 - Random.Range(0, 0.3f));
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
		{
			glitchMaterial.SetFloat("displace", 0);
		}

        Graphics.Blit(source, destination, glitchMaterial);
    }
}