using System;
using UnityEngine;

public class MetaMenuManager : MonoBehaviour
{

    public static MetaMenuManager Instance;

    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject LoadingScreenUI;
    [SerializeField] private GameObject QuitApplicationUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        // Each UI will only be SetActive IF current MetaState == MetaState.XYZ

        MainMenuUI.SetActive(state == MetaState.InitializeApplication);
        LoadingScreenUI.SetActive(state == MetaState.LoadScene);
        QuitApplicationUI.SetActive(state == MetaState.QuitApplication);
    }

}


// ==NOTES==
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