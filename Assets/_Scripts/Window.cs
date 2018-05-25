using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

    public enum State { Closed, Opened, Latched }

    public State windowState;

    // Use this for initialization
    void Start () {
        ChangeStateTo(windowState);
	}
	
    public void ChangeStateTo(State state)
    {
        switch (state)
        {
            case State.Closed:
                windowState = State.Closed;
                GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 0.6f);
                break;
            case State.Opened:
                windowState = State.Opened;
                GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 0.2f);
                break;
            case State.Latched:
                windowState = State.Latched;
                GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1f);
                break;
        }
    }
}
