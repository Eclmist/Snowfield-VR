using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InGameMenuPrefabs
{
    public GameObject characterScreen;
    public GameObject settingsScreen;
    public GameObject quitPrompt;
    public GameObject messageBox;
    public GameObject inventory;
}


public class InGameMenuArea : MonoBehaviour
{
    [SerializeField]
    private InGameMenuPrefabs prefabs;
    [SerializeField]
    private Animator gameMain;

    private void Start()
    {
        gameMain = GetComponent<Animator>();
    }

    public void CharacterActivation(bool active)
    {
        if (prefabs.characterScreen != null)
            prefabs.characterScreen.SetActive(active);
    }

    public void SettingsActivation(bool active)
    {
        if (prefabs.settingsScreen != null)
            prefabs.settingsScreen.SetActive(active);
    }

    public void QuitGamePrompt(bool active)
    { 
        if(prefabs.quitPrompt != null)
            prefabs.quitPrompt.SetActive(active);
    }

    public void MessageBoxActivation(bool active)
    {
        if (prefabs.messageBox != null)
            prefabs.messageBox.SetActive(active);
    }

    public void InventoryActivation(bool active)
    {
        if (prefabs.inventory != null)
            prefabs.inventory.SetActive(active);
    }

    public void Animation(int t)
    {
        gameMain.SetBool("Open", false);
        StartCoroutine(WaitForSeconds(t));
    }

    IEnumerator WaitForSeconds(int t)
    {
        yield return (t);
        gameMain.SetBool("Open", true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
