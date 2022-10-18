// Based on Tarodev Game Manager tutorial
// https://www.youtube.com/watch?v=4I0vonyqMi8
//
// Note: There is a lot of controversy in the comments whether this is the best
// way to implement a GameManager. Tarodev is convinced this system is fine for
// SoloDev or small teams. Here we are just testing it out to try a new system,
// learn something about Singletons and enums, and see if it makes sense to us. 
//
// Additional Note: Tarodev mentions in the comments..
//
// Think of GameManager as an orchestrator. It's the only place to further the game to the next step
// (by game I mean actual gameplay, not main menu, splash screen etc).
//
// && This video is a manager to control the flow of the actual game and has no overlap to his guide.
// You would absolutely not want to put this on an object which persists through every scene including
// menus and the like, you'd want a brand new instance of it each new game.
//
// In this Game Jame game we have used Game Manager exactly the way he says NOT to do it. 
// Does it work? So far a highly qualified.. "Yeeeessss..??"
// However, it's probably more aimed towards the Turn Based system he outlined where it manages a single 
// "game" that you are playing rather than managing the whole GameSystem.
// Terminology is a b*tch.


using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState _gameState;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //This will only work for the first level
        UpdateGameState(GameState.StartGame);
        //Need to either eliminate this or figure out a way to use DontDestroyOnLoad
        //https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Object.DontDestroyOnLoad.html
    }

    public void UpdateGameState(GameState newGameState)
    {
        _gameState = newGameState;

        switch (newGameState)
        {
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.LoadLevel:
                HandleLoadLevel();
                break;
            case GameState.StartLevel:
                HandleStartLevel();
                break;
            case GameState.PlayLevel:
                HandlePlayLevel();
                break;
            case GameState.RestartLevel:
                HandleRestartLevel();
                break;
            case GameState.CompleteLevel:
                HandleCompleteLevel();
                break;
            case GameState.EndGame:
                HandleEndGame();
                break;
        }

        OnGameStateChanged?.Invoke(newGameState);
        // Tarodev explains why to use >> ?.Invoke <<
        // It prevents an "unsubscribed error"
        // Whatever that means (O_o)
    }

    private void HandleStartGame()
    {
        // Handled by MenuManager
        // MenuManager subscribes to public static event OnGameStateChanged
        // StartGame called from Play Button
    }

    private void HandleStartLevel()
    {
    }

    private void HandleLoadLevel()
    {
    }

    private void HandlePlayLevel()
    {
    }

    private void HandleRestartLevel()
    {
    }

    private void HandleCompleteLevel()
    {
    }

    private void HandleEndGame()
    {
    }
}


// [From Video comments] Declaring enum outside the GameManager class because..
// "when referencing the enum outside of the Game Manager class we don't have to do
// GameManager.GameState.State, we can just do GameState.State. No other reason"

public enum GameState
{
    StartGame,
    LoadLevel,
    StartLevel,
    PlayLevel,
    RestartLevel,
    CompleteLevel,
    EndGame
}