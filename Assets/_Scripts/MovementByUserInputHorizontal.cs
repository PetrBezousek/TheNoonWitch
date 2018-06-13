using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByUserInputHorizontal : MonoBehaviour {

    [SerializeField]
    GameObject dashMark;

    //serialize just for testing purpose
    [SerializeField]
    private float maxSpeed = 30f;

    //serialize just for testing purpose
    [SerializeField]
    private float currSpeed = 30f;

    private float moveValue = 1;

    [SerializeField]
    private float minSecondsHoldingKeyToDash = 0.5f;

    [SerializeField]
    private float timeHoldingKeyMultiplier = 1f;//1 = 2s držení tlačítka, 2s dash kupředu ... 0.5 = 2s, 1s

    float delta;
    float chargeMoveTime;
    bool startChargedMove = false;
    KeyCode lastKey;

    Vector3 v3;

    [SerializeField]
    float boundXRight;
    [SerializeField]
    float boundXLeft;

    //Update
    private void UpdateManager_OnUpdateEvent()
    {
        //player input (Left/Right)

        /*
         * Basic movement
         * 
             moveValue = Input.GetAxis("Horizontal");
         if (moveValue != 0)
         {
             //move object left/right
             transform.position = transform.position + (new Vector3(moveValue * speed, 0, 0)* Time.deltaTime);
         }*/


        //Charged movement
        if (!startChargedMove)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {

                lastKey = KeyCode.RightArrow;
                chargeMoveTime += delta * timeHoldingKeyMultiplier;//charging

                if(chargeMoveTime > minSecondsHoldingKeyToDash)
                {
                    dashMark.SetActive(true);
                    // (transform.position.x + (chargeMoveTime*currSpeed))   .....   místo na které hráč dashne
                    dashMark.transform.position = new Vector3(transform.position.x + (chargeMoveTime * currSpeed),dashMark.transform.position.y);
                }

            }else if(chargeMoveTime > minSecondsHoldingKeyToDash && lastKey == KeyCode.RightArrow)
            {
                //charge right
                moveValue = 1;
                startChargedMove = true;
            }


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                

                lastKey = KeyCode.LeftArrow;
                chargeMoveTime += delta * timeHoldingKeyMultiplier;//charging

                if (chargeMoveTime > minSecondsHoldingKeyToDash)
                {
                    dashMark.SetActive(true);
                    // (transform.position.x - (chargeMoveTime*currSpeed))   .....   místo na které hráč dashne
                    dashMark.transform.position = new Vector3(transform.position.x - (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                }
            }
            else if (chargeMoveTime > minSecondsHoldingKeyToDash && lastKey == KeyCode.LeftArrow)
            {
                //charge left
                moveValue = -1;
                startChargedMove = true;
                
            }
        }
        else
        {
            if (chargeMoveTime > 0)
            {
                //test if player hits boundary
                v3 = transform.position + (new Vector3(moveValue * currSpeed, 0, 0) * delta);

                if (v3.x > boundXRight || v3.x < boundXLeft)
                {
                    //stop
                    chargeMoveTime = 0;
                    startChargedMove = false;
                }
                else
                {
                    //move
                    transform.position = transform.position + (new Vector3(moveValue * currSpeed, 0, 0) * delta);
                    chargeMoveTime -= delta;
                    if(moveValue > 0)
                    {
                        dashMark.transform.position = new Vector3(transform.position.x + (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                    }
                    else
                    {
                        dashMark.transform.position = new Vector3(transform.position.x - (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                    }
                    
                }
            }
            else
            {
                startChargedMove = false;
                dashMark.SetActive(false);
            }
        }
        
    }
    

    public void DebuffSpeed(float debuff)
    {
        currSpeed = maxSpeed;
        currSpeed *= debuff;
    }

    private void OnDisable()
    {
            //unsubscribe from Update
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= UpdateManager_OnUpdateEvent;
       
    }
    private void OnEnable()
    {
        delta = Time.deltaTime;
        currSpeed = maxSpeed;

        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += UpdateManager_OnUpdateEvent;
    }
}
