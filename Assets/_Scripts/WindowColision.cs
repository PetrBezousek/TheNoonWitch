using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public bool isKnocking = false;

    public GameObject lastWindow;

    public bool isSpooking = false;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToNoonWitch(gameObject);
    }

    private void WindowColision_OnUpdateNotifyAboutItselfEvent(GameObject window)
    {
        //am I in range of new window
        if((window != lastWindow)
            &&(Math.Abs(transform.position.x - window.transform.position.x) < range))
        {
            lastWindow = window;//breaks infinite updating

            GetComponent<MovementBySimulatedInputHorizontal>().moveState = MovementBySimulatedInputHorizontal.Move.Stay;//becaouse witch always stops

            switch (window.GetComponent<Window>().windowState)
            {
                case Window.State.Closed:
                    isKnocking = true;

                    //start animation
                    window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DORestartById("Knock");
                    window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DOPlayById("Knock");
                   

                    Invoke("TryToOpenWindow", buchbuchTime);//wait 3 seconds (buch! buch)
                    break;
                case Window.State.Opened:
                    isSpooking = true;
                    OnNoonWitchSpookEvent(true);//bububu
                    break;
                case Window.State.Latched:
                    isKnocking = true;

                    //start animation
                    window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DORestartById("Knock");
                    window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DOPlayById("Knock");

                    Invoke("TryToOpenWindow", buchbuchTime);//wait 3 seconds (buch! buch)
                    break;
            }
        }
        //Noon witch is spooking and player shut window
        if ((window == lastWindow)
            && (Math.Abs(transform.position.x - window.transform.position.x) < range)
            && !isKnocking
            && window.GetComponent<Window>().windowState != Window.State.Opened
            && GetComponent<MovementBySimulatedInputHorizontal>().moveState == MovementBySimulatedInputHorizontal.Move.Stay)
        {

            isSpooking = false;
            OnNoonWitchSpookEvent(false);//bububu.. pls staph
            isKnocking = true;//breaks infinite update

            //start animation
            window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DORestartById("Knock");
            window.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DOPlayById("Knock");

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
        isKnocking = false;

        //stop animation
        lastWindow.GetComponent<Window>().FrameClosed.GetComponent<DOTweenAnimation>().DOPause();

        if(lastWindow.GetComponent<Window>().windowState != Window.State.Latched)
        {
            noonWitchWalking.SetActive(false);
            noonWitchSpooking.SetActive(true);

            isSpooking = true;

            //window is closed then (player cannot open windows)
            lastWindow.GetComponent<Window>().ChangeStateTo(Window.State.Opened);
            RaiseOnNoonWitchSpookEvent(true);
        }
        else
        {
            RaiseOnNoonWitchSpookEvent(false);
            GetComponent<MovementBySimulatedInputHorizontal>().MoveToFarerPoint();

            noonWitchSpooking.SetActive(false);
            noonWitchWalking.SetActive(true);
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
