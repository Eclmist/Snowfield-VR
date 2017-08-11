using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InGameMenuPrefabs
{
    public GameObject characterScreen;
    public GameObject settingsScreen;
    public GameObject quitPrompt;
    public GameObject mailScreen;
}



public class InGameMenuArea : MonoBehaviour
{
    [SerializeField]
    private InGameMenuPrefabs prefabs;
   
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

    public void MailActivation(bool active)
    {
        if (prefabs.mailScreen != null)
            prefabs.mailScreen.SetActive(active);
    }

    public void Deactivate()
    {
        CharacterActivation(false);
        SettingsActivation(false);
        QuitGamePrompt(false);
        MailActivation(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
