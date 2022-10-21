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

    public float loadingUIDelaySecs = .05f; 
    // Loading screen is too fast to see so tiny delay.

    private void Start()
    {
        _metaManager = FindObjectOfType<MetaManager>();
        GetSceneToLoad();
        StartAsyncLoadCoroutine();
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

        // GameManager gets activated as soon as the scene is loaded
        // Try SceneActivation code
        // Also can make sure that GameManager activates StartUI after a trigger,
        // not automatically.

        // REFACTOR
        // https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
        // When allowSceneActivation is set to false, Unity stops progress at 0.9, and
        // maintains.isDone at false. When AsyncOperation.allowSceneActivation is set to true,
        // isDone can complete. While isDone is false, the AsyncOperation queue is stalled. 

        //Prevent scene activation
        //var scene = SceneManager.LoadSceneAsync(sceneIndex);
        //scene.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(delaySecs);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        // OPTIONAL: LoadSceneAsync(sceneIndex, LoadSceneMode.Additive)

        Debug.Log("AsyncLoad Started: sceneIndex = " + sceneIndex);

        float loadPercent = 0f;

        while (!operation.isDone)
        {
            yield return new WaitForSecondsRealtime(delaySecs);
            Debug.Log("Waited for: " + delaySecs);
            if (loadPercent <= operation.progress)
            { loadPercent = loadPercent + .05f; }
            Debug.Log("Operation %:" + loadPercent * 100);
            UpdateSlider(loadPercent);
            yield return null; // waits one frame before proceeding
        }

        Debug.Log("AsyncLoad Completed: sceneIndex = " + sceneIndex);
        Debug.Log("LoadPercent %:" + loadPercent * 100);

        //Why is this NOT delaying for seconds??

        while (loadPercent < 1f)
        {
            yield return new WaitForSecondsRealtime(delaySecs);
            Debug.Log("Waited for: " + delaySecs);

            loadPercent = loadPercent + .05f; // |SAME AS| loadPercent += 0.1f
            UpdateSlider(loadPercent);
            yield return null;
        } 

        loadPercent = 1f;
        _metaManager.LoadingComplete = true;
        Debug.Log(_metaManager.LoadingComplete.ToString());

        yield return new WaitForSecondsRealtime(delaySecs);

        UpdateSlider(loadPercent);
        //Gives the effect of 100% completion just before starting the round
        //Could use DOTween to smooth this but surely we have better things to work on (>_<)

        //scene.allowSceneActivation = true;
        // May be unnecessary if we use Operation method as the DoWhile method relied on 0.9 completion
        // Which yields before Unity's final >0.9 loading operation is complete

        yield return new WaitForSecondsRealtime(delaySecs);
        // Delay to allow SceneActivation to kick in before moving to next action

        _metaManager.TransitionToGameManager();

    }

}