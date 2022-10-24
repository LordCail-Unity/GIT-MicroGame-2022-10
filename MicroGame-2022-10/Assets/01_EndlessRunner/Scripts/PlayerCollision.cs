using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.collider.tag == "Obstacle")
        {
            Debug.Log("Collision: Obstacle");

            Debug.Log("LevelCompleted T/F:" + GameManager.Instance.levelCompleted.ToString());

            GameManager.Instance.UpdateGameState(GameState.ExitLevel);
            Debug.Log("GameManager State >> ExitLevel");
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.tag == "Killzone")
        {
            Debug.Log("Trigger Activated: Killzone");

            PlayerController.Instance.SinkingFeeling();

            Debug.Log("LevelCompleted T/F:" + GameManager.Instance.levelCompleted.ToString());

            GameManager.Instance.UpdateGameState(GameState.ExitLevel);
            Debug.Log("GameManager State >> ExitLevel");
        }
        if (triggerCollider.tag == "Finish")
        {
            Debug.Log("Trigger Activated: Finish");

            GameManager.Instance.levelCompleted = true; // Default = false;
            Debug.Log("LevelCompleted T/F:" + GameManager.Instance.levelCompleted.ToString());

            GameManager.Instance.UpdateGameState(GameState.ExitLevel);
            Debug.Log("GameManager State >> ExitLevel");
        }
    }

}


// ==NOTES==

// public LevelManager _levelManager;
// Replaced by
// FindObjectOfType<LevelManager>().LevelRestart();

// private PlayerController _playerController;
// AWAKE _playerController = this.GetComponent<PlayerController>();
// Replaced by GameManager State