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

using System; // Required for..  public static event Action<>
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{

    public static MetaManager Instance;
    public MetaState _metaState;

    public static event Action<MetaState> OnMetaStateChanged;

    private int currentLevel;
    private int levelToLoad;

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
                HandleStartGame();
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

    public void StartGame()
    {
        // Move StartGame code >> HandleStartGame..
        // Handled by MenuManager
        // MenuManager subscribes to public static event OnGameStateChanged
        // StartGame called from Play Button

        // Move Loading actions to an Async LevelLoader script with a fake 1 second-ish loading screen 
        // Each LevelManager method can refer to the LevelLoader and feed in the level to load
        // Eg below = LevelLoader.LoadLevel(nextLevel)

        MetaManager.Instance.UpdateMetaState(MetaState.LoadLevel); // Update >> LoadLevel state
        SceneManager.LoadScene(currentLevel + 1);
    }


    private void HandleStartGame()
    {
    }

    private void HandleLoadLevel()
    {
        //TEMP QUICK CODE TO CHECK FOR ENDGAME SCENE
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
        LoadLevel(0); 
        // Ensure MainMenu is at BuildIndex = 0 or change input
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

    public void LoadLevel(int sceneIndex)
    {
        levelToLoad = sceneIndex;
        SceneManager.LoadScene(levelToLoad); 
        
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