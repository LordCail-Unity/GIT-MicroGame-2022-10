using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private GameObject Player;
    private Collider _collider;

    public float xOffset, yOffset, zOffset;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        _collider = Player.GetComponent<Collider>();
        Debug.Log("Main Camera: Rigidbody locked and loaded");
    }

    private void Update()
    {
        this.transform.position = _collider.transform.position + new Vector3(xOffset, yOffset, zOffset);
        this.transform.LookAt(_collider.transform.position);
    }

}