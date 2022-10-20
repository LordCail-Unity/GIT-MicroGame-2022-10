using System; // Required for..  public static event Action<>
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{

    public static MetaManager Instance;
    public MetaState _metaState;

    public static event Action<MetaState> OnMetaStateChanged;

    public int mainMenuSceneIndex = 0; 
    public int firstSceneIndex = 1;

    private int currentSceneIndex;
    private int sceneToLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // "gameObject" NOT "GameObject" as the latter will throw an error!!
            // Why dis? (O_o) 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateMetaState(MetaState.InitializeApplication);
        UpdateCurrentSceneIndex();
        sceneToLoad = firstSceneIndex;
    }

    private void UpdateCurrentSceneIndex()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void UpdateMetaState(MetaState newMetaState)
    {
        _metaState = newMetaState;

        switch (newMetaState)
        {
            case MetaState.InitializeApplication:
                HandleInitializeApplication(); // This is called at Start
                break;
            case MetaState.LoadScene:
                HandleLoadScene();
                break;
            case MetaState.GameManager:
                HandleGameManager();
                break;
            case MetaState.QuitApplication:
                HandleQuitApplication();
                break;
        }
        OnMetaStateChanged?.Invoke(newMetaState);
    }

    private void HandleInitializeApplication()
    {
        // StartMenuUI activated automatically during this MetaState by MetaMenuManager
        
        // Called automatically when game is initialized so we don't want to immediately LoadLevel
        // Could do things here like instantiating the Main Menu or other GameObjects in a specific order,
        // setting up audio managers, etc
    }

    private void HandleLoadScene()
    {
        Debug.Log("HandleLoadScene: Current Scene = " + currentSceneIndex);
        Debug.Log("HandleLoadScene: SceneToLoad = " + sceneToLoad);
        LoadScene(sceneToLoad);
    }

    private void HandleGameManager()
    {
        Debug.Log("MetaManager: Hand over to GameManager");
    }

    private void HandleQuitApplication()
    {
        // Wrong order.. Need to..
        // Change the MetaState > MetaState.QuitApplication
        // That will display the Quit Screen
        // On button press do Quit Application

        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

    public void LoadMainMenuScene()
    {
        Debug.Log("LevelManager.LoadMainMenuScene");
        sceneToLoad = mainMenuSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void ReloadThisScene()
    {
        Debug.Log("LevelManager.ReloadThisScene");
        sceneToLoad = currentSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadNextScene()
    {
        Debug.Log("LevelManager.LoadNextLevel");
        sceneToLoad = currentSceneIndex + 1;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadScene(int sceneIndex)
    {
        
        // Change to async loading method 
        // Add a loading screen
        SceneManager.LoadScene(sceneIndex);
        UpdateCurrentSceneIndex();
        UpdateMetaState(MetaState.GameManager);
    }

    public void QuitGame()
    {
        UpdateMetaState(MetaState.QuitApplication);
    }

}

    public enum MetaState
{
    InitializeApplication,
    LoadScene,
    GameManager,
    QuitApplication
}



// ==NOTES==


// HandleInitializeGame()
// Called automatically when game is initialized so we don't want to immediately LoadLevel
// Could do things here like instantiating the Main Menu or other GameObjects in a specific order,
// setting up audio managers, etc


// ASYNC LOADING OPTION:
// Create Async LevelLoader script with a fake 1 second-ish loading screen 
// Each MetaManager method can refer to the LevelLoader and feed in the level to load


// ADDITIVE LOADING OPTION:
// (levelToLoad, LoadSceneMode.Additive)


//TEMP QUICK CODE TO CHECK FOR ENDGAME SCENE
// Do we need this any more?
// Update to EndGame state
//var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
//    if (currentSceneIndex == SceneManager.sceneCountInBuildSettings)
//    {
//        MetaManager.Instance.UpdateMetaState(MetaState.EndGame);
//        Debug.Log("MetaState >> EndGame");
//    }


// Updating MetaState >> LoadScene should automatically trigger
// HandleLoadScene();


// Updating MetaState >> QuitApplication should automatically trigger
// HandleLoadScene();


// We have GameManager to handle transitions in a single game level. See GameManager notes.
//
// [From Video comments] Declaring enum outside the MetaManager class because..
// "when referencing the enum outside of the MetaManager class we don't have to do
// MetaManager.MetaState.State, we can just do MetaState.State. No other reason"
//
// How to deal with Start & End scenes?
//
// One thing we could do is to set up LevelManager "above" GameManager
// This would make sense as the LevelManager is looking after Scenes
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
// Need to figure out how to use DontDestroyOnLoad for 1RingManager (TEMP LevelManager)
//
// https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Object.DontDestroyOnLoad.html
//
// METAMANAGER DONT DESTROY ON LOAD METHOD..
//
// Tarodev's SceneManager / LevelManager video will be the basis of our DontDestroyOnLoad code
//
// https://www.youtube.com/watch?v=OmobsXZSRKo
// ..
