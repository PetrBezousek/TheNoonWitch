using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementByUserInputHorizontal : MonoBehaviour {

    [SerializeField]
    FixedPosition ObjectImagePositionScript;

    [SerializeField]
    GameObject dashMark;

    [SerializeField]
    public AnimatorSettings anim;

    //serialize just for testing purpose
    [Space]
    [Header("Speed with empty hands")]
    [SerializeField]
    private float maxSpeed = 30f;

    //serialize just for testing purpose
    [Space]
    [Header("Current ingame speed")]
    [SerializeField]
    private float currSpeed = 30f;

    private float moveValue = 1;

    [Space]
    [Header("How long player needs to hold key to be able to dash (in sec)")]
    [SerializeField]
    private float minSecondsHoldingKeyToDash = 0.5f;

    [Space]
    [Header("1 = 2sec hold key, dash for 2 sec ... 4 = 2sec hold key, dash for 0.5sec")]
    [SerializeField]
    private float timeHoldingKeyMultiplier = 1f;//1 = 2s držení tlačítka, 2s dash kupředu ... 0.5 = 2s, 1s
    
    float delta;
    public float chargeMoveTime;
    public bool startChargedMove = false;
    KeyCode lastKey;

    public bool isNotMoving = true;

    Vector3 v3;

    [SerializeField]
    public float boundXRight;
    [SerializeField]
    public float boundXLeft;

    private SoundManager sound;

    [SerializeField] GameObject tutorArrowLeft;
    [SerializeField] GameObject tutorArrowRight;

    private void Awake()
    {
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void hideTutorArrows()
    {
        isNotMoving = false;
        tutorArrowLeft.SetActive(false);
        tutorArrowRight.SetActive(false);
        // tutorArrowLeft.GetComponent<FixedPosition>().Hide();
        // tutorArrowRight.GetComponent<FixedPosition>().Hide();
    }

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
                hideTutorArrows();
                lastKey = KeyCode.RightArrow;
                chargeMoveTime += Time.deltaTime * timeHoldingKeyMultiplier;//charging

                if(chargeMoveTime > minSecondsHoldingKeyToDash)
                {
                    anim.StartCharging();
                    ObjectImagePositionScript.FlipX(true);
                    dashMark.GetComponent<SpriteRenderer>().DOFade(1, 0.2f);
                    // (transform.position.x + (chargeMoveTime*currSpeed))   .....   místo na které hráč dasahne
                    if (transform.position.x + (chargeMoveTime * currSpeed) < boundXRight)
                    {
                        dashMark.transform.position = new Vector3(transform.position.x + (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                    }
                    else
                    {
                        dashMark.transform.position = new Vector3(boundXRight, dashMark.transform.position.y);
                    }
                }

            }else if(chargeMoveTime > minSecondsHoldingKeyToDash && lastKey == KeyCode.RightArrow)
            {
                //START charge right
                sound.PlaySound("Tap");
                anim.StartRunning();
                ObjectImagePositionScript.FlipX(true);
                moveValue = 1;
                startChargedMove = true;
            }


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                hideTutorArrows();
                lastKey = KeyCode.LeftArrow;
                chargeMoveTime += Time.deltaTime * timeHoldingKeyMultiplier;//charging

                if (chargeMoveTime > minSecondsHoldingKeyToDash)
                {
                    anim.StartCharging();
                    ObjectImagePositionScript.FlipX(false);
                    dashMark.GetComponent<SpriteRenderer>().DOFade(1, 0.2f);
                    // (transform.position.x - (chargeMoveTime*currSpeed))   .....   místo na které hráč dashne
                    if (transform.position.x - (chargeMoveTime * currSpeed) > boundXLeft)
                    {
                        dashMark.transform.position = new Vector3(transform.position.x - (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                    }
                    else
                    {
                        dashMark.transform.position = new Vector3(boundXLeft, dashMark.transform.position.y);
                    }
                }
            }
            else if (chargeMoveTime > minSecondsHoldingKeyToDash && lastKey == KeyCode.LeftArrow)
            {
                //START charge left
                sound.PlaySound("Tap");
                anim.StartRunning();
                ObjectImagePositionScript.FlipX(false);
                moveValue = -1;
                startChargedMove = true;
                
            }
        }
        else //player is dashing
        {
            if (chargeMoveTime > 0)
            {
                //test if player hits boundary
                v3 = transform.position + (new Vector3(moveValue * currSpeed, 0, 0) * Time.deltaTime);

                if (v3.x > boundXRight || v3.x < boundXLeft)
                {
                    //stop
                    Stop();
                }
                else
                {
                    //move
                    transform.position = transform.position + (new Vector3(moveValue * currSpeed, 0, 0) * Time.deltaTime);
                    chargeMoveTime -= Time.deltaTime;
                    if(moveValue > 0)
                    {
                        if (transform.position.x + (chargeMoveTime * currSpeed) < boundXRight)
                        {
                            dashMark.transform.position = new Vector3(transform.position.x + (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                        }
                        else
                        {
                            dashMark.transform.position = new Vector3(boundXRight, dashMark.transform.position.y);
                        }
                    }
                    else
                    {
                        if (transform.position.x - (chargeMoveTime * currSpeed) > boundXLeft)
                        {
                            dashMark.transform.position = new Vector3(transform.position.x - (chargeMoveTime * currSpeed), dashMark.transform.position.y);
                        }
                        else
                        {
                            dashMark.transform.position = new Vector3(boundXLeft, dashMark.transform.position.y);
                        }
                    }
                    
                }
            }
            else
            {
                Stop();
            }
        }
        
    }

    public void Stop()
    {
        chargeMoveTime = 0;
        sound.StopSound("Tap");
        anim.StartStay();
        startChargedMove = false;
        dashMark.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
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

        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += UpdateManager_OnUpdateEvent;
    }
}
