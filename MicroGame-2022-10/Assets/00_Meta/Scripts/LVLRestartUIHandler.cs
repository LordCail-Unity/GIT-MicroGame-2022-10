using UnityEngine;

public class LVLRestartUIHandler : MonoBehaviour
{

    public void RestartLevel()
    {
        Debug.Log("Button: Replay");
        LevelManager.Instance.ReloadScene();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        LevelManager.Instance.LoadMainMenu();
    }

}