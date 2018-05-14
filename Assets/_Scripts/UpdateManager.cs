using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour {

    //Creating event FixedUpdate
    public delegate void OnFixedUpdate();
    public event OnFixedUpdate OnFixedUpdateEvent;

    //Creating event Update
    public delegate void OnUpdate();
    public event OnUpdate OnUpdateEvent;

    private void Update()
    {
        //raise event every frame    
        if (OnUpdateEvent != null)
        {
            OnUpdateEvent();
        }
    }

    void FixedUpdate () {
		//raise event every fixed frame (ussualy shorter then update() )
        if(OnFixedUpdateEvent != null)
        {
            OnFixedUpdateEvent();
        }
	}
}
