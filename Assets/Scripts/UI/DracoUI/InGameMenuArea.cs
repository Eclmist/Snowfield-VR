using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InGameMenuPrefabs
{
    public GameObject characterScreen;
    public GameObject settingsScreen;
    public GameObject quitPrompt;
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

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
