using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

    [SerializeField]
    public GameObject FrameClosed;

    [SerializeField]
    GameObject FrameOpened;

    public enum State { Closed, Opened, Latched }

    public State windowState;

    // Use this for initialization
    void Start () {
        ChangeStateTo(windowState);//just for testing (so that i can have scene with open windows but on start it sets to parameters value)

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

                player.GetComponent<PickItems>().equipedItem.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
                player.GetComponent<PickItems>().equipedItem.GetComponent<SpriteRenderer>().sortingOrder = 100;

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
                FrameClosed.SetActive(false);
                FrameOpened.SetActive(true);
                break;
            case State.Closed:
                windowState = State.Closed;
                FrameOpened.SetActive(false);
                FrameClosed.SetActive(true);
                break;
            case State.Latched:
                windowState = State.Latched;
                FrameOpened.SetActive(false);
                FrameClosed.SetActive(true);
                break;
        }
    }
}
