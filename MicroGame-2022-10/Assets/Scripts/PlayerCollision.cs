using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = this.GetComponent<PlayerController>();
        Debug.Log("Player Collision: Player Controller locked and loaded");
    }

    private void OnCollisionEnter(Collision collidedObject)
    {
        if (collidedObject.collider.tag == "Obstacle")
        {
            _playerController.enabled = false;
            Debug.Log("Movement Disabled: Obstacle");
            // Transition to next scene
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        if(triggerCollider.tag == "Finish")
        {
            _playerController.enabled = false;
            Debug.Log("Movement Disabled: Finish");
            // Transition to next scene
        }
    }

}