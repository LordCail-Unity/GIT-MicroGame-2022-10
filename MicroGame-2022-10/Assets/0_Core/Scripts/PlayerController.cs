using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float forwardForce = 500f;
    public float sidewaysForce = 20f;
    // public float jumpForce;

    private Rigidbody _rigidbody; // most commonly "rb" instead of "_rigidbody"

    private float xInput;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        Debug.Log("Player Controller: Rigidbody locked and loaded");
    }

    private void Update()
    {
        getInputs();
    }

    private void getInputs()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        _rigidbody.AddForce(0, 0, forwardForce * Time.deltaTime);
        _rigidbody.AddForce(xInput * sidewaysForce, 0, 0);
    }

}