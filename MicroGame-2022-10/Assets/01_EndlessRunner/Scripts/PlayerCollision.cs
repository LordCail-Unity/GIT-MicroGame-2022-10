using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public static PlayerCollision Instance;
    // Used by GameManager to disable Collision on ExitLevel event
    // Is creating a singleton here overkill?

    private bool triggerActivated = false;

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

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        // if (!PlayerCollision.Instance.enabled) return;

        if (triggerActivated == true) return;

        if (collidedObject.collider.tag == "Obstacle")
        {
            Debug.Log("Collision: Obstacle");

            Debug.Log("LevelCompleted T/F:" + GameManager.Instance.levelCompleted.ToString());

            triggerActivated = true;

            GameManager.Instance.UpdateGameState(GameState.ExitLevel);
            Debug.Log("GameManager State >> ExitLevel");
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        // if (!PlayerCollision.Instance.enabled) return;

        // Could create separate tag for the type of killzone here
        // Eg a poison killzone etc
        if (triggerCollider.tag == "Killzone")
        {
            Debug.Log("Trigger Activated: Sinking");
            PlayerController.Instance.SinkingFeeling();
        }

        if (triggerActivated == true) return;

        if (triggerCollider.tag == "Killzone")
        {
            Debug.Log("Trigger Activated: Killzone");
            Debug.Log("LevelCompleted T/F:" + GameManager.Instance.levelCompleted.ToString());

            triggerActivated = true;

            GameManager.Instance.UpdateGameState(GameState.ExitLevel);
            Debug.Log("GameManager State >> ExitLevel");
        }

        if (triggerCollider.tag == "Finish")
        {
            Debug.Log("Trigger Activated: Finish");

            GameManager.Instance.levelCompleted = true; // Default = false;
            Debug.Log("LevelCompleted T/F:" + GameManager.Instance.levelCompleted.ToString());

            triggerActivated = true;

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