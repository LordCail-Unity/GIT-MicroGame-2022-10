using System.Collections; // Required for IEnumerator
using UnityEngine;
using UnityEngine.UI; // Required for access to Slider objects
using UnityEngine.SceneManagement; // Required for Load Scene

public class LoadingUIHandler : MonoBehaviour
{

    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TMPro.TMP_Text _loadingPercentText;
    [SerializeField] private float loadingUIDelaySecs = .05f;
    // Loading screen is too fast to see so tiny delay

    private void OnEnable()
    {
        Debug.Log("LoadingUIHandler.OnEnable");
        StartProgressBar();
    }

    public void StartProgressBar()
    {
        Debug.Log("LoadingUIHandler.StartProgressBar");
        StartCoroutine(ProgressBar(loadingUIDelaySecs));
    }

    private void UpdateSlider(float sliderValue)
    {
        _loadingSlider.value = sliderValue; // MUST be 0 to 1

        float sliderPercent = Mathf.Round(sliderValue * 100f);
        _loadingPercentText.text = sliderPercent + "%";
        // Debug.Log("UpdateSlider: " + _loadingPercentText.text);
    }

    IEnumerator ProgressBar(float delaySecs)
    {
        Debug.Log("ProgressBar started");

        float sliderValue = 0f;

        while (sliderValue <= 0.9f)
        {
            if (sliderValue <= LoadingHandler.Instance.loadingProgress)
            { sliderValue = sliderValue + .05f; }
            //Debug.Log("sliderPercent:" + sliderPercent * 100 + "%");
            UpdateSlider(sliderValue);

            yield return new WaitForSecondsRealtime(delaySecs);
            //Debug.Log("WhileLoop1 Waited for: " + delaySecs);
        }

        while (sliderValue <= 1f)
        {
            sliderValue = sliderValue + 0.05f;
            UpdateSlider(sliderValue);

            yield return new WaitForSecondsRealtime(delaySecs);
            //Debug.Log("WhileLoop2 Waited for: " + delaySecs);
        }

        LoadingHandler.Instance.loadingScreenComplete = true;
        Debug.Log("SliderUpdate Completed:" + sliderValue * 100 + "%");
        //Debug.Log("Final sliderPercent:" + sliderPercent * 100 + "%");
    }

}

// == NOTES ==

// SET UP UI TO SIMPLY:
// LOOK FOR LOADING HANDLER PROGRESS VARIABLE
// THEN UPDATE SLIDER
// REPEAT
//
// OPTIONAL: USE EVENT & ACTION SYSTEM AND SUBSCRIBE TO IT?

//private void OnEnable()
//{
//    // !!IMPORTANT!!
//    // While Start will only ever be called once after creation,
//    // On Enable is called every time the script component,
//    // or the object it's attached to, is enabled.
//    // This is essential here as we are not destroying and creating LoadingHandler,
//    // we are Enabling and Disabling it as required.

//    Debug.Log("LoadingHandler.OnEnable");
//    GetSceneToLoad();
//    StartAsyncLoadCoroutine();

//    //OnEnable called BEFORE Start
//}

//sliderPercent = sliderPercent + 0.05f;
//// |SAME AS| loadPercent += 0.05f;