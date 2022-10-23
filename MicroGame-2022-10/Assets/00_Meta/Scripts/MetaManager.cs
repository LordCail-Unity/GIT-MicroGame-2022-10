using System; // Required for..  public static event Action<>
// using System.Collections; // Required for IEnumerator Coroutines but not currently using
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{

    public static MetaManager Instance;
    public MetaState _metaState;

    private GameManager _gameManager;

    public static event Action<MetaState> OnMetaStateChanged;

    public int mainMenuSceneIndex = 0; 
    public int firstSceneIndex = 1;

    // Moved to LoadingHandler
    // public float loadingUIDelaySecs = 0.1f;
    
    [HideInInspector] public int currentSceneIndex = 0;
    [HideInInspector] public int finalSceneIndex;
    [HideInInspector] public int sceneToLoad = 1;

    [HideInInspector] public bool LoadingComplete; 
    // Should probably use SceneActivation instead but going to try a quick fix

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

        UpdateMetaState(MetaState.SetupApp);
    }

    private void Start()
    {
        UpdateMetaState(MetaState.MainMenu);

        UpdateCurrentSceneIndex();
        Instance.sceneToLoad = firstSceneIndex;
        Debug.Log("MetaMgr Start: sceneToLoad = " + Instance.sceneToLoad);

        Instance.finalSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        Debug.Log("MetaMgr Start: finalSceneIndex = " + Instance.finalSceneIndex);
    }

    public void UpdateCurrentSceneIndex()
    {
        Instance.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void UpdateMetaState(MetaState newMetaState)
    {
        _metaState = newMetaState;

        switch (newMetaState)
        {
            case MetaState.SetupApp:
                HandleSetupApp();
                break;
            case MetaState.MainMenu:
                HandleMainMenu(); 
                break;
            case MetaState.LoadScene:
                HandleLoadScene();
                break;
            case MetaState.GameManager:
                HandleGameManager();
                break;
            case MetaState.QuitMenu:
                HandleQuitMenu();
                break;
            case MetaState.QuitApp:
                HandleQuitApplication();
                break;
            default:
                break;
        }
        OnMetaStateChanged?.Invoke(newMetaState);
    }

    private void HandleSetupApp()
    {
        Debug.Log("HandleSetupApp: MetaState = " + _metaState);
    }

    public void TransitionToMainMenu()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
        // Possibly call this from Update??
        // This could decouple the calling methods too as they can simply
        // trigger public switches etc..
    }

    private void HandleMainMenu()
    {
        Debug.Log("HandleMainMenu: MetaState = " + _metaState);
    }

    public void TransitionToLoadScene()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
    }

    private void HandleLoadScene()
    {
        Debug.Log("HandleLoadScene: MetaState = " + _metaState);

        Debug.Log("HandleLoadScene: Current Scene = " + Instance.currentSceneIndex);
        Debug.Log("HandleLoadScene: SceneToLoad = " + Instance.sceneToLoad);
    }

    public void TransitionToGameManager()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
        UpdateCurrentSceneIndex();
        Debug.Log("AsyncLoad: newCurrentSceneIndex = " + Instance.currentSceneIndex);
        Debug.Log("AsyncLoad: finalSceneIndex = " + finalSceneIndex);

        // MOVE THIS CHECK BEFORE LOADING SCREEN
        // CURRENTLY IT SHOWS LOADING SCREEN BEFORE 
        // QUIT SCREEN

        if (Instance.currentSceneIndex == finalSceneIndex)
        {
            MetaManager.Instance.UpdateMetaState(MetaState.QuitMenu);
        }
        else
        {
            MetaManager.Instance.UpdateMetaState(MetaState.GameManager);
        }
    }

    private void HandleGameManager()
    {
        Debug.Log("HandleGameManager: MetaState = " + _metaState);
        Debug.Log("MetaManager: Hand over to GameManager");

        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.UpdateGameState(GameState.StartLevel);

    }

    public void TransitionToQuitMenu()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
    }

    private void HandleQuitMenu()
    {
        Debug.Log("HandleQuitMenu: MetaState = " + _metaState);
    }

    public void TransitionToQuitApp()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
    }

    private void HandleQuitApplication()
    {
        Debug.Log("HandleQuitApplication: MetaState = " + _metaState);
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

    // REFACTOR ALL BELOW INTO
    //
    // >> MetaUI >> Possible
    //
    // >> LoadingHandler >> NO
    // CANNOT be called by LoadingHandler because this sequence ACTIVATES LoadingHandler

    public void LoadMainMenuScene()
    {
        Debug.Log("MetaManager.LoadMainMenuScene");
        Instance.sceneToLoad = Instance.mainMenuSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadThisScene()
    {
        Debug.Log("MetaManager.LoadThisScene");
        Instance.sceneToLoad = Instance.currentSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadNextScene()
    {
        Debug.Log("MetaManager.LoadNextScene");
        Instance.sceneToLoad = Instance.currentSceneIndex + 1;
        UpdateMetaState(MetaState.LoadScene);
    }


    public void QuitApp()
    {
        UpdateMetaState(MetaState.QuitApp);
    }

}

public enum MetaState
{
    SetupApp = 1,
    MainMenu = 2,
    LoadScene = 3,
    GameManager = 4,
    QuitMenu = 5,
    QuitApp = 6
}



// ==NOTES==

// READ THE WHOLE THREAD LINKED HERE ESPECIALLY THE COMMENTS 
// Outlines multiple different ways to approach this issue
//
// https://stackoverflow.com/questions/35890932/unity-game-manager-script-works-only-one-time/35891919#35891919


// Variables do NOT reset between Playtest sessions
// Try using Instance.currentSceneIndex in the code body
// This seems to have fixed it


// Debug.Log("MetaMgr Start: MetaState = " + _metaState); 
// Why is this debug log coming out as an integer? 
// And not the right integer for the Enum type..
// But only if you pause the playtest before pressing the Start button (O_o)


// HandleMainMenu
// MetaUI activated automatically during this MetaState by MetaMenuManager
// Called automatically when game is initialized so we don't want to immediately LoadLevel
// Could do things here like instantiating the Main Menu or other GameObjects in a specific order,
// setting up audio managers, etc



// FinalSceneIndex
// BuildIndex starts at 0 so remove 1
// Try "Instance.variable" as the variables don't seem to be resetting between Playtests

// DontDestroyOnLoad(gameObject);
// "gameObject" NOT "GameObject" as the latter will throw an error!!
// Why dis? (O_o) 


// HandleInitializeGame()
// Called automatically when game is initialized so we don't want to immediately LoadLevel
// Could do things here like instantiating the Main Menu or other GameObjects in a specific order,
// setting up audio managers, etc


// BASIC LOADING OPTION
// public void LoadScene(int sceneIndex)
// { SceneManager.LoadScene(sceneIndex); }
// Need to wait AT LEAST one frame after loading to set active scene
// OR call SetActiveScene to be explicit..
// In the end we just went with Tarodev's loading script
// https://www.youtube.com/watch?v=OmobsXZSRKo&t=32s

// ASYNC LOADING OPTION:
// Create Async LevelLoader script with a fake 1 second-ish loading screen 
// Each MetaManager method can refer to the LevelLoader and feed in the level to load


// ADDITIVE LOADING OPTION:
// (levelToLoad, LoadSceneMode.Additive)


//TEMP QUICK CODE TO CHECK FOR ENDGAME SCENE
// Do we need this any more?
// Update to EndGame state
//    if (currentSceneIndex == finalSceneIndex)
//    {
//        MetaManager.Instance.UpdateMetaState(MetaState.EndGame);
//        Debug.Log("MetaState >> EndGame");
//    }


// HandleQuitApp
// Initially had wrong order.. Needed to..
// Change the MetaState > MetaState.QuitMenu
// That will display the Quit Menu
// On button press change to MetaState QuitApp
// That triggers Quit Application


// Updating MetaState >> LoadScene should automatically trigger
// HandleLoadScene();


// Updating MetaState >> QuitApplication should automatically trigger
// HandleLoadScene();

// Get Enum member from int
// https://answers.unity.com/questions/447240/get-enum-member-from-int.html


// We have GameManager to handle transitions in a single game level. See GameManager notes.
//
// [From Video comments] Declaring enum outside the MetaManager class because..
// "when referencing the enum outside of the MetaManager class we don't have to do
// MetaManager.MetaState.State, we can just do MetaState.State. No other reason"
//
// How to deal with Start & End scenes?
//
// One thing we could do is to set up MetaManager "above" GameManager
// This would make sense as the MetaManager is looking after Scenes
// including the Start and EndGame scenes (it should be called SceneManager
// but this would clash with the Unity class of the same name).
// 
// We will probably also introduce a 1RingManager -- a DontDestroyOnLoad to rule them all -- 
// which can handle game initialization, Save & Load data, and things like settings, audio, etc 
// 
// The 1Ring could even instantiate all of the required systems in a linear fashion 
// giving us full control over startup. 
//
// For now let's adapt Tarodev's GameManager code to suit our grand Meta scheming.
// Need to figure out how to use DontDestroyOnLoad for 1RingManager (TEMP MetaManager)
//
// https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Object.DontDestroyOnLoad.html
//
// METAMANAGER DONT DESTROY ON LOAD METHOD..
//
// Tarodev's SceneManager / MetaManager video will be the basis of our DontDestroyOnLoad code
//
// https://www.youtube.com/watch?v=OmobsXZSRKo
// ..


// Really good explanation of SerializeField
// 
// https://gamedevbeginner.com/how-to-get-a-variable-from-another-script-in-unity-the-right-way/
//
// This will show in the inspector
// [SerializeField]
// private float playerHealth;
//
// This will not show in the inspector
// [HideInInspector]
// public float playerHealth;
//
// Generally speaking, it’s good practice to only make a variable public if other scripts need to access it and, if they don’t, to keep it private.
// If, however, other scripts do not need to access it, but you do need to see it in the inspector then you can use Serialize Field instead.
