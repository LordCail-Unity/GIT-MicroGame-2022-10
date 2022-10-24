using System;
using UnityEngine;

public class GameUI : MonoBehaviour
{

    [SerializeField] private GameObject StartLevelUI;
    [SerializeField] private GameObject InGameHUDUI;
    [SerializeField] private GameObject PauseMenuUI;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        // Each UI will only be SetActive IF current GameState == GameState.XYZ
        StartLevelUI.SetActive(state == GameState.StartLevel);
        InGameHUDUI.SetActive(state == GameState.PlayLevel);
        PauseMenuUI.SetActive(state == GameState.PauseLevel);
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
// [SerializeField] private GameObject StartLevelUI, PlayLevelUI, RestartLevelUI, CompleteLevelUI;


// Tarodev: >>  +=  <<   is how you subscribe to Events in C#
// Tarodev: >>   -=  << is how you unsubscribe from Events in C#
// It's good practice to unsubscribe on destroy


// SetActive requires a bool so above act like IF statements eg
// If state == GameState.StartGame
// Set StartMenuUI to Active 
// Else leave it inactive
