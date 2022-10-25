using System.Collections; // Required for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // Required for Load Scene

public class LoadingHandler : MonoBehaviour
{

    public static LoadingHandler Instance;
    // Public because accessed by LevelManager 

    [HideInInspector] public float loadingProgress;
    // Public because accessed by LoadingUIHandler

    [HideInInspector] public bool loadingScreenComplete = false;

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

    public void StartAsyncLoadCoroutine(int sceneIndexToLoad, bool activateLoadingUI)
    {
        Debug.Log("LoadingHandler.StartAsyncLoadCoroutine");
        Debug.Log("ActivateLoadingUI = " + activateLoadingUI.ToString());

        if (activateLoadingUI == false)
        { loadingScreenComplete = true; }
        else { loadingScreenComplete = false; }
        
        StartCoroutine(AsyncLoad(sceneIndexToLoad));
    }

    IEnumerator AsyncLoad(int sceneIndex)
    {
        Debug.Log("LoadingHandler: AsyncLoad started");

        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        Debug.Log("_asyncOperation Started: sceneIndex = " + sceneIndex);

        _asyncOperation.allowSceneActivation = false;

        while (!_asyncOperation.isDone)
        {
            loadingProgress = _asyncOperation.progress;

            // Check if the Operation has finished
            // AND LoadingScreen is complete
            if (_asyncOperation.progress >= 0.9f && loadingScreenComplete == true)
            {
                //Activate the Scene
                _asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        Debug.Log("_asyncOperation.isDone: sceneIndex = " + sceneIndex);
        //Debug.Log("_asyncOperation sliderPercent:" + sliderPercent * 100 + "%");

        LevelManager.Instance.loadingComplete();
    }

}



// ==NOTES==

// using System.Threading.Tasks; // Required for Task.Delay used in Async methods
// DON'T USE "PUBLIC VOID ASYNC" METHOD TYPE AS IT IS INCOMPATIBLE WITH WEBGL
// DO USE Coroutines: "IENUMERATOR" method instead


// ONENABLE stuff below only applies to the LoadingUI which is activated on State change
// NOT the LoadingHandler which is permanently active under MetaManager

// !!IMPORTANT!!
// While Start will only ever be called once after creation,
// On Enable is called every time the script component,
// or the object it's attached to, is enabled.
// This is essential here as we are not destroying and creating LoadingHandler,
// we are Enabling and Disabling it as required.

//private void OnEnable()
//{
//    Debug.Log("LoadingHandler.OnEnable");
//    GetSceneToLoad();
//    StartAsyncLoadCoroutine();
//}

// Can use LoadSceneAsync(sceneIndex) OR sceneName
// OPTIONAL: LoadSceneAsync(sceneIndex, LoadSceneMode.Additive)


// Need to figure out how to wait for the loading screen slider to finish BEFORE activating scene
// IF we are loading a Game Scene and using the loading screen slider


// MetaManager is a global static instance so don't need this:
// private MetaManager _metaManager;

// To call methods simply use MetaManager.Instance.xyzmethod(); 
// To call variables simply use MetaManager.Instance.xyzvariable; 


// GameManager gets activated as soon as the scene is loaded
// This activates the GameUI which is overlaying the LoadingUI 
// making it look like the LoadingUI isn't working even though it is
//
// We can:
// (1) Delay SceneActivation
// -- but does this delay the GameSetup?
// (2) Delay GameUI until a condition is met
// -- Probably preferable as it gives us more control and ensures everything
// is set up and ready to go as soon as the switch is flipped..
//
// Try SceneActivation code
// Also can make sure that GameManager activates StartUI after a trigger,
// not automatically.

// REFACTOR
// https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
// When allowSceneActivation is set to false, Unity stops progress at 0.9, and
// maintains.isDone at false. When AsyncOperation.allowSceneActivation is set to true,
// isDone can complete. While isDone is false, the AsyncOperation queue is stalled. 

//var sceneName = SceneManager.GetSceneByBuildIndex(sceneIndex).name; 


//Gives the effect of 100% completion just before starting the round
//Could use DOTween to smooth this but surely we have better things to work on (>_<)

//scene.allowSceneActivation = true;
// May be unnecessary if we use Operation method as the DoWhile method relied on 0.9 completion
// Which yields before Unity's final >0.9 loading operation is complete

// MetaManager.Instance.LoadingComplete = true;
// Debug.Log(MetaManager.Instance.LoadingComplete.ToString());