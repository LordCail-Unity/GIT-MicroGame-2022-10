using UnityEngine;

public class LVLLoseUIHandler : MonoBehaviour
{

    public void RestartLevel()
    {
        Debug.Log("Button: Replay");
        MetaManager.Instance.ChangeMetaStateToLevelLose();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        MetaManager.Instance.ChangeMetaStateToMainMenu();
    }

}