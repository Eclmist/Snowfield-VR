using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct InGameMenuPrefabs
{
    public GameObject loadScreen;
    public GameObject settingsScreen;
    public GameObject quitPrompt;
    public GameObject mailScreen;
    public Animator buttons;
}



public class InGameMenuArea : MonoBehaviour
{
    [SerializeField]
    private InGameMenuPrefabs prefabs;

    //public void CharacterActivation(bool active)
    //{
    //    if (prefabs.characterScreen != null)
    //        prefabs.characterScreen.SetActive(active);
    //}

    public void SaveGame()
    {
        SaveManager.Instance.Save();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SettingsActivation(bool active)
    {
        if (prefabs.settingsScreen != null)
            prefabs.settingsScreen.SetActive(active);
    }

    public void LoadPrompt(bool active)
    {
        if (prefabs.settingsScreen != null)
            prefabs.loadScreen.SetActive(active);
    }


    public void QuitGamePrompt(bool active)
    { 
        if(prefabs.quitPrompt != null)
            prefabs.quitPrompt.SetActive(active);
    }

    public void MailActivation(bool active)
    {
        if (prefabs.mailScreen != null)
		{

			prefabs.mailScreen.SetActive(active);
            prefabs.mailScreen.GetComponent<Animator>().SetBool("Activate", active);
			if(active)
				MessageManager.Instance.DisplayMailInterface();
		}
    }

    public void Deactivate()
    {
        LoadPrompt(false);
        SettingsActivation(false);
        QuitGamePrompt(false);
        MailActivation(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InGameAppear(bool i)
    {
        prefabs.buttons.SetBool("Activate", i);
    }
}
