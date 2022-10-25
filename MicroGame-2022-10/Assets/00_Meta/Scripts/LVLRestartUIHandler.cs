using UnityEngine;

public class LVLRestartUIHandler : MonoBehaviour
{

    public void RestartLevel()
    {
        Debug.Log("Button: Replay");
        MetaManager.Instance.LoadThisScene();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        MetaManager.Instance.LoadMainMenuScene();
    }

}