using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// The purpose of this script is to set the player movement by overriding the rigidbody velocity.
    /// This is just for demostration purposes and not useful for the core idea of the project.
    /// Feel free to delete this script.
    /// </summary>
    
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody2D rigidBody2D;

    private int HASH_DIR_X = Animator.StringToHash("DirX");
    private int HASH_DIR_Y = Animator.StringToHash("DirY");
    private const float playerSpeed = 1.5f;

    private enum PlayerState {IDLE,MOVING};
    private PlayerState currentState;
    void Start ()
    {
        currentState = PlayerState.IDLE;
    }
	
	void Update ()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");       
        anim.SetFloat(HASH_DIR_X, inputX);
        anim.SetFloat(HASH_DIR_Y, inputY);        
        Vector2 finalVelocity = (new Vector2(inputX, inputY) ) * playerSpeed;
        if (currentState == PlayerState.IDLE && finalVelocity.magnitude > 0.0f)
        {
            anim.SetTrigger("Move");
            currentState = PlayerState.MOVING;
        }
        else if (currentState == PlayerState.MOVING && finalVelocity.magnitude == 0.0f)
        {
            anim.SetTrigger("Idle");
            currentState = PlayerState.IDLE;
        }
        rigidBody2D.velocity = finalVelocity;
    }
}
