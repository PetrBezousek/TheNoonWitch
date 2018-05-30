using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour {

    public delegate void OnStartScreaming(GameObject sender);
    public event OnStartScreaming OnStartScreamingEvent;

    public bool isHavingToy = false;

	// Use this for initialization
	void Start () {

        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToChild(gameObject);
        InvokeRepeating("Scream", 5f, 5f);
	}
	

    public void Scream()
    {
            if(OnStartScreamingEvent != null)
            {
                OnStartScreamingEvent(gameObject);
            }
    }
}
