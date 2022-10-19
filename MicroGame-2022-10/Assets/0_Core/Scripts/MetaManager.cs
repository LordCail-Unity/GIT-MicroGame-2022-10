using System; // Required for..  public static event Action<>
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{

    public static MetaManager Instance;
    public MetaState _metaState;

    public static event Action<MetaState> OnMetaStateChanged;

    private int currentLevel;
    public int mainMenuIndex = 0; // MainMenu BuildIndex
    public int levelToLoad = 1; // Default BuildIndex

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // gameObject NOT GameObject as the latter will throw an error!!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateMetaState(MetaState.StartGame);
        UpdateCurrentLevelIndex();
    }

    private void UpdateCurrentLevelIndex()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void UpdateMetaState(MetaState newMetaState)
    {
        _metaState = newMetaState;

        switch (newMetaState)
        {
            case MetaState.StartGame:
                HandleStartGame(); // This is called at Start
                break;
            case MetaState.LoadLevel:
                HandleLoadLevel();
                break;
            case MetaState.EndGame:
                HandleEndGame();
                break;
        }
        OnMetaStateChanged?.Invoke(newMetaState);
    }

    private void HandleStartGame()
    {
        // Because this method is called at Start we don't want to immediately LoadLevel
    }

    public void LoadFirstLevel()
    {
        // StartGame called from Play Button
        // Unless we're doing something else here PlayButton can just go straight to HandleStartGame..

        MetaManager.Instance.UpdateMetaState(MetaState.LoadLevel);
        // Update to LoadLevel state

        LoadLevel(levelToLoad);
    }

    private void HandleLoadLevel()
    {
        LoadLevel(levelToLoad);

        //TEMP QUICK CODE TO CHECK FOR ENDGAME SCENE
        // Update to EndGame state

        //var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //    if (currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        //    {
        //        MetaManager.Instance.UpdateMetaState(MetaState.EndGame);
        //        Debug.Log("MetaState >> EndGame");
        //    }

    }

    private void HandleEndGame()
    {
    }

    public void LoadMainMenu()
    {
        LoadLevel(mainMenuIndex); 
    }

    public void ReloadThisLevel()
    {
        Debug.Log("LevelManager.ReloadThisLevel");
        LoadLevel(currentLevel); 
    }

    public void LoadNextLevel()
    {
        Debug.Log("LevelManager.LoadNextLevel");
        levelToLoad = currentLevel + 1;
        LoadLevel(levelToLoad);
    }

    public async void LoadLevel(int sceneIndex)
    {
        // Create Async LevelLoader script with a fake 1 second-ish loading screen 
        // Each MetaManager method can refer to the LevelLoader and feed in the level to load

        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        // ADDITIVE LOADING OPTION:
        // (levelToLoad, LoadSceneMode.Additive)
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

}

    public enum MetaState
{
    StartGame,
    LoadLevel,
    EndGame
}



// ==NOTES==
//
// We have GameManager to handle transitions in a single game level. See GameManager notes.
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
