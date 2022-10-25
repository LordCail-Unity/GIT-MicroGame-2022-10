using UnityEngine;

public class QuitUIHandler : MonoBehaviour
{

    public void QuitApp()
    {
        Debug.Log("Button: Quit");
        MetaManager.Instance.ChangeMetaStateToQuitApp();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        MetaManager.Instance.LoadMainMenuScene();
    }
    
}