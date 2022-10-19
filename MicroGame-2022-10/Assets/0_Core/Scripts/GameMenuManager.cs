// Heavily modified code based on Tarodev Game Manager tutorial
// https://www.youtube.com/watch?v=4I0vonyqMi8
//
// Trialling new system using..
// MetaManager for Start, Loading & EndGame
// GameManager for StartLevel, PlayLevel, RestartLevel, CompleteLevel
// 
// Will also set up corresponding
// MetaMenuManager | MetaMenuManagerUI
// GameMenuManager | GameMenuManagerUI
//
// See MetaManager & GameManager for additional notes.
//

using System;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject StartGameUI;
    [SerializeField] private GameObject LoadingScreenUI;
    [SerializeField] private GameObject StartLevelUI;
    [SerializeField] private GameObject PlayLevelUI;
    [SerializeField] private GameObject RestartLevelUI;
    [SerializeField] private GameObject CompleteLevelUI;
    [SerializeField] private GameObject EndGameUI;

    // OR [SerializeField] private GameObject StartGameUI, LoadingScreenUI, PlayLevelUI, RestartLevelUI, CompleteLevelUI, EndGameUI;


    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        // Tarodev: >>  +=  <<   is how you subscribe to Events in C#
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        // Tarodev: >>   -=  << is how you unsubscribe from Events in C#
        // It's good practice to unsubscribe on destroy
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        StartGameUI.SetActive(state == GameState.StartGame);
        LoadingScreenUI.SetActive(state == GameState.LoadLevel);
        StartLevelUI.SetActive(state == GameState.StartLevel);
        PlayLevelUI.SetActive(state == GameState.PlayLevel);
        RestartLevelUI.SetActive(state == GameState.RestartLevel);
        CompleteLevelUI.SetActive(state == GameState.CompleteLevel);
        EndGameUI.SetActive(state == GameState.EndGame);
    }

    // SetActive requires a bool so above act like IF statements eg
    // If state == GameState.StartGame
    // Set StartMenuUI to Active 
    // Else leave it inactive

}