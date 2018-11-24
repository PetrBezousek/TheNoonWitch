using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour {

    [Header("Seconds to survive")]
    [SerializeField] float secondsHardPhase;

    [Header("Seconds to survive")]
    [SerializeField]
    float secondsImpossiblePhase;

    public bool startClockHard;
    public bool startClockImpossible;

    [SerializeField] Text timeTxt;


	void Start () {

        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += Time_OnUpdateEvent; ;
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
                    startClockHard = false;
                    startClockImpossible = true;
                }
            }
            timeTxt.text = (Mathf.Floor(secondsHardPhase) + secondsImpossiblePhase).ToString();
        }
        if (startClockImpossible)
        {
            secondsImpossiblePhase -= UnityEngine.Time.deltaTime;

            if (secondsImpossiblePhase < 0)
            {
                GetComponent<GamePhases>().StartPhase(GamePhases.Phase.EndGameWin_9_DONT_USE);
                startClockImpossible = false;
            }

            timeTxt.text = (Mathf.Floor(secondsImpossiblePhase)).ToString();

        }
    }
}
