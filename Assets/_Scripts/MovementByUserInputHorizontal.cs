using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByUserInputHorizontal : MonoBehaviour {  
    
    //serialize just for testing purpose
    [SerializeField]
    private float speed = 30f;

    void Start () {
        //subscribe to fixedUpdate
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnFixedUpdateEvent += UpdateManager_OnFixedUpdateEvent;       
    }

    //Fixed update
    private void UpdateManager_OnFixedUpdateEvent()
    {   
        //player input (Left/Right)
        float move = Input.GetAxis("Horizontal");
        if (move != 0)
        {
            //move object left/right
            transform.position = transform.position + (new Vector3(move*speed, 0, 0)* Time.deltaTime);
        }
    }
    
}
