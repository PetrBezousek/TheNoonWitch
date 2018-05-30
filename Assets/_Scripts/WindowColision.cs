using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowColision : MonoBehaviour {

    public delegate void OnNoonWitchSpook(bool isSpooking);
    public event OnNoonWitchSpook OnNoonWitchSpookEvent;

    [SerializeField]
    private float range;

    private bool isKnocking = false;

    public GameObject lastWindow;

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
                    Invoke("TryToOpenWindow", 3f);//wait 3 seconds (buch! buch)
                    break;
                case Window.State.Opened:
                    OnNoonWitchSpookEvent(true);//bububu
                    break;
                case Window.State.Latched:
                    isKnocking = true;
                    Invoke("TryToOpenWindow", 3f);//wait 3 seconds (buch! buch)
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
            OnNoonWitchSpookEvent(false);//bububu.. pls staph
            isKnocking = true;//breaks infinite update
            Invoke("TryToOpenWindow", 3f);//wait 3 seconds (buch! buch)
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
        if(lastWindow.GetComponent<Window>().windowState != Window.State.Latched)
        {   
            //window is closed then (player cannot open windows)
            lastWindow.GetComponent<Window>().ChangeStateTo(Window.State.Opened);
            RaiseOnNoonWitchSpookEvent(true);
        }
        else
        {
            RaiseOnNoonWitchSpookEvent(false);
            GetComponent<MovementBySimulatedInputHorizontal>().ResumeMoving();
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
