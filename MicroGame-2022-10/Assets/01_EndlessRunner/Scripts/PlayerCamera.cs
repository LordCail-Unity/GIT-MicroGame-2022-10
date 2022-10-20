using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    private GameObject CameraFollowTarget;

    public float xOffset, yOffset, zOffset;

    private void Awake()
    {
        CameraFollowTarget = GameObject.FindWithTag("CameraFollowTarget");
        Debug.Log("Main Camera: CameraFollowTarget locked and loaded");
        //MUST HAVE CameraFollowTarget at center of player gameobject
    }

    private void Update()
    {
        this.transform.position = CameraFollowTarget.transform.position + new Vector3(xOffset, yOffset, zOffset);
        this.transform.LookAt(CameraFollowTarget.transform.position);
    }

}