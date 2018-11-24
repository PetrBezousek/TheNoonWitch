using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSettings : MonoBehaviour {
    
    [SerializeField]
    private Animator anim;

    [SerializeField]
    [Range(0, 2)]
    float speedStay;


    [SerializeField]
    [Range(0, 2)]
    float speedCharging;
    [SerializeField]
    [Range(0, 2)]
    float speedChargingMax;
    [SerializeField]
    [Range(0, 2)]
    float speedRunning;
    [SerializeField]
    [Range(0, 2)]
    float speedRunningFinaly;
    [SerializeField]
    [Range(0, 2)]
    float speedRunStayBlend;
    [SerializeField]
    [Range(0, 2)]
    float speedPickingUpChild;
    [SerializeField]
    [Range(0, 2)]
    float speedHoldingChildLoop;
    [SerializeField]
    [Range(0, 2)]
    float speedScream;

    [SerializeField] Anima2D.SpriteMeshInstance chBody;
    [SerializeField] Anima2D.SpriteMeshInstance chHandLeft;
    [SerializeField] Anima2D.SpriteMeshInstance chHandRight;
    [SerializeField] Anima2D.SpriteMeshInstance chHead;

    [SerializeField]
    MovementToThePoint movePlayerToPoint;

    private string currentAnim;

    [SerializeField]
    MovementByUserInputHorizontal playerMovement;

   /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartScream();
        }
    }*/ 

    private void Start()
    {
        anim.Play("Stay");
        anim.speed = speedStay;
        currentAnim = "Stay";       
    }

    public void StartStay()
    {
        anim.Play("RunStayBlend");
        anim.speed = speedRunStayBlend;
        currentAnim = "RunStayBlend";
    }
    public void StartScream()
    {
        anim.Play("PlayerScream");
        anim.speed = speedScream;
        currentAnim = "PlayerScream";
    }
    public void StartStayFinaly()
    {
        anim.Play("Stay");
        anim.speed = speedStay;
        currentAnim = "Stay";
    }

    public void StartCharging()
    {
        if(currentAnim != "Charging")
        {
            anim.Play("Charging");
            anim.speed = speedCharging;
            currentAnim = "Charging";
        }
    }

    public void StartMaximumCharge()
    {
        if (!playerMovement.startChargedMove)
        {
            anim.Play("ChargingMax");
            anim.speed = speedChargingMax;
            currentAnim = "ChargingMax";
        }
    }

    public void StartRunning()
    {
        if (currentAnim != "RunStayBlend")
        {
            anim.Play("Run");
            anim.speed = speedRunning;
            currentAnim = "Run";
        }
    }
    public void StartRunningFinaly()
    {
        if(playerMovement.chargeMoveTime > 0)//im 100% sure it fixes bug with run animation persisting
        {
            anim.Play("Runing");
            anim.speed = speedRunningFinaly;
            currentAnim = "Runing";
        }
    }
    
    public void StartPickingUpChild()
    {
        anim.Play("PickingUpChild");
        anim.speed = speedPickingUpChild;
        currentAnim = "PickingUpChild";
    }
    public void StartHoldingChildLoop()
    {
        anim.Play("HoldingChildLoop");
        anim.speed = speedHoldingChildLoop;
        currentAnim = "HoldingChildLoop";
    }

    public void PutChildInFront()
    {
        chBody.sortingLayerName = "Player";
        chHandLeft.sortingLayerName ="Player";
        chHandRight.sortingLayerName ="Player";
        chHead.sortingLayerName ="Player";

        chBody.sortingOrder = 550;
        chHandLeft.sortingOrder = 525;
        chHandRight.sortingOrder = 500;
        chHead.sortingOrder = 555;
    }

    public void StartChildGetingPickedUp()
    {
        GameObject.FindGameObjectWithTag("ChildBody").GetComponent<AnimationSettingsChild>().StartPlayerPicksChildUp();
    }
}
