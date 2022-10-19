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

using System;
using UnityEngine;

public class MetaMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject StartGameUI;
    [SerializeField] private GameObject LoadingScreenUI;
    [SerializeField] private GameObject EndGameUI;

    // OR one line option..
    // [SerializeField] private GameObject StartGameUI, LoadingScreenUI, EndGameUI;

    private void Awake()
    {
        MetaManager.OnMetaStateChanged += MetaManager_OnMetaStateChanged;
        // Tarodev: >>  +=  <<   is how you subscribe to Events in C#
    }

    private void OnDestroy()
    {
        MetaManager.OnMetaStateChanged -= MetaManager_OnMetaStateChanged;
        // Tarodev: >>   -=  << is how you unsubscribe from Events in C#
        // It's good practice to unsubscribe on destroy
    }

    private void MetaManager_OnMetaStateChanged(MetaState state)
    {
        StartGameUI.SetActive(state == MetaState.StartGame);
        LoadingScreenUI.SetActive(state == MetaState.LoadLevel);
        EndGameUI.SetActive(state == MetaState.EndGame);
    }

    // SetActive requires a bool so above act like IF statements eg
    // If state == MetaState.StartGame
    // Set StartMenuUI to Active 
    // Else leave it inactive

}