using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    private Rigidbody _rigidbody; // most commonly "rb" instead of "_rigidbody"

    public float forwardForce = 500f;
    public float sidewaysForce = 20f;

    // Can add jumping with an IsGrounded check
    // public float jumpForce; 

    private float xInput;

    // Can add all-direction movement with a zInput check
    // private float zInput; 

    // private bool canMove = false;

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

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        Debug.Log("PlayerController: Subscribed to GameManager_OnGameStateChanged");
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    private void GameManager_OnGameStateChanged(GameState obj)
    {
    }

    private void Update()
    {
        getInputs();
    }

    private void getInputs()
    {
        xInput = Input.GetAxis("Horizontal");

        // Can add all-direction movement with zInput 
        // zInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        MoveForwards();
        MoveSideways();

        // Jump();
    }

    private void MoveForwards()
    {
        _rigidbody.AddForce(0, 0, forwardForce * Time.deltaTime);
    }

    private void MoveSideways()
    {
        _rigidbody.AddForce(xInput * sidewaysForce, 0, 0);

        // Can add forwards/backwards movement with zInput 
        // _rigidbody.AddForce(0, 0, zInput * sidewaysForce);

        // All-direction movement
        // _rigidbody.AddForce(xInput * sidewaysForce, 0, zInput * sidewaysForce);
    }

    // Can add jumping with an IsGrounded check eg
    // (NOT FULL CODE)
    //
    // private void Jump()
    // {
    // IF (IsGrounded == true)
    //   {
    //   _rigidbody.AddForce(0, jumpForce * Time.deltaTime, 0);
    //   }
    // }

    public void SinkingFeeling()
    {
        // SINKING INTO LAVA EFFECT
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.drag = 20;
        Debug.Log("PlayerController: Lava effect activated");
    }

}


//==NOTES==


// if(_rigidbody == null)
// { Debug.Log("PlayerController: Can't find Rigidbody"); }
// else { Debug.Log("PlayerController: Rigidbody locked and loaded"); }
