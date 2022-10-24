using System;
using UnityEngine;

public class MetaUI : MonoBehaviour
{

    public static MetaUI Instance;

    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject LoadingScreenUI;
    [SerializeField] private GameObject RestartLevelUI;
    [SerializeField] private GameObject CompleteLevelUI;
    [SerializeField] private GameObject QuitMenuUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            // Should probably only have one DDOL in your application.
            // In our case that would be MetaManager.
            // However we are using MetaUI as a persistent MainMenu system across all scenes
            // so we'll leave it as DDOL for now.
        }
        else
        {
            Destroy(gameObject);
        }

        MetaManager.OnMetaStateChanged += MetaManager_OnMetaStateChanged;
    }

    private void OnDestroy()
    {
        MetaManager.OnMetaStateChanged -= MetaManager_OnMetaStateChanged;
    }

    private void MetaManager_OnMetaStateChanged(MetaState state)
    {
        MainMenuUI.SetActive(state == MetaState.MainMenu);
        LoadingScreenUI.SetActive(state == MetaState.LoadScene);
        RestartLevelUI.SetActive(state == MetaState.RestartLevel);
        CompleteLevelUI.SetActive(state == MetaState.CompleteLevel);
        QuitMenuUI.SetActive(state == MetaState.QuitMenu);
    }

}


// ==NOTES==
//
// Each UI will only be SetActive IF current MetaState == MetaState.XYZ
//
// Heavily modified code based on Tarodev Game Manager tutorial
// https://www.youtube.com/watch?v=4I0vonyqMi8
//
// Trialling new system using..
// MetaManager for Start, Loading & EndGame
// GameManager for StartLevel, PlayLevel, RestartLevel, CompleteLevel
// 
// Will also set up corresponding
// MetaMenuManager | MetaMenuManagerUI | MetaManager
// GameMenuManager | GameMenuManagerUI | GameManager
//
// See MetaManager & GameManager for additional notes.
//


// OR one line option..
// [SerializeField] private GameObject StartGameUI, LoadingScreenUI, EndGameUI;


// Tarodev: >>  +=  <<   is how you subscribe to Events in C#
// Tarodev: >>   -=  << is how you unsubscribe from Events in C#
// It's good practice to unsubscribe on destroy


// SetActive requires a bool so above act like IF statements eg
// If state == MetaState.StartGame
// Set StartMenuUI to Active 
// Else leave it inactive