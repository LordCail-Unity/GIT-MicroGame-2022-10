using System; // Required for:  public static event Action<>
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState _gameState;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.StartLevel);
    }

    public void UpdateGameState(GameState newGameState)
    {
        _gameState = newGameState;

        switch (newGameState)
        {
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
        }
        OnGameStateChanged?.Invoke(newGameState);
    }

    private void HandleStartLevel()
    {
        // I think updating the GameState makes most sense inside GameManager..
        // GameManager.Instance.UpdateGameState(GameState.PlayLevel); 
        // Update GameState >> PlayLevel
    }

    private void HandlePlayLevel()
    {
        // --COLLISION--
        // IF COLLISION = OBJECT OR
        // IF TRIGGER = KILLZONE
        // THEN Update GameState >> RestartLevel
        // GameManager.Instance.UpdateGameState(GameState.RestartLevel); 
        // 
        // --COLLISION--
        // IF TRIGGER = FINISH
        // THEN Update GameState >> CompleteLevel
        // GameManager.Instance.UpdateGameState(GameState.CompleteLevel); 
    }

    private void HandleRestartLevel()
    {
        // On Restart Level activated:
        // --PLAYER CTRL-- Disable player controller
        // --MENU MGR-- Pause before activating RESTART screen
        // --MENU MGR-- Show RESTART screen
        // Click to
        // (1) Retry (eg to beat high score) OR
        // (2) Main Menu
        // Hand back to MetaManager to destroy this level / scene

        FindObjectOfType<PlayerController>().enabled = false;
        // GameManager can disable PlayerController 

    }

    private void HandleCompleteLevel()
    {
        // On Complete Level activated:
        // --PLAYER CTRL-- Disable player controller
        // --MENU MGR-- Pause before activating WIN screen
        // --MENU MGR-- Show WIN screen
        // Click to
        // (1) Next Level OR 
        // (2) Retry (eg to beat high score) OR
        // (3) Main Menu
        // Hand back to MetaManager to destroy this level / scene

        FindObjectOfType<PlayerController>().enabled = false;
        // PlayerController can't disable itself but GameManager can 

    }

}

public enum GameState
{
    StartLevel,
    PlayLevel,
    RestartLevel,
    CompleteLevel
}



// ==NOTES==
//
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


// Original version was just Instance = this;
// Do we need this more comprehensive Awake code?


// In his video, Tarodev explains why to use >> ?.Invoke <<
// It prevents an "unsubscribed error"
// Whatever that means (O_o)


// [From Video comments] Declaring enum outside the GameManager class because..
// "when referencing the enum outside of the Game Manager class we don't have to do
// GameManager.GameState.State, we can just do GameState.State. No other reason"