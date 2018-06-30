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
    

    private void Start()
    {
        anim.Play("Stay");
        anim.speed = speedStay;
    }

    public void StartStay()
    {
        anim.Play("RunStayBlend");
        anim.speed = speedRunStayBlend;
    }
    public void StartStayFinaly()
    {
        anim.Play("Stay");
        anim.speed = speedStay;
    }

    public void StartCharging()
    {
        anim.Play("Charging");
        anim.speed = speedCharging;
    }

    public void StartMaximumCharge()
    {
        anim.Play("ChargingMax");
        anim.speed = speedChargingMax;
    }

    public void StartRunning()
    {
        anim.Play("Run");
        anim.speed = speedRunning;
    }
    public void StartRunningFinaly()
    {
        anim.Play("Runing");
        anim.speed = speedRunningFinaly;
    }
}
