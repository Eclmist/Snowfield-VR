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
        prefabs.characterScreen.SetActive(active);
    }

    public void SettingsActivation(bool active)
    {
        prefabs.settingsScreen.SetActive(active);
    }

    public void QuitGame(bool active)
    {
        prefabs.quitPrompt.SetActive(active);
    }
}
