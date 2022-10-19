using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    // public LevelManager _levelManager;
    // Replaced by
    // FindObjectOfType<LevelManager>().LevelRestart();

    private PlayerController _playerController;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _playerController = this.GetComponent<PlayerController>();
        Debug.Log("Player Collision: Player Controller locked and loaded");
        _rigidbody = this.GetComponent<Rigidbody>();
        Debug.Log("Player Collision: Player Rigidbody locked and loaded");
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.collider.tag == "Obstacle")
        {
            _playerController.enabled = false;
            Debug.Log("Movement Disabled: Obstacle");

            // RESTART LEVEL
            // _levelManager.ReloadThisLevel();
            FindObjectOfType<MetaManager>().ReloadThisLevel();
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        if(triggerCollider.tag == "Finish")
        {
            _playerController.enabled = false;
            Debug.Log("Movement Disabled: Finish");

            // COMPLETE LEVEL
            // _levelManager.LoadNextLevel();
            FindObjectOfType<MetaManager>().LoadNextLevel();
        }

        if (triggerCollider.tag == "Killzone")
        {
            _playerController.enabled = false;
            Debug.Log("Movement Disabled: Killzone");

            // SINKING INTO LAVA EFFECT:
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.drag = 20;
            Debug.Log("Movement Disabled: Killzone");

            // RESTART LEVEL
            // _levelManager.ReloadThisLevel();
            FindObjectOfType<MetaManager>().ReloadThisLevel();
        }
    }

}