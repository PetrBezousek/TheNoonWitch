using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WindowColision : MonoBehaviour {

    public delegate void OnNoonWitchSpook(bool isSpooking);
    public event OnNoonWitchSpook OnNoonWitchSpookEvent;

    [SerializeField]
    [Header("Range for opening window (e.g. 1 = witch has to be 1 pixel from window origin/center)")]
    private float range;

    [SerializeField]
    [Header("How long (seconds) is noon witch knocking till trying to open window")]
    private float buchbuchTime = 3f;

    [SerializeField]
    public GameObject noonWitchSpooking;
    [SerializeField]
    public GameObject noonWitchWalking;

    [SerializeField]
    public GameObject latch;

    SoundManager sound;

    public bool isKnocking = false;

    public GameObject lastWindow;

    public bool isSpooking = false;

    private void Awake()
    {
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToNoonWitch(gameObject);
    }

    private void WindowColision_OnUpdateNotifyAboutItselfEvent(GameObject window)
    {
        //am I in range of new window
        if ((window != lastWindow)
            && (Math.Abs(transform.position.x - window.transform.position.x) <= range))
        {
            lastWindow = window;//breaks infinite updating
        }

        //GetComponent<MovementBySimulatedInputHorizontal>().moveState = MovementBySimulatedInputHorizontal.Move.Stay;//becaouse witch always stops
        
        //Noon witch is spooking and player shut window
        if ((window == lastWindow)
            && (Math.Abs(transform.position.x - window.transform.position.x) <= range)
            && !isKnocking
            && !GetComponent<MovementBySimulatedInputHorizontal>().isOnTheWay()
            && window.GetComponent<Window>().windowState != Window.State.Opened)
        {
            isSpooking = false;
            OnNoonWitchSpookEvent(false);//bububu.. pls staph
            isKnocking = true;//breaks infinite update

            //start animation
            window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DORestartById("Knock");
            window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DOPlayById("Knock");

            sound.PlaySound("windowKnock");

            Invoke("TryToOpenWindow", buchbuchTime);//wait 3 seconds (buch! buch)
        }
    }

    private void RaiseOnNoonWitchSpookEvent(bool isSpooking)
    {
        if (OnNoonWitchSpookEvent != null)
        {
            OnNoonWitchSpookEvent(isSpooking);
        }
    }

    private void TryToOpenWindow()
    {
        //stop animation
        lastWindow.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DOPause();
        if (GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase != GamePhases.Phase.EndGame_8)
        {
            isKnocking = false;


            GameObject logic = GameObject.FindGameObjectWithTag("GameLogic");

            if(lastWindow.GetComponent<Window>().windowState != Window.State.Latched)
            {
                noonWitchWalking.SetActive(false);
                noonWitchSpooking.SetActive(true);

                sound.PlaySound("noonWitchScream");
                isSpooking = true;

                //window is closed then (player cannot open windows)
                lastWindow.GetComponent<Window>().ChangeStateTo(Window.State.Opened);
                RaiseOnNoonWitchSpookEvent(true);
                
                GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().HighlightLatchStart();
            }
            else
            {
                if (!isKnocking)
                {
                    RaiseOnNoonWitchSpookEvent(false);
                    GetComponent<MovementBySimulatedInputHorizontal>().MoveToFarerPoint();
                    lastWindow = null;

                    noonWitchSpooking.SetActive(false);
                    noonWitchWalking.SetActive(true);
                }

            }
        }
    }

    //Start listening to item
    public void SubscribeToNewItem(GameObject item)
    {
        item.GetComponent<BroadcastItselfToNoonWitch>().OnUpdateNotifyAboutItselfEvent += WindowColision_OnUpdateNotifyAboutItselfEvent; ;
    }

    //Stop listening to item
    public void UnSubscribeToNewItem(GameObject item)
    {
        item.GetComponent<BroadcastItselfToNoonWitch>().OnUpdateNotifyAboutItselfEvent -= WindowColision_OnUpdateNotifyAboutItselfEvent;
    }
}
