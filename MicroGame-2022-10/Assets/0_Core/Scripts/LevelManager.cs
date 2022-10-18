using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // 0 = Build Index of Main Menu
    }

    public void ReloadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // OPTION: buildIndex, LoadSceneMode.Additive
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT APPLICATION");
    }

}