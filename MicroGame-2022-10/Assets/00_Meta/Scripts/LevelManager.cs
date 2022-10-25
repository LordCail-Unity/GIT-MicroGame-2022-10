using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    public int mainMenuSceneIndex = 0;
    public int firstSceneIndex = 1;

    [HideInInspector] public int currentSceneIndex;
    [HideInInspector] public int sceneToLoad;
    [HideInInspector] public int maxLevelCompleted;
    [HideInInspector] public int finalSceneIndex;

    private bool isMainMenuScene;
    private bool isQuitMenuScene;
    private bool isGameScene;

    [HideInInspector] public bool isThisLevelCompleted;

    public bool activateLoadingUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetFinalSceneIndex();
        UpdateCurrentSceneIndex();
        UpdateSceneToLoad(firstSceneIndex);
        Instance.maxLevelCompleted = firstSceneIndex;
        activateLoadingUI = true;
        isGameScene = true;
    }

    private void GetFinalSceneIndex()
    {
        Instance.finalSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        Debug.Log("LevelManager.finalSceneIndex = " + Instance.finalSceneIndex);
    }

    private void UpdateCurrentSceneIndex()
    {
        Instance.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("LevelManager.UpdateCurrentSceneIndex = " + Instance.currentSceneIndex);
        UpdateMaxLevelCompleted();
    }

    public void UpdateMaxLevelCompleted()
    {
        //Put this into a PlayerPrefs save 
        if (Instance.currentSceneIndex > Instance.maxLevelCompleted)
        {
            Instance.maxLevelCompleted = Instance.currentSceneIndex;
        }

        if(isThisLevelCompleted == true)
        {
            Instance.maxLevelCompleted = Instance.currentSceneIndex + 1;
            isThisLevelCompleted = false;
        }
        Debug.Log("UpdateMaxLevelCompleted: " + Instance.maxLevelCompleted);
    }

    private void UpdateSceneToLoad(int SceneIndex)
    {
        Instance.sceneToLoad = SceneIndex;
        Debug.Log("LevelManager.UpdateSceneToLoad = " + Instance.sceneToLoad);
    }

    public void LoadScene()
    {
        Debug.Log("LevelManager.LoadScene: " + Instance.sceneToLoad);
        if (activateLoadingUI == true)
        {
            Debug.Log("LoadScene.activateLoadingUI: " + activateLoadingUI.ToString());
            MetaManager.Instance.UpdateMetaState(MetaState.LoadGameScene);
        }
        if (Instance.sceneToLoad == Instance.finalSceneIndex)
        {
            isMainMenuScene = false;
            isGameScene = false;
            isQuitMenuScene = true;
            activateLoadingUI = false;
        }
        LoadingHandler.Instance.StartAsyncLoadCoroutine(Instance.sceneToLoad, activateLoadingUI);
    }

    public void LoadMainMenu()
    {
        Debug.Log("LevelManager.LoadMainMenu");
        UpdateSceneToLoad(Instance.mainMenuSceneIndex);
        isMainMenuScene = true;
        activateLoadingUI = false;
        LoadScene();
    }

    public void ReloadScene()
    {
        Debug.Log("LevelManager.ReloadScene");
        UpdateSceneToLoad(Instance.currentSceneIndex);
        isGameScene = true;
        activateLoadingUI = true;
        LoadScene();
    }

    public void LoadNextScene()
    {
        Debug.Log("LevelManager.LoadNextScene");
        UpdateSceneToLoad(Instance.currentSceneIndex + 1);
        isGameScene = true;
        activateLoadingUI = true;
        LoadScene();
    }

    public void LoadSceneByIndex(int SceneIndex)
    {
        Debug.Log("LevelManager.LoadSceneByIndex");
        if(SceneIndex == 0)
        {
            SceneIndex = Instance.maxLevelCompleted;
        }
        UpdateSceneToLoad(SceneIndex);
        isGameScene = true;
        activateLoadingUI = true;
        LoadScene();
    }

    public void LoadQuitMenu()
    {
        Debug.Log("LevelManager.LoadQuitMenu");
        UpdateSceneToLoad(Instance.finalSceneIndex);
        isQuitMenuScene = true;
        activateLoadingUI = false;
        LoadScene();
    }

    public void loadingComplete()
    {
        Debug.Log("LevelManager.loadingComplete");
        Debug.Log("isMainMenuScene: " + isMainMenuScene.ToString());
        Debug.Log("isGameScene: " + isGameScene.ToString());
        Debug.Log("isQuitMenuScene: " + isQuitMenuScene.ToString());

        UpdateCurrentSceneIndex();

        if (isQuitMenuScene == true)
        {
            MetaManager.Instance.UpdateMetaState(MetaState.QuitMenu);
            ResetBools();
            return;
        }

        if (isMainMenuScene == true)
        {
            MetaManager.Instance.UpdateMetaState(MetaState.MainMenu);
            ResetBools();
            return;
        }

        if(isGameScene == true)
        {
            MetaManager.Instance.UpdateMetaState(MetaState.GameManager);
            ResetBools();
            return;
        }


    }

    private void ResetBools()
    {
        isMainMenuScene = false;
        isQuitMenuScene = false;
        isGameScene = false;
        activateLoadingUI = false;
    }

    public void QuitApp()
    {
        MetaManager.Instance.UpdateMetaState(MetaState.QuitApp);
    }

}


//UpdateCurrentSceneIndex();

//// THIS CHECK MUST BE BEFORE LOADSCENE
//// Otherwise LoadingScreen will show up before Start & Quit Menus
//// Just let the loading screen show?

// MOVE TO LOADING HANDLER | DO NOT CALL LOADING UI
// SceneManager.LoadScene(Instance.sceneToLoad);


//public void LoadGameScene()
//{
//    Debug.Log("LevelManager.LoadGameScene: " + Instance.sceneToLoad);

//    // MOVE TO LOADING HANDLER | CALL LOADING UI
//    // MetaManager.Instance.UpdateMetaState(MetaState.LoadGameScene);

//    bool activateLoadingUI = true;
//    LoadingHandler.Instance.StartAsyncLoadCoroutine(Instance.sceneToLoad, activateLoadingUI);
//}

//if (Instance.sceneToLoad == Instance.mainMenuSceneIndex)
//{
//    Debug.Log("Instance.sceneToLoad == Instance.mainMenuSceneIndex");
//    SceneManager.LoadScene(Instance.mainMenuSceneIndex);
//    Debug.Log("SceneManager.LoadScene(Instance.mainMenuSceneIndex)");
//    UpdateMetaState(MetaState.MainMenu);
//    return;
//}

//if (Instance.sceneToLoad == Instance.finalSceneIndex)
//{
//    Debug.Log("Instance.sceneToLoad == Instance.finalSceneIndex");
//    SceneManager.LoadScene(Instance.finalSceneIndex);
//    Debug.Log("SceneManager.LoadScene(Instance.finalSceneIndex)");
//    UpdateMetaState(MetaState.QuitMenu);
//    return;
//}

//if (Instance.sceneToLoad != Instance.mainMenuSceneIndex && Instance.sceneToLoad != Instance.finalSceneIndex)
//{
//    Debug.Log("Instance.sceneToLoad != Instance.mainMenuSceneIndex ");
//    Debug.Log("&& Instance.sceneToLoad != Instance.finalSceneIndex");
//    UpdateMetaState(MetaState.LoadScene);
//    return;
//}


// LOAD NEXT SCENE
// TO DO IN THIS STATE
// On Complete Level activated:
// --PLAYER CTRL-- Disable player controller
// --MENU MGR-- Pause before activating COMPLETE screen
// --MENU MGR-- Show COMPLETE screen
// Click to
// (1) Next Level OR 
// (2) Retry (eg to beat high score) OR
// (3) Main Menu
// Hand back to MetaManager to destroy this level / scene