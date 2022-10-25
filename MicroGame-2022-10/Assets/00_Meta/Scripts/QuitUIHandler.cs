using UnityEngine;

public class QuitUIHandler : MonoBehaviour
{

    public void QuitApp()
    {
        Debug.Log("Button: Quit");
        LevelManager.Instance.QuitApp();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        LevelManager.Instance.LoadMainMenu();
    }
    
}