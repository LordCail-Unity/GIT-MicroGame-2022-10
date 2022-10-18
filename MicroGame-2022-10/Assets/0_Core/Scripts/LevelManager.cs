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
        GameManager.Instance.UpdateGameState(GameState.PlayLevel); // Update GameState >> LoadLevel state
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
        // Eg below = LevelLoader.LoadLevel(nextLevel)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

}