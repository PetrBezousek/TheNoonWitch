using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

    public delegate void OnStateChange(GameObject sender);
    public event OnStateChange OnStateChangedEvent;

    public enum State { Closed, Opened, Latched }

    public State windowState;

    // Use this for initialization
    void Start () {
        ChangeStateTo(windowState);//just for testing

        //subscribe
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnWindowUsedEvent += Window_OnItemUsedEvent;
    }

    private void Window_OnItemUsedEvent(GameObject player)
    {
        if(gameObject == player.GetComponent<PickItems>().theClosestPlace)
        {
            //Latch the window
            if ((player.GetComponent<PickItems>().equipedItem != null) && (player.GetComponent<PickItems>().equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch))
            {
                GetComponent<Window>().ChangeStateTo(Window.State.Latched);
                
                //position
                player.GetComponent<PickItems>().equipedItem.transform.parent = transform;
                player.GetComponent<PickItems>().equipedItem.transform.localPosition = new Vector3(0, 0, 0);

                player.GetComponent<PickItems>().equipedItem.GetComponent<InteractiveItem>().isPickable = true;
                player.GetComponent<PickItems>().equipedItem = null;//'couse I used it just now 
                
            }//or just shut the window
            else if (GetComponent<Window>().windowState == Window.State.Opened)
            {
                GetComponent<Window>().ChangeStateTo(Window.State.Closed);
            }

        }

    }

    public void ChangeStateTo(State state)
    {
        switch (state)
        {
            case State.Opened:
                windowState = State.Opened;
                GetComponent<Renderer>().material.color = new Color(0.8f, 0.2f, 0.2f, 1f);
                break;
            case State.Closed:
                windowState = State.Closed;
                GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.8f, 1f);
                break;
            case State.Latched:
                windowState = State.Latched;
                GetComponent<Renderer>().material.color = new Color(0.2f, 0.8f, 0.2f, 1f);
                break;
        }
    }
}
