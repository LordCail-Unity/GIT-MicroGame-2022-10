using UnityEngine;

public class LVLCompleteUIHandler : MonoBehaviour
{

    public void NextLevel()
    {
        Debug.Log("Button: Continue");
        LevelManager.Instance.LoadNextScene();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        LevelManager.Instance.LoadMainMenu();
    }

}