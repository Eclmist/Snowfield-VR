using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InGameMenuPrefabs
{
    public GameObject characterScreen;
    public GameObject settingsScreen;
}



public class InGameMenuArea : VR_Button {
    [SerializeField] private InGameMenuPrefabs prefabs;

    private void Awake()
    {
        if (InGameUI.Instance.GetLastState() != InGameState.PAUSE)
        {

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	
    protected override void OnApplicationMenuPress()
    {
        InGameUI.Instance.SetGameState(InGameState.INGAME);
        base.OnApplicationMenuPress();
        Destroy(this.gameObject);
    }

    public void CharacterActivation(bool active)
    {
        prefabs.characterScreen.SetActive(active);
        if (active)
            InGameUI.Instance.SetGameMenuState(InGamePause.CHARACTER);
    }

    public void SettingsActivation(bool active)
    {
        prefabs.settingsScreen.SetActive(active);
        if (active)
            InGameUI.Instance.SetGameMenuState(InGamePause.SETTINGS);
    }

    
    

}
