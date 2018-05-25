using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowColision : MonoBehaviour {

    [SerializeField]
    private float range;

    private GameObject lastWindow;

    
    private void WindowColision_OnUpdateNotifyAboutItselfEvent(GameObject window)
    {
        //am I in range of some new window
        if((window != lastWindow)
            &&(window.transform.position.x < transform.position.x + range && window.transform.position.x > transform.position.x - range))
        {
            lastWindow = window;

            GetComponent<MovementBySimulatedInputHorizontal>().moveState = MovementBySimulatedInputHorizontal.Move.Stay;

            KnockKnock();
        }
    }

    public void KnockKnock()
    {
        switch (lastWindow.GetComponent<Window>().windowState)
        {
            case Window.State.Closed:
                Invoke("TryToOpenWindow", 3f);//wait 3 seconds (buch! buch)

                break;
            case Window.State.Opened:
                Debug.Log("BuBUbu");
                break;
            case Window.State.Latched:
                Invoke("TryToOpenWindow", 3f);
                break;
        }
    }

    private void TryToOpenWindow()
    {
        if(lastWindow.GetComponent<Window>().windowState != Window.State.Latched)
        {
            lastWindow.GetComponent<Window>().ChangeStateTo(Window.State.Opened);
            Debug.Log("BuBUbu");
        }
        else
        {
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
