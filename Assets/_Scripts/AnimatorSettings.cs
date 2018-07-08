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

    private string currentAnim;
    

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
        anim.Play("ChargingMax");
        anim.speed = speedChargingMax;
        currentAnim = "ChargingMax";
    }

    public void StartRunning()
    {
        anim.Play("Run");
        anim.speed = speedRunning;
        currentAnim = "Run";
    }
    public void StartRunningFinaly()
    {
        anim.Play("Runing");
        anim.speed = speedRunningFinaly;
        currentAnim = "Runing";
    }
}
