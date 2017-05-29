using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MainMenuPrefab
{
    public GameObject Settings;
    public GameObject Credits;
    public GameObject MainMenuText;
}

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private MainMenuPrefab prefabs;
    public static MainMenuManager Instance;
    private Text txt;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Overlord.");
            Destroy(this);
        }
    }
    // Use this for initialization
    void Start()
    {
        txt = prefabs.MainMenuText.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActiveDeactiveSettings()
    {
        if (!prefabs.Settings.activeInHierarchy && MainMenu.Instance.GetLastState() == MainMenuState.SETTINGS)
            prefabs.Settings.SetActive(true);
        else if (MainMenu.Instance.GetLastState() != MainMenuState.SETTINGS && prefabs.Settings.activeInHierarchy)
            prefabs.Settings.SetActive(false);
    }

    public void ActiveDeactiveCredits()
    {
        if (!prefabs.Settings.activeInHierarchy && MainMenu.Instance.GetLastState() == MainMenuState.CREDITS)
            prefabs.Credits.SetActive(true);
        else if (MainMenu.Instance.GetLastState() != MainMenuState.CREDITS && prefabs.Settings.activeInHierarchy)
            prefabs.Credits.SetActive(false);
    }

    public void TextChange(string newTxt)
    {
        txt.text = newTxt;
    }

    public void Left()
    {
        Debug.Log("Left");
        if (MainMenu.Instance.GetLLState() != MainMenuState.IDLE)
        {
            MainMenu.Instance.SetState(MainMenu.Instance.GetLLState());
        }
        else
        {

        }
    }

    public void Right()
    {
        Debug.Log("Right");
        if (MainMenu.Instance.GetNextState() != MainMenuState.IDLE)
        {
            MainMenu.Instance.SetState(MainMenu.Instance.GetNextState());
            Debug.Log("GoingRight");
        }
        else
        {

        }
    }

    public void MainMenuSelect()
    {
        Debug.Log("MainMenu");
        if (MainMenu.Instance.GetLastState() != MainMenuState.IDLE)
        {
            Debug.Log("SceneChange");
            switch (MainMenu.Instance.GetLastState())
            {
                case MainMenuState.CONTINUE:
                    Debug.Log("Continue");
                    break;
                case MainMenuState.NEW_GAME:
                    Debug.Log("NewGame");
                    break;
                case MainMenuState.SETTINGS:
                    Debug.Log("Settings");
                    break;
                case MainMenuState.CREDITS:
                    Debug.Log("Credits");
                    break;
                case MainMenuState.QUIT:
                    Debug.Log("Quit");
                    break;
                default:
                    Debug.Log("Nothing");
                    break;
            }
        }

    }
}
