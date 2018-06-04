using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByUserInputHorizontal : MonoBehaviour {

    //serialize just for testing purpose
    [SerializeField]
    private float maxSpeed = 30f;

    //serialize just for testing purpose
    [SerializeField]
    private float speed = 30f;

    private float moveValue = 0;   

    //Fixed update
    private void UpdateManager_OnFixedUpdateEvent()
    {
        //player input (Left/Right)
        moveValue = Input.GetAxis("Horizontal");
        if (moveValue != 0)
        {
            //move object left/right
            transform.position = transform.position + (new Vector3(moveValue * speed, 0, 0)* Time.deltaTime);
        }
    }

    public void DebuffSpeed(float debuff)
    {
        speed = maxSpeed;
        speed *= debuff;
    }

    private void OnDisable()
    {
            //unsubscribe from fixedUpdate
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnFixedUpdateEvent -= UpdateManager_OnFixedUpdateEvent;
       
    }
    private void OnEnable()
    {
        //subscribe to fixedUpdate
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnFixedUpdateEvent += UpdateManager_OnFixedUpdateEvent;
    }
}
