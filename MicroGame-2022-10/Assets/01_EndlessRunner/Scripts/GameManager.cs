using System; // Required for:  public static event Action<>
using System.Collections; // Required for IEnumerator Coroutines
using UnityEngine;

// == GLOBAL ENUM ==
// Tarodev set up global enum BELOW the class
// We have moved it to the top for readability
// Check how best to index number and reconfigure enum indexing
// EG Could index them 10, 20, 30 etc or even 100, 200, 300, etc
// to give room to add new enums

public enum GameState
{
    SetupLevel = 10,
    StartLevel = 20,
    PlayLevel = 30,
    PauseLevel = 40,
    ExitLevel = 50
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState _gameState;

    // private PlayerController _playerController;
    // REFACTORED as public static instance

    public static event Action<GameState> OnGameStateChanged;

    private int StartLevelDelaySecs = 1;
    private int menuUIDelaySecs = 2;
    // private so we don't need to Reset GameManager every time we change it

    [HideInInspector] public bool levelCompleted = false;

    private void Awake()
    {
        Debug.Log("GameManager Awake()");

        if (Instance == null)
        { Instance = this; }
        else { Destroy(gameObject); }

        UpdateGameState(GameState.SetupLevel);
        // Unnecessary as SetupLevel is default but just in case..
    }

    private void Start()
    {

        DisablePlayerMovement();
        Debug.Log("GameManager: PlayerController disabled");

        // UpdateGameState(GameState.StartLevel);
        // DISABLED: Wait for MetaManager to trigger StartLevel
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
            case GameState.SetupLevel:
                HandleSetupLevel();
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
            case GameState.ExitLevel:
                HandleExitLevel();
                break;
        }
        OnGameStateChanged?.Invoke(newGameState);
    }

    private void HandleSetupLevel()
    {
        Debug.Log("GameManager: GameState = " + _gameState);
        // Wait for MetaManager to finish loading processes
        // MetaManager will then trigger StartLevel
    }

    private void HandleStartLevel()
    {
        Debug.Log("GameManager: GameState = " + _gameState);
        StartCoroutine(Countdown());
    }

    private void HandlePlayLevel()
    {
        Debug.Log("GameManager: GameState = " + _gameState);
        EnablePlayerMovement();
        Debug.Log("Player movement enabled");

        // TO DO IN THIS STATE
        // In case we are coming out of Pause menu..
        // Set Time.deltaTime = 1f;
    }

    private void HandlePauseLevel()
    {
        Debug.Log("GameManager: GameState = " + _gameState);
        DisablePlayerMovement();
        Debug.Log("Player movement disabled");

        // TO DO IN THIS STATE
        // Set Time.deltaTime = 0f;
    }

    private void HandleExitLevel()
    {
        Debug.Log("GameManager: GameState = " + _gameState);

        DisablePlayerMovement();
        Debug.Log("Player movement disabled");

        StartCoroutine(ExitActions());
    }

    private IEnumerator ExitActions()
    {
        // Stop doing stuff..
        // Where should we trigger this? 
        // Currently PlayerCollision but move to GameManager?? 

        // --PLAYER CTRL-- Disable player controller
        // Hand back to MetaManager to destroy this level / scene

        yield return StartCoroutine(Wait(menuUIDelaySecs));
        Debug.Log("StartCoroutine(Wait(menuUIDelaySecs))");
        // Feeding delay in seconds to Wait coroutine

        if (levelCompleted == true)
        {
            MetaManager.Instance.ChangeMetaStateToCompleteLevel();
        }
        else
        {
            MetaManager.Instance.ChangeMetaStateToRestartLevel();
        }
    }

    private void DisablePlayerMovement()
    {
        PlayerController.Instance.enabled = false;
        Debug.Log("PlayerController.Instance.enabled = " + PlayerController.Instance.enabled.ToString());

        // Prevents multiple collisions eg with obstacles
        // PlayerCollision.Instance.enabled = false;
        // Debug.Log("PlayerCollision.Instance.enabled = " + PlayerController.Instance.enabled.ToString());

        // _playerController.enabled = false;
        // REFACTORED as public static PlayerController Instance
    }

    private void EnablePlayerMovement()
    {
        PlayerController.Instance.enabled = true;
        Debug.Log("PlayerController.Instance.enabled = " + PlayerController.Instance.enabled.ToString());

        // _playerController.enabled = true;
        // REFACTORED as public static PlayerController Instance

        // Disabling prevents multiple collisions eg with obstacles
        // As we are disabling it in the "Disable Player" we need to enable it too
        // PlayerCollision.Instance.enabled = true;
        // Debug.Log("PlayerCollision.Instance.enabled = " + PlayerController.Instance.enabled.ToString());
    }

    private IEnumerator Countdown()
    {
        yield return StartCoroutine(Wait(StartLevelDelaySecs));
        // Feeding delay in seconds to Wait coroutine

        UpdateGameState(GameState.PlayLevel);
        Debug.Log("GameManager: PlayerController enabled");
    }

    private IEnumerator Wait(int delaySecs)
    {
        Debug.Log("IEnumerator Wait: Waiting started");

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

        Debug.Log("IEnumerator Wait: Waiting completed");
    }

    public void UnpauseGame()
    {
        UpdateGameState(GameState.PlayLevel);
    }

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