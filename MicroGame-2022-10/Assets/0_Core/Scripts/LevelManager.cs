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

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public void StartGame()
    {

        // Move Loading actions to an Async LevelLoader script with a fake 1 second-ish loading screen 
        // Each LevelManager method can refer to the LevelLoader and feed in the level to load
        // Eg below = LevelLoader.LoadLevel(nextLevel)

        GameManager.Instance.UpdateGameState(GameState.LoadLevel); // Update GameState >> LoadLevel state
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.UpdateGameState(GameState.PlayLevel); // Update GameState >> PlayLevel state
    }

    public void LoadMainMenu()
    {
        // Eg below = LevelLoader.LoadLevel(mainMenuLevel)
        SceneManager.LoadScene(0); // 0 = Build Index of Main Menu
    }

    public void ReloadThisLevel()
    {
        // Eg below = LevelLoader.LoadLevel(thisLevel)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // OPTION: buildIndex, LoadSceneMode.Additive
    }

    public void LoadNextLevel()
    {
        Debug.Log("LevelManager.LoadNextLevel");
        
        // Eg below = LevelLoader.LoadLevel(nextLevel)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //TEMP QUICK CODE
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            GameManager.Instance.UpdateGameState(GameState.EndGame);
            Debug.Log("GameState >> EndGame");
        }
        // Update GameState >> EndGame state

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

}