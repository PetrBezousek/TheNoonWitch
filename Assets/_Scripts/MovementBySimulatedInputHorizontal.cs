using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBySimulatedInputHorizontal : MonoBehaviour {
    
    [Header("A and B position of her patrol (in pixels, game window is 18 pixels long)")]
    [SerializeField]
    int boundaryLeft= -30;
    [SerializeField]
    int boundaryRight = 30;

    //serialize just for testing purpose
    [Header("Speed in pixels per second")]
    [SerializeField]
    public float speed = 30f;
    float moveValue = 0;

    private Move lastDirection;

    public enum Move { Left, Right, Stay}
    public Move moveState = new Move();

    void Start()
    {
        moveState = Move.Left;
    }

    private void OnEnable()
    {
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += UpdateManager_OnFixedUpdateEvent;
    }

    //update
    private void UpdateManager_OnFixedUpdateEvent()
    {
        
        //simulated input (Left/Right/stay)
        switch (moveState)
        {
            case Move.Left:
                moveValue = -1;
                break;
            case Move.Right:
                moveValue = 1;
                break;
            case Move.Stay:
                if(moveValue == 1)
                {
                    lastDirection = Move.Right;
                }
                else if (moveValue == -1)
                {
                    lastDirection = Move.Left;
                }
                moveValue = 0;

                break;
        }
        if (moveValue != 0)
        {
            //move object left/right
            transform.position = transform.position + (new Vector3(moveValue * speed, 0, 0) * UnityEngine.Time.deltaTime);
            if(transform.position.x < boundaryLeft)
            {
                moveState = Move.Right;
            }
            if (transform.position.x > boundaryRight)
            {
                moveState = Move.Left;
            }
        }
    }

    public void ResumeMoving()
    {
        moveState = lastDirection;
    }

    private void OnDisable()
    {
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= UpdateManager_OnFixedUpdateEvent;
    }
}
