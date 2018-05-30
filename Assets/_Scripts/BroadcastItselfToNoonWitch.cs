using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastItselfToNoonWitch : MonoBehaviour {

    //Creating event 
    public delegate void OnUpdateNotifyAboutItself(GameObject sender);
    public event OnUpdateNotifyAboutItself OnUpdateNotifyAboutItselfEvent;

    // Use this for initialization
    void Start()
    {
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += BroadcastItself_OnUpdateEvent;
        //tell player to subscibe to this object
        GameObject.FindGameObjectWithTag("NoonWitch").GetComponent<WindowColision>().SubscribeToNewItem(this.gameObject);
    }

    private void BroadcastItself_OnUpdateEvent()
    {
        //raise event every update()
        if (OnUpdateNotifyAboutItselfEvent != null)
        {
            OnUpdateNotifyAboutItselfEvent(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("GameLogic") && GameObject.FindGameObjectWithTag("NoonWitch"))
        {
            //unsubscribe to Update
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= BroadcastItself_OnUpdateEvent;

        }
    }
}
