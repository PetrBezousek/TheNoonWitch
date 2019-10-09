using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITime : MonoBehaviour {

    [Header("Seconds to survive")]
    [SerializeField] float secondsHardPhase;

    [Header("Seconds to survive")]
    [SerializeField]
    public float secondsImpossiblePhase;

    [SerializeField]
    public bool startClockHard;
    [SerializeField]
    public bool startClockImpossible;

    [SerializeField] Text timeTxt;

    [SerializeField] GameObject psycheStatus;

    private float timerSum;

    private float fillAmountCounter;

    [SerializeField]
    private bool isTweeningTimeStatus = false;

    void Start () {
        timerSum = secondsHardPhase + secondsImpossiblePhase;
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += Time_OnUpdateEvent;
    }

    public void StartThatTimerBro()
    {
        DOTween.To(() => psycheStatus.GetComponent<Image>().fillAmount, x => psycheStatus.GetComponent<Image>().fillAmount = x, 0, 125).SetEase(Ease.InOutSine).OnComplete(() => {
            secondsImpossiblePhase = -1;
        });
    }
    private void Update()
    {
    }

    private void Time_OnUpdateEvent()
    {

        if (startClockHard)
        {
            secondsHardPhase -= UnityEngine.Time.deltaTime;
            if (secondsHardPhase < 0)
            {
                if(GetComponent<GamePhases>().currentPhase == GamePhases.Phase.CompleteHard_6_NO_CHANGE_YET)
                {
                    GetComponent<GamePhases>().StartPhase(GamePhases.Phase.CompleteImpossible_7_NO_CHANGE_YET);
                    startClockHard = false;
                }
                else
                {
                    psycheStatus.GetComponent<DOTweenAnimation>().DORestartById("shake");
                    psycheStatus.GetComponent<DOTweenAnimation>().DOPlayById("shake");
                    startClockHard = false;
                    startClockImpossible = true;
                }
            }
            //TODO porad to blikaaaa
            //float tempFill = (float)Math.Round((Decimal)(((secondsHardPhase + secondsImpossiblePhase) / timerSum) * 100), 0);
            /*
            if (fillAmountCounter != tempFill)
            {
                fillAmountCounter = tempFill;
                DOTween.To(() => psycheStatus.GetComponent<Image>().fillAmount, x => psycheStatus.GetComponent<Image>().fillAmount = x, 0, 120);
            }*/
            timeTxt.text = (Mathf.Floor(secondsHardPhase) + secondsImpossiblePhase).ToString();
        }
        if (startClockImpossible)
        {
            secondsImpossiblePhase -= UnityEngine.Time.deltaTime;

            if (secondsImpossiblePhase < 0 && GetComponent<Psyche>().psycheCurr > 0)
            {
                Debug.Log("000");
                GetComponent<GamePhases>().StartPhase(GamePhases.Phase.EndGame_8);
                startClockImpossible = false;
            }

            timeTxt.text = (Mathf.Floor(secondsImpossiblePhase)).ToString();

        }
    }
}
