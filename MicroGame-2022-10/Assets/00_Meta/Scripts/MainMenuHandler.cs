using UnityEngine;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{

    [HideInInspector] private int highScore = 0;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("SavedHighScore", highScore);
        // Get highScore from the saved data or set to default 0
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        highScoreText.text = highScore.ToString();
        highScore = 0;
        UpdateScoreText();
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("SavedHighScore");
    }

    public void ResetPlayerPrefs()
    {
        //[OPTIONAL]
        PlayerPrefs.DeleteAll(); // Delete ALL saved PlayerPrefs data
    }


}
