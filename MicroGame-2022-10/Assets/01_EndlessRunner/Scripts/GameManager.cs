using System; // Required for:  public static event Action<>
using System.Collections; // Required for IEnumerator Coroutines
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState _gameState;

    private PlayerController _playerController;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Debug.Log("GameManager Awake()");

        if (Instance == null)
        { Instance = this; }
        else { Destroy(gameObject); }

        UpdateGameState(GameState.LoadingIn);
        Debug.Log("GameManager: GameState = " + _gameState);
    }

    private void Start()
    {    
        Debug.Log("GameManager Start()");
        _playerController = FindObjectOfType<PlayerController>();

        if (_playerController == null)
        { Debug.Log("GameManager: Can't find PlayerController"); }
        else { Debug.Log("GameManager: PlayerController locked and loaded"); }

        DisablePlayerMovement();
        Debug.Log("GameManager: PlayerController disabled");

        UpdateGameState(GameState.StartLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key was pressed");

            if (_gameState == GameState.PlayLevel)
            { UpdateGameState(GameState.PauseLevel); }
            else { UpdateGameState(GameState.PlayLevel); }

            // TO DO
            // What about ESC during Restart or Complete Level?

        }
    }

    public void UpdateGameState(GameState newGameState)
    {
        _gameState = newGameState;

        switch (newGameState)
        {
            case GameState.LoadingIn:
                HandleLoadingIn();
                break;
            case GameState.StartLevel:
                HandleStartLevel();
                break;
            case GameState.PlayLevel:
                HandlePlayLevel();
                break;
            case GameState.PauseLevel:
                HandlePauseLevel();
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

    private void HandleLoadingIn()
    {
        // Wait for MetaManager to finish loading processes
        // MetaManager will then trigger StartLevel
    }

    private void HandleStartLevel()
    {
        StartCoroutine(Countdown());
    }

    private void HandlePlayLevel()
    {
        EnablePlayerMovement();

        // TO DO
        // In case we are coming out of Pause menu..
        // Set Time.deltaTime = 1f;
    }

    private void HandlePauseLevel()
    {
        DisablePlayerMovement();

        // TO DO
        // Set Time.deltaTime = 0f;
    }

    private void HandleRestartLevel()
    {
        DisablePlayerMovement();

        // TO DO
        // On Restart Level activated:
        // --PLAYER CTRL-- Disable player controller
        // --MENU MGR-- Pause before activating RESTART screen
        // --MENU MGR-- Show RESTART screen
        // Click to
        // (1) Retry (eg to beat high score) OR
        // (2) Main Menu
        // Hand back to MetaManager to destroy this level / scene
    }

    private void HandleCompleteLevel()
    {
        DisablePlayerMovement();

        // TO DO
        // On Complete Level activated:
        // --PLAYER CTRL-- Disable player controller
        // --MENU MGR-- Pause before activating WIN screen
        // --MENU MGR-- Show WIN screen
        // Click to
        // (1) Next Level OR 
        // (2) Retry (eg to beat high score) OR
        // (3) Main Menu
        // Hand back to MetaManager to destroy this level / scene
    }

    private void DisablePlayerMovement()
    {
        _playerController.enabled = false;
    }

    private void EnablePlayerMovement()
    {
        _playerController.enabled = true;
    }

    private IEnumerator Countdown()
    {
        int StartLevelDelay = 3;
        // Hardcoded delay in seconds
        yield return StartCoroutine(Wait(StartLevelDelay));
        // Feeding delay in seconds to Wait coroutine

        UpdateGameState(GameState.PlayLevel);
        Debug.Log("GameManager: PlayerController enabled");
    }

    private IEnumerator Wait(int delaySecs)
    {
        Debug.Log("GameManager: Waiting started");

        for (int i = 0; i < delaySecs; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            Debug.Log("Waited one second");
            // Figure out how to feed this into the UI
            // 3.. 2.. 1.. Go!
            // Set to 1/3 of a second so the delay isn't too long?
            //
            // This logic could be adapted to a StartLevel tutorial
            // eg Coroutine: Disable PlayerController | Show tutorial text Wait until key pressed
        }

        Debug.Log("GameManager: Waiting completed");
    }

    public void UnpauseGame()
    {
        UpdateGameState(GameState.PlayLevel);
    }

}

public enum GameState
{
    LoadingIn,
    StartLevel,
    PlayLevel,
    PauseLevel,
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


// StartCoroutine(Countdown());
// structure based on.. Eric5h5 · Aug 02, 2013
//
// Modification supplied by Rambit · May 08, 2014
// Just a quick note for other people using this solution:
// yield return StartCoroutine(FinishFirst(5.0f));
// I got an error when having new between return and StartCoroutine.
// https://answers.unity.com/questions/228150/hold-or-wait-while-coroutine-finishes.html


// I think updating the GameState makes most sense inside GameManager..
// GameManager.Instance.UpdateGameState(GameState.PlayLevel); 
// Update GameState >> PlayLevel


// PlayerController can't disable itself but GameManager can 


// Original version was just Instance = this;
// Do we need this more comprehensive Awake code?


// In his video, Tarodev explains why to use >> ?.Invoke <<
// It prevents an "unsubscribed error"
// Whatever that means (O_o)


// [From Video comments] Declaring enum outside the GameManager class because..
// "when referencing the enum outside of the Game Manager class we don't have to do
// GameManager.GameState.State, we can just do GameState.State. No other reason"


// HandlePlayLevel >>
//
// --PLAYERCOLLISION SCRIPT--
// IF COLLISION = OBJECT OR
// IF TRIGGER = KILLZONE
// THEN Update GameState >> RestartLevel
// GameManager.Instance.UpdateGameState(GameState.RestartLevel); 
// 
// IF TRIGGER = FINISH
// THEN Update GameState >> CompleteLevel
// UpdateGameState(GameState.CompleteLevel); 