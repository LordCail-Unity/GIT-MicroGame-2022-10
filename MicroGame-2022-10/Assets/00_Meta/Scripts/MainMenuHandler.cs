using UnityEngine;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{

    [HideInInspector] private int highScore = 0;
    [SerializeField] private TextMeshProUGUI highScoreText;


    private void OnEnable()
    {
        highScore = PlayerPrefs.GetInt("SavedHighScore", highScore);
        Debug.Log("Retrieved High Score: " + highScore);

        // Get highScore from the saved data or set to default 0
        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = highScore.ToString();
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("SavedHighScore");
        highScore = 0;
        UpdateHighScoreText();
    }

    //[OPTIONAL]
    //public void ResetPlayerPrefs()
    //{

    //    PlayerPrefs.DeleteAll(); // Delete ALL saved PlayerPrefs data
    //}


}
