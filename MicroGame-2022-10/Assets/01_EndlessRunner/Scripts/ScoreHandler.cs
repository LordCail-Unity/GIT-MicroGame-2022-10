using System;
using UnityEngine;
// using UnityEngine.UI; // Not used as we are using TMPro (TextMeshPro) instead
using TMPro;

public class ScoreHandler : MonoBehaviour
{

    private GameObject _player;

    // Change scores to Scriptable Objects
    // These can then be referred to by GameManager
    // or MetaManager for Score Boards (by level) etc

    [HideInInspector] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    // SerializeField = Show private field in Inspector

    [HideInInspector] private int highScore = 0;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        highScore = PlayerPrefs.GetInt("SavedHighScore", highScore);
        // Get highScore from the saved data or set to default 0
        UpdateScoreText();
    }

    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        GetScore();
        CheckHighScore();
        UpdateScoreText();
    }

    private void GetScore()
    {
        var zPosition = _player.transform.position.z;
        score = Mathf.FloorToInt(zPosition);
        scoreText.SetText(score.ToString());
    }

    private void CheckHighScore()
    {
        if(score > highScore)
        {
            highScore = score;
            
            // MOVE SAVE FROM UPDATE
            // TO AN EXIT LEVEL FUNCTION
            // EG WITH SCRIPTABLE OBJECTS
            SaveHighScore();
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("SavedHighScore", highScore);
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }

    // ADD A BUTTON TO MENU TO RESET HIGH SCORE

}