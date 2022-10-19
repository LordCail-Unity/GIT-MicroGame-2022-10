using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.collider.tag == "Obstacle")
        {
            Debug.Log("Collision: Obstacle");

            GameManager.Instance.UpdateGameState(GameState.RestartLevel);
            Debug.Log("GameManager State >> RestartLevel");
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.tag == "Killzone")
        {
            Debug.Log("Trigger Activated: Killzone");

            FindObjectOfType<PlayerController>().SinkingFeeling();

            GameManager.Instance.UpdateGameState(GameState.RestartLevel);
            Debug.Log("GameManager State >> RestartLevel");
        }
        if (triggerCollider.tag == "Finish")
        {
            Debug.Log("Trigger Activated: Finish");

            GameManager.Instance.UpdateGameState(GameState.CompleteLevel);
            Debug.Log("GameManager State >> CompleteLevel");
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