using System.Collections; // Required for IEnumerator
// using System.Threading.Tasks; // Required for Task.Delay used in Async methods
// DON'T USE ASYNC METHOD TYPE AS IT IS INCOMPATIBLE WITH WEBGL
using UnityEngine;
using UnityEngine.UI; // Required for access to Slider objects
using UnityEngine.SceneManagement; // Required for Load Scene

public class LoadingHandler : MonoBehaviour
{
    // MetaManager is a global static instance so don't need this:
    // private MetaManager _metaManager;

    // To call methods simply use MetaManager.Instance.xyzmethod(); 
    // To call variables simply use MetaManager.Instance.xyzvariable; 

    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TMPro.TMP_Text _loadingPercentText;

    private int sceneIndexToLoad;

    private float loadingUIDelaySecs = .05f; 
    // Loading screen is too fast to see so tiny delay.

    private void Awake()
    {
        Debug.Log("LoadingHandler.Awake");
        // MetaManager is a global static instance so don't need this:
        //_metaManager = FindObjectOfType<MetaManager>();

        // To call methods simply use MetaManager.Instance.xyzmethod(); 
        // To call variables simply use MetaManager.Instance.xyzvariable; 
    }

    private void OnEnable()
    {
        // !!IMPORTANT!!
        // While Start will only ever be called once after creation,
        // On Enable is called every time the script component,
        // or the object it's attached to, is enabled.
        // This is essential here as we are not destroying and creating LoadingHandler,
        // we are Enabling and Disabling it as required.

        Debug.Log("LoadingHandler.OnEnable");
        GetSceneToLoad();
        StartAsyncLoadCoroutine();

        //OnEnable called BEFORE Start
    }

    private void Start()
    {
    }

    private void GetSceneToLoad()
    {
        sceneIndexToLoad = MetaManager.Instance.sceneToLoad;
        Debug.Log("LoadingHandler: GetSceneToLoad = " + sceneIndexToLoad);
    }

    public void StartAsyncLoadCoroutine()
    {
        Debug.Log("LoadingHandler: StartAsyncLoadCoroutine");
        StartCoroutine(AsyncLoad(sceneIndexToLoad, loadingUIDelaySecs));
    }

    private void UpdateSlider(float progressPercent)
    {
        _loadingSlider.value = progressPercent;
        _loadingPercentText.text = Mathf.Round(progressPercent * 100f) + "%";
        // Debug.Log("UpdateSlider: " + _loadingPercentText.text);
    }

    IEnumerator AsyncLoad(int sceneIndex, float delaySecs)
    {
        Debug.Log("LoadingHandler: AsyncLoad started");

        yield return new WaitForSecondsRealtime(delaySecs);


        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(sceneIndex); // Can use sceneIndex OR sceneName
        // OPTIONAL: LoadSceneAsync(sceneIndex, LoadSceneMode.Additive)

        _asyncOperation.allowSceneActivation = false;

        //Debug.Log("_asyncOperation Started: sceneIndex = " + sceneIndex);

        float sliderPercent = 0f;

        while (!_asyncOperation.isDone)
        {
            yield return new WaitForSecondsRealtime(delaySecs);
            //Debug.Log("WhileLoop1 Waited for: " + delaySecs);

            if (sliderPercent <= _asyncOperation.progress)
            { sliderPercent = sliderPercent + .05f; }
            //Debug.Log("sliderPercent:" + sliderPercent * 100 + "%");
            UpdateSlider(sliderPercent);

            _asyncOperation.allowSceneActivation = true;
            Debug.Log("_asyncOperation: SceneActivated = " + sceneIndex);
        }

        // One frame delay to allow SceneActivation to kick in before moving to next action
        yield return null;

        Debug.Log("_asyncOperation Completed: sceneIndex = " + sceneIndex);
        //Debug.Log("_asyncOperation sliderPercent:" + sliderPercent * 100 + "%");

        while (sliderPercent <= 1f)
        {
            sliderPercent = sliderPercent + 0.05f;
            // |SAME AS| loadPercent += 0.05f;
            UpdateSlider(sliderPercent);

            yield return new WaitForSecondsRealtime(delaySecs);
            //Debug.Log("WhileLoop2 Waited for: " + delaySecs);
        }

        Debug.Log("SliderUpdate Completed:" + sliderPercent * 100 + "%");
        //Debug.Log("Final sliderPercent:" + sliderPercent * 100 + "%");

        yield return new WaitForSecondsRealtime(delaySecs);

        MetaManager.Instance.ChangeMetaStateToGameManager();

    }

}


// ==NOTES==

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