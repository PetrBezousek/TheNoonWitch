using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PsycheStatus : MonoBehaviour {

    [SerializeField]
    private bool isPrimaryStatusBar;

    [SerializeField]
    private float timerMax;

    [SerializeField]
    private float timerCurr;

    private bool lerp;

    private float psyche;

    private float psycheMax;

    [SerializeField]
    [Range(0,5)]
    private float psycheLossSpeed;//speed of transition

    [SerializeField] GameObject psycheStatus;

    // Use this for initialization
    void Start () {

        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().OnPsycheChangedEvent += PsycheStatus_OnPsycheChangedEvent;
        psyche = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheCurr;
        psycheMax = psyche;
        timerCurr = timerMax;
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += PsycheStatus_OnUpdateEvent;
    }

    /*
    private void Tick()
    {
        //tohle se beztak vola kazdou vterinu
        timerCurr--;
        if (isPrimaryStatusBar) { GetComponent<Image>().fillAmount = timerCurr / timerMax; }
        Invoke("StartLerp", 0.5f);//little delay so that player can notice change

        //do a transition between old and new value
        if ((!isPrimaryStatusBar) && lerp)
        {
            //if it reached new value, stop
            if (GetComponent<Image>().fillAmount - psycheLossSpeed * Time.deltaTime < timerCurr / timerMax)
            {
                GetComponent<Image>().fillAmount = timerCurr / timerMax;
                lerp = false;//stop
            }
            else
            {
                GetComponent<Image>().fillAmount -= psycheLossSpeed * Time.deltaTime;
            }
        }
    }*/

    private void PsycheStatus_OnUpdateEvent()
    {
        /*
        //do a transition between old and new value
        if ((!isPrimaryStatusBar) && lerp)
        {
            //if it reached new value, stop
            if (GetComponent<Image>().fillAmount - psycheLossSpeed * Time.deltaTime < timerCurr / timerMax)
            {
                GetComponent<Image>().fillAmount = timerCurr / timerMax;
                lerp = false;//stop
            }
            else
            {
                GetComponent<Image>().fillAmount -= psycheLossSpeed * Time.deltaTime;
            }
        }

         OLD
        //do a transition between old and new value
        if ((!isPrimaryStatusBar)&&lerp)
        {
            //if it reached new value, stop
            if (GetComponent<Image>().fillAmount - psycheLossSpeed * Time.deltaTime < psyche/ psycheMax)
            {
                GetComponent<Image>().fillAmount = psyche/psycheMax;
                lerp = false;//stop
            }
            else
            {
                GetComponent<Image>().fillAmount -= psycheLossSpeed * Time.deltaTime;
            }
        }
        */

    }

    private void StartLerp()
    {
        lerp = true;
    }

    private void PsycheStatus_OnPsycheChangedEvent(float currentPsyche)
    {

        //OLD
        /*
        psyche = currentPsyche;
        if (isPrimaryStatusBar) { GetComponent<Image>().fillAmount = currentPsyche / psycheMax; }
        Invoke("StartLerp", 0.5f);//little delay so that player can notice change
        */
    }


}
