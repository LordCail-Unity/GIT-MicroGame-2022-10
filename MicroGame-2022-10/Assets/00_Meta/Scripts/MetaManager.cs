using System; // Required for..  public static event Action<>
// using System.Collections; // Required for IEnumerator Coroutines but not currently using
using System.Threading.Tasks; // Required for Task.Delay
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{

    public static MetaManager Instance;
    public MetaState _metaState;

    public static event Action<MetaState> OnMetaStateChanged;

    public int mainMenuSceneIndex = 0; 
    public int firstSceneIndex = 1;

    public float loadingUIDelaySecs = 0.1f;

    private int currentSceneIndex = 0;
    private int sceneToLoad = 1;
    private int finalSceneIndex;

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

    private void UpdateCurrentSceneIndex()
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

    private void HandleMainMenu()
    {
        Debug.Log("HandleMainMenu: MetaState = " + _metaState);
    }

    private void HandleLoadScene()
    {
        Debug.Log("HandleLoadScene: MetaState = " + _metaState);

        Debug.Log("HandleLoadScene: Current Scene = " + Instance.currentSceneIndex);
        Debug.Log("HandleLoadScene: SceneToLoad = " + Instance.sceneToLoad);

        LoadSceneAsync(Instance.sceneToLoad);
    }

    private void HandleGameManager()
    {
        Debug.Log("HandleGameManager: MetaState = " + _metaState);
        Debug.Log("MetaManager: Hand over to GameManager");
    }

    private void HandleQuitMenu()
    {
        Debug.Log("HandleQuitMenu: MetaState = " + _metaState);

    }

    private void HandleQuitApplication()
    {
        Debug.Log("HandleQuitApplication: MetaState = " + _metaState);
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

    public void LoadMainMenuScene()
    {
        Debug.Log("MetaManager.LoadMainMenuScene");
        Instance.sceneToLoad = Instance.mainMenuSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadThisScene()
    {
        Debug.Log("MetaManager.ReloadThisScene");
        Instance.sceneToLoad = Instance.currentSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadNextScene()
    {
        Debug.Log("MetaManager.LoadNextScene");
        Instance.sceneToLoad = Instance.currentSceneIndex + 1;
        UpdateMetaState(MetaState.LoadScene);
    }

    public async void LoadSceneAsync(int sceneIndex)
    {
        Debug.Log("AsyncLoad: sceneIndex = " + sceneIndex);

        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        var progress = scene.progress;

        // Can we change this to an Operation style method 
        // like the one we used in the Brackeys Loading tutorial?
        // Effect is the same but it feels cleaner

        do 
        {
            await Task.Delay(1000);
            Debug.Log("Waited 100");
        } 
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        await Task.Delay(1); 
        // Delay to allow SceneActivation to kick in before moving to next action

        UpdateCurrentSceneIndex();
        Debug.Log("AsyncLoad: New currentSceneIndex = " + Instance.currentSceneIndex);
        Debug.Log("AsyncLoad: finalSceneIndex = " + Instance.finalSceneIndex);

        if (Instance.currentSceneIndex == Instance.finalSceneIndex)
        {
            UpdateMetaState(MetaState.QuitMenu);
        }
        else
        {
            UpdateMetaState(MetaState.GameManager);
        }
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
