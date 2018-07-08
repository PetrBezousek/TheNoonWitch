using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyItself : MonoBehaviour {

	public void destroySelf()
    {
        Destroy(gameObject);
    }

    public void destroySelfInTime(float inTime)
    {
        Invoke("destroySelf", inTime);
    }
}
