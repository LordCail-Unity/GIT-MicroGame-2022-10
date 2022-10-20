using System; // Required for..  public static event Action<>
using System.Collections; // Required for IEnumerator Coroutines
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
    }

    private void Start()
    {
        UpdateMetaState(MetaState.MainMenu);

        UpdateCurrentSceneIndex();
        sceneToLoad = firstSceneIndex;
        finalSceneIndex = SceneManager.sceneCountInBuildSettings - 1; // BuildIndex starts at 0 so remove 1
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
            case MetaState.MainMenu:
                HandleMainMenu(); // This is called at Start
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

    private void HandleMainMenu()
    {
        Debug.Log("HandleMainMenu: MetaState = " + _metaState);
        // MetaUI activated automatically during this MetaState by MetaMenuManager

        // Called automatically when game is initialized so we don't want to immediately LoadLevel
        // Could do things here like instantiating the Main Menu or other GameObjects in a specific order,
        // setting up audio managers, etc
    }

    private void HandleLoadScene()
    {
        Debug.Log("HandleLoadScene: MetaState = " + _metaState);

        Debug.Log("HandleLoadScene: Current Scene = " + currentSceneIndex);
        Debug.Log("HandleLoadScene: SceneToLoad = " + sceneToLoad);

        LoadSceneAsync(sceneToLoad);
    }

    private void HandleGameManager()
    {
        Debug.Log("HandleGameManager: MetaState = " + _metaState);
        Debug.Log("MetaManager: Hand over to GameManager");
    }

    private void HandleQuitApplication()
    {
        Debug.Log("HandleQuitApplication: MetaState = " + _metaState);
        // Wrong order.. Need to..
        // Change the MetaState > MetaState.QuitApplication
        // That will display the Quit Screen
        // On button press do Quit Application

        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

    public void LoadMainMenuScene()
    {
        Debug.Log("MetaManager.LoadMainMenuScene");
        sceneToLoad = mainMenuSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadThisScene()
    {
        Debug.Log("MetaManager.ReloadThisScene");
        sceneToLoad = currentSceneIndex;
        UpdateMetaState(MetaState.LoadScene);
    }

    public void LoadNextScene()
    {
        Debug.Log("MetaManager.LoadNextScene");
        sceneToLoad = currentSceneIndex + 1;
        UpdateMetaState(MetaState.LoadScene);
    }

    public async void LoadSceneAsync(int sceneIndex)
    {
        Debug.Log("AsyncLoad: sceneIndex = " + sceneIndex);

        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        var progress = scene.progress;

        do 
        {
            await Task.Delay(100);
            Debug.Log("Waited 100");
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        await Task.Delay(1);

        UpdateCurrentSceneIndex();
        Debug.Log("AsyncLoad: New CurrentScene = " + currentSceneIndex);
        UpdateMetaState(MetaState.GameManager);
    }

    public void QuitGame()
    {
        UpdateMetaState(MetaState.QuitApplication);
    }

}

    public enum MetaState
{
    MainMenu,
    LoadScene,
    GameManager,
    QuitApplication
}



// ==NOTES==


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
