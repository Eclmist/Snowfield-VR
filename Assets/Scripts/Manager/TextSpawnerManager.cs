using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSpawnerManager : MonoBehaviour {

    [SerializeField]
    private SpawnedText textPrefab;
    [SerializeField]
    private Vector3 offset = new Vector3(0f,0.2f,0f);


    public static TextSpawnerManager Instance;

    void Awake()
    {
        Instance = this;
    }
	
    // Generates a text that floats above the given transform and fades away
    public void SpawnText(string text, Color color, Transform t)
    {
        Collider c = t.GetComponent<Collider>();
        SpawnedText st;

        if(c)
            st = Instantiate(textPrefab, (t.position + t.up * (c.bounds.size.y) + offset), t.rotation);
        else
            st = Instantiate(textPrefab, (t.position + t.up * + offset.y), t.rotation);

        st.SetText(text);
        st.SetColor(color);
        Destroy(st.gameObject, st.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0).Length);

    }


    public void SpawnText(string text, Color color, Transform t, float scale)
    {
        Collider c = t.GetComponent<Collider>();
        SpawnedText st;

        if (c)
            st = Instantiate(textPrefab, (t.position + t.up * (c.bounds.size.y) + offset), t.rotation);
        else
            st = Instantiate(textPrefab, (t.position + t.up * +offset.y), t.rotation);

        st.transform.localScale *= scale;
        st.SetText(text);
        st.SetColor(color);
        Destroy(st.gameObject, st.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0).Length);

    }


    // Generates a text that stays at a position 
    public void SpawnSoundEffect(string text, Color color, Transform t, Vector3 offset, float duration, float scale = 1)
    {

        
        SpawnedText st = Instantiate(textPrefab,t.position + offset,Quaternion.identity);
        

        st.transform.localScale *= scale;
        st.SetText(text);
        st.SetColor(color);
        st.GetComponentInChildren<Animator>().enabled = false;
        st.GetComponentInChildren<Outline>().enabled = true;
        StartCoroutine(Shake(st.gameObject, duration, 0.07f));

    }


    private IEnumerator Shake(GameObject g, float shakeDuration, float shakeIntensity)
    {

        float timeElapsed = 0.0f;

        Vector3 originalPos = g.transform.position;

        while (timeElapsed < shakeDuration)
        {

            timeElapsed += Time.deltaTime;


            // As the shake approaches the end, the smooth value increases
            // The smooth value will be multiplied to the shake intensity
            // This will slowly take away the shake intensty over time so it doesn't look like it snaps
            float percentComplete = timeElapsed / shakeDuration;
            float smooth = 1.0f - Mathf.Clamp(percentComplete, 0.0f, 1.0f);

            // Multiplying by ( *2 - 1 ) gets a range from -1 to 1
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;

            x *= shakeIntensity * smooth;
            y *= shakeIntensity * smooth;

            g.transform.position = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);


            yield return new WaitForEndOfFrame();
        }

        g.transform.position = originalPos;
        Destroy(g);
    }



}
