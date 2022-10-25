using UnityEngine;

public class LVLWinUIHandler : MonoBehaviour
{

    public void NextLevel()
    {
        Debug.Log("Button: Continue");
        MetaManager.Instance.ChangeMetaStateToLevelWin();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        MetaManager.Instance.ChangeMetaStateToMainMenu();
    }

}