using UnityEngine;

public class LVLCompleteUIHandler : MonoBehaviour
{

    public void NextLevel()
    {
        Debug.Log("Button: Continue");
        MetaManager.Instance.LoadNextScene();
    }

    public void MainMenu()
    {
        Debug.Log("Button: Main Menu");
        MetaManager.Instance.LoadMainMenuScene();
    }

}