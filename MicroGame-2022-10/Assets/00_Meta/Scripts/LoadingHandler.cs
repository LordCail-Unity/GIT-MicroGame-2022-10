using System.Collections; // Required for IEnumerator
using System.Threading.Tasks; // Required for Task.Delay
using UnityEngine;
using UnityEngine.UI; // Required for access to Slider objects
using UnityEngine.SceneManagement; // Required for Load Scene

public class LoadingHandler : MonoBehaviour
{

    private MetaManager _metaManager;

    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TMPro.TMP_Text _loadingPercentText;

    private int sceneIndexToLoad;

    private float loadingUIDelaySecs = .05f; 
    // Loading screen is too fast to see so tiny delay.

    private void Awake()
    {
        Debug.Log("LoadingHandler.Awake");
        _metaManager = FindObjectOfType<MetaManager>();
    }

    private void OnEnable()
    {
        //While Start will only ever be called once,
        //On Enable is called every time the script component,
        //or the object it's attached to, is enabled.

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
        sceneIndexToLoad = _metaManager.sceneToLoad;
    }

    public void StartAsyncLoadCoroutine()
    {
        StartCoroutine(AsyncLoad(sceneIndexToLoad, loadingUIDelaySecs));
    }

    private void UpdateSlider(float progressPercent)
    {
        _loadingSlider.value = progressPercent;
        _loadingPercentText.text = Mathf.Round(progressPercent * 100f) + "%";
        Debug.Log("UpdateSlider: " + _loadingPercentText.text);
    }

    IEnumerator AsyncLoad(int sceneIndex, float delaySecs)
    {

        yield return new WaitForSecondsRealtime(delaySecs);


        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(sceneIndex); // Can use sceneIndex OR sceneName
        // OPTIONAL: LoadSceneAsync(sceneIndex, LoadSceneMode.Additive)

        _asyncOperation.allowSceneActivation = false;

        Debug.Log("_asyncOperation Started: sceneIndex = " + sceneIndex);

        float loadPercent = 0f;

        while (!_asyncOperation.isDone)
        {
            yield return new WaitForSecondsRealtime(delaySecs);
            Debug.Log("WhileLoop1 Waited for: " + delaySecs);
            if (loadPercent <= _asyncOperation.progress)
            { loadPercent = loadPercent + .05f; }
            Debug.Log("AsyncOperation:" + loadPercent * 100 + "%");
            UpdateSlider(loadPercent);

            _asyncOperation.allowSceneActivation = true;
            Debug.Log("AsyncLoad Completed: SceneActivated = " + sceneIndex);
        }

        // One frame delay to allow SceneActivation to kick in before moving to next action
        yield return null;

        Debug.Log("_asyncOperation Completed: sceneIndex = " + sceneIndex);
        Debug.Log("LoadPercent:" + loadPercent * 100 + "%");

        while (loadPercent <= 0.9f)
        {
            yield return new WaitForSecondsRealtime(delaySecs);
            Debug.Log("WhileLoop2 Waited for: " + delaySecs);

            loadPercent = loadPercent + 0.05f;
            // |SAME AS| loadPercent += 0.05f;
            UpdateSlider(loadPercent);
        }

        loadPercent = 1f;
        UpdateSlider(loadPercent);

        //_metaManager.LoadingComplete = true;
        //Debug.Log(_metaManager.LoadingComplete.ToString());

        yield return new WaitForSecondsRealtime(delaySecs);

        _metaManager.TransitionToGameManager();

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