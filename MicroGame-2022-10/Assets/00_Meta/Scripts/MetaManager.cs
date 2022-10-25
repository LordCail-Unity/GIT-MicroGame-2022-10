using System; // Required for..  public static event Action<>
// using System.Collections; // Required for IEnumerator Coroutines but not currently using
using UnityEngine;
using UnityEngine.SceneManagement;


// == GLOBAL ENUM ==
// Tarodev set up global enum BELOW the class
// We have moved it to the top for readability
// Check how best to index number and reconfigure enum indexing
// EG Could index them 10, 20, 30 etc (or even 100, 200, 300, etc)
// to give room to add new enums (depending on how many you might need)

public enum MetaState
{
    SetupApp = 10,
    MainMenu = 20, 
    LoadScene = 30, // Hand over to LoadingHandler
    GameManager = 40, // Hand over to GameManager to manage the Level Gameplay
    LevelRestart = 50, // Currently for Lose Game but could also be triggered mid-game from Pause Menu
    LevelComplete = 60, // Currently On Win; Could also trigger Save functions
    QuitMenu = 70,
    QuitApp = 80
}

public class MetaManager : MonoBehaviour
{

    public static MetaManager Instance;
    public MetaState _metaState;

    // GameManager is a global static instance so don't need this:
    // private GameManager _gameManager;

    public static event Action<MetaState> OnMetaStateChanged;

    public int mainMenuSceneIndex = 0; 
    public int firstSceneIndex = 1;

    // Moved to LoadingHandler
    // public float loadingUIDelaySecs = 0.1f;
    
    [HideInInspector] public int currentSceneIndex = 0;
    [HideInInspector] public int finalSceneIndex;
    [HideInInspector] public int sceneToLoad = 1;

    // Removed. Use SceneActivation instead:
    // [HideInInspector] public bool LoadingComplete; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UpdateMetaState(MetaState.SetupApp);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeMetaStateToMainMenu();
        Instance.finalSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        Debug.Log("MetaMgr: finalSceneIndex = " + Instance.finalSceneIndex);
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
            case MetaState.LevelRestart:
                HandleLevelRestart();
                break;
            case MetaState.LevelComplete:
                HandleLevelWin();
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
        Debug.Log("MetaManager: MetaState = " + _metaState);
    }

    public void ChangeMetaStateToMainMenu()
    {
        UpdateMetaState(MetaState.MainMenu);
    }

    private void HandleMainMenu()
    {
        // Figure out how to load MainMenu scene without stuffing 
        // everything up..
        
        Debug.Log("MetaManager: MetaState = " + _metaState);

        UpdateCurrentSceneIndex();
        Instance.sceneToLoad = firstSceneIndex;
        Debug.Log("MetaMgr: sceneToLoad = " + Instance.sceneToLoad);
    }

    public void ChangeMetaStateToLoadScene()
    {
        Debug.Log("MetaManager.ChangeMetaStateToLoadScene");

        UpdateCurrentSceneIndex();

        // THIS CHECK MUST BE BEFORE LOADSCENE
        // Otherwise LoadingScreen will show up before QuitMenu
        // Just let the loading screen show?

        if (Instance.sceneToLoad == Instance.mainMenuSceneIndex)
        {
            Debug.Log("Instance.sceneToLoad == Instance.mainMenuSceneIndex");
            SceneManager.LoadScene(Instance.mainMenuSceneIndex);
            Debug.Log("SceneManager.LoadScene(Instance.mainMenuSceneIndex)");
            UpdateMetaState(MetaState.MainMenu);
            return;
        }

        if (Instance.sceneToLoad == Instance.finalSceneIndex)
        {
            Debug.Log("Instance.sceneToLoad == Instance.finalSceneIndex");
            SceneManager.LoadScene(Instance.finalSceneIndex);
            Debug.Log("SceneManager.LoadScene(Instance.finalSceneIndex)");
            UpdateMetaState(MetaState.QuitMenu);
            return;
        }

        if (Instance.sceneToLoad != Instance.mainMenuSceneIndex && Instance.sceneToLoad != Instance.finalSceneIndex)
        {
            Debug.Log("Instance.sceneToLoad != Instance.mainMenuSceneIndex ");
            Debug.Log("&& Instance.sceneToLoad != Instance.finalSceneIndex");
            UpdateMetaState(MetaState.LoadScene);
            return;
        }

    }

    private void HandleLoadScene()
    {
        Debug.Log("MetaManager: MetaState = " + _metaState);

        Debug.Log("HandleLoadScene: Current Scene = " + Instance.currentSceneIndex);
        Debug.Log("HandleLoadScene: SceneToLoad = " + Instance.sceneToLoad);
    }

    public void ChangeMetaStateToGameManager()
    {
        Debug.Log("MetaManager.ChangeMetaStateToGameManager");

        UpdateCurrentSceneIndex();
        UpdateMetaState(MetaState.GameManager);
    }

    private void HandleGameManager()
    {
        Debug.Log("MetaManager: MetaState = " + _metaState);
        Debug.Log("MetaManager: Hand over to GameManager");

        // GameManager is a global static instance so don't need this:
        // _gameManager = FindObjectOfType<GameManager>();
        // _gameManager.UpdateGameState(GameState.StartLevel);

        GameManager.Instance.UpdateGameState(GameState.StartLevel);
    }

    public void ChangeMetaStateToLevelRestart()
    {
        Debug.Log("MetaManager.ChangeMetaStateToLevelRestart");
        UpdateMetaState(MetaState.LevelRestart);
    }

    private void HandleLevelRestart()
    {
        // MOVED FROM GAME MANAGER
        Debug.Log("MetaManager: MetaState = " + _metaState);
        LoadThisScene();
    }

    public void ChangeMetaStateToLevelWin()
    {
        Debug.Log("MetaManager.ChangeMetaStateToLevelWin");
        UpdateMetaState(MetaState.LevelComplete);
    }

    private void HandleLevelWin()
    {
        // MOVE THIS STATE TO META MANAGER?

        Debug.Log("MetaManager: MetaState = " + _metaState);

        LoadNextScene();

        // TO DO IN THIS STATE
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

    public void ChangeMetaStateToQuitMenu()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
        Debug.Log("MetaManager.ChangeMetaStateToQuitMenu");
        UpdateMetaState(MetaState.QuitMenu);
    }

    private void HandleQuitMenu()
    {
        Debug.Log("MetaManager: MetaState = " + _metaState);
    }

    public void ChangeMetaStateToQuitApp()
    {
        // Check conditions to switch MetaState 
        // If all conditions are met, change the MetaState
        Debug.Log("MetaManager.ChangeMetaStateToQuitApp");
        UpdateMetaState(MetaState.QuitApp);
    }

    private void HandleQuitApplication()
    {
        Debug.Log("MetaManager: MetaState = " + _metaState);
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

    // REFACTOR ALL BELOW INTO
    //
    // >> MetaUI = Possible
    // >> New MetaUI sub-component = better eg new MetaUIButtonHandler
    // >> LoadingHandler = NOT possible(?)
    // CANNOT be called by LoadingHandler because this sequence ENABLES LoadingHandler
    // when MetaUI enables LoadingScreenUI

    public void UpdateCurrentSceneIndex()
    {
        Instance.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("MetaManager: newCurrentSceneIndex = " + Instance.currentSceneIndex);
    }

    public void LoadMainMenuScene()
    {
        Debug.Log("MetaManager.LoadMainMenuScene");

        Instance.sceneToLoad = Instance.mainMenuSceneIndex;
        Debug.Log("Instance.sceneToLoad = " + Instance.sceneToLoad.ToString());

        ChangeMetaStateToLoadScene();
        // From other classes call this using:
        // MetaManager.Instance.ChangeMetaStateToLoadScene();
    }

    public void LoadThisScene()
    {
        Debug.Log("MetaManager.LoadThisScene");

        Instance.sceneToLoad = Instance.currentSceneIndex;
        
        ChangeMetaStateToLoadScene();
        // From other classes call this using:
        // MetaManager.Instance.ChangeMetaStateToLoadScene();
    }

    public void LoadNextScene()
    {
        Debug.Log("MetaManager.LoadNextScene");
        
        Instance.sceneToLoad = Instance.currentSceneIndex + 1;
        
        ChangeMetaStateToLoadScene();
        // From other classes call this using:
        // MetaManager.Instance.ChangeMetaStateToLoadScene();
    }

    public void QuitApp()
    {
        ChangeMetaStateToQuitApp();
    }

}




// ==NOTES==

// READ THE WHOLE THREAD LINKED HERE ESPECIALLY THE COMMENTS 
// Outlines multiple different ways to approach this issue
//
// In particular see bottom comment (and bottom of this page) for 
//
// https://stackoverflow.com/questions/35890932/unity-game-manager-script-works-only-one-time/35891919#35891919


// Another way to handle MetaManager
// https://answers.unity.com/questions/1695582/additive-scene-loading-or-dont-destroy-on-load.html
// "I start the game with a scene that loads all my singletons
// (like translation management, input management, etc.)
// then sends the users to a new main menu scene
// where they can do stuff before playing the game (loaded in separate game scenes)."


// ASYNC LOAD MAIN MENU??
// Doesn't work as it loads up the MetaManager again..
// ASYNC UNLOAD CURRENT SCENE??
// Doesn't work without Additive Loading as you cannot unload the only scene you have open
// Change our Async loading to Additive mode and always have Scene 0 open?
// Then we could take Main Menu out of DDOL mode


// CHANGETO methods
// Check conditions to switch MetaState 
// If all conditions are met, change the MetaState
// Could simply add the checks into UpdateMetaState
// However although it is longer I prefer this for readability

// REFACTOR CHANGETO calls? Possibly call all of the ChangeTo methods from Update??
// UPSIDE: This could decouple the calling methods as they can simply
// trigger public switches etc..
// DOWNSIDE: Lots of unnecessary Update checks


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


// HANDLE LEVEL RESTART 
// TO DO IN THIS STATE
// On Restart Level activated:
// --MENU MGR-- Pause before activating RESTART screen
// --MENU MGR-- Show RESTART screen
// Click to
// (1) Retry (eg to beat high score) OR
// (2) Main Menu


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



// https://docs.unity3d.com/2020.3/Documentation/Manual/SpecialFolders.html

//Special folder names
//You can usually choose any name you like for the folders you create to organize your Unity project. However, there are folder names that Unity reserves for special purposes. For example, you must place Editor scripts
// in a folder called Editor for them to work correctly.

//This page contains the full list of special folder names used by Unity.


//Assets
//The Assets
// folder is the main folder that contains the Assets used by a Unity project. The contents of the Project window
// in the Editor correspond directly to the contents of the Assets folder. Most API functions assume that everything is located in the Assets folder, and so don’t require it to be mentioned explicitly. However, some functions do need to have the Assets folder included as part of a pathname (for example, certain functions in the AssetDatabase class).


//Editor
//Editor scripts add functionality to Unity during development, but aren’t available in builds at runtime. Scripts in an Editor folder run as Editor scripts, not runtime scripts.

//You can have multiple Editor folders placed anywhere inside the Assets folder. Place your Editor scripts inside an Editor folder or a subfolder within it.

//The exact location of an Editor folder affects the time at which its scripts are compiled relative to other scripts. See documentation on Special Folders and Script Compilation Order for a full description.

//Use the EditorGUIUtility.Load function in Editor scripts to load Assets from a Resources folder within an Editor folder. These Assets are only loaded through Editor scripts, and are stripped from builds.

//Note: Unity doesn’t allow components derived from MonoBehaviour to be assigned to GameObjects
// if the scripts are in the Editor folder.


//Editor Default Resources
//Editor scripts can make use of Asset files loaded on-demand using the EditorGUIUtility.Load function.This function looks for the Asset files in a folder called Editor Default Resources.

//You can only have one Editor Default Resources folder and you must place it in Project root, directly within the Assets folder.Place the needed Asset files in this Editor Default Resources folder, or a subfolder within it.Always include the subfolder path in the path passed to the EditorGUIUtility.Load function if your Asset files are in subfolders.


//Gizmos
//Gizmos allow you to add graphics to the Scene
// View to help visualize design details that are otherwise invisible. The Gizmos.DrawIcon function places an icon in the Scene to act as a marker for a special object or position.You must place the image file used to draw this icon in a folder called Gizmos
// for the DrawIcon function to locate it.

//You can only have one Gizmos folder and it must be placed in the root of the Project, directly within the Assets folder.Place the needed Asset files in this Gizmos folder or a subfolder within it.Always include the subfolder path in the path passed to the Gizmos.DrawIcon function if your Asset files are in subfolders.


//Resources
//You can load Assets on-demand from a script instead of creating instances of Assets in a Scene for use in gameplay.You do this by placing the Assets in a folder called Resources. Load these Assets by using the Resources.Load function.

//You can have multiple Resources folders placed anywhere inside the Assets folder.Place the needed Asset files in a Resources folder or a subfolder within it. Always include the subfolder path in the path passed to the Resources.Load function if your Asset files are in subfolders.

//Note: If the Resources folder is an Editor subfolder, the Assets in it are loadable from Editor scripts, but are removed from builds.


//Standard Assets
//When you import a Standard Asset package (menu: Assets > Import Package) the Assets are placed in a folder called Standard Assets. As well as containing the Assets, these folders also have an effect on script compilation order; see the page on Special Folders and Script Compilation Order for further details.

//You can only have one Standard Assets folder and it must be placed in the root of the Project, directly within the Assets folder. Place the needed Assets files in this Standard Assets folder or a subfolder within it.


//StreamingAssets
//You may want the Asset to be available as a separate file in its original format (although it’s more common to directly incorporate Assets into a build). For example, you need to access a video file from the filesystem to play the video on IOS
// using Handheld.PlayFullScreenMovie.

//To include streaming Assets, do the following:

//Place the file in the StreamingAssets folder.
//The file remains unchanged when copied to the target machine, where it’s available from a specific folder.
//See the page about Streaming Assets for further details.

//You can only have one StreamingAssets folder and it must be placed in the root of the Project, directly within the Assets folder. Place the Assets files in the StreamingAssets folder or subfolder. Always include the subfolder path in the path used to reference the streaming asset if your Asset files are in subfolders.


//Hidden Assets
//During the import process, Unity ignores the following files and folders in the Assets folder (or a sub-folder within it):

//Hidden folders.
//Files and folders which start with ‘.’.
//Files and folders which end with ‘~’.
//Files and folders named cvs.
//Files with the extension .tmp.
//This prevents importing special and temporary files created by the operating system or other applications.