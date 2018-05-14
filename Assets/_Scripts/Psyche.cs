using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psyche : MonoBehaviour {

    [SerializeField]
    private float psyche;

    [SerializeField]
    private float multiplierFireplace = 0;

    [SerializeField]
    private float psycheLossSpeed = 1;

    // Use this for initialization
    void Start () {
        psyche = 1000;

        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += Psyche_OnUpdateEvent; ;
    }

    private void Psyche_OnUpdateEvent()
    {
        psyche -= psycheLossSpeed *(1 + multiplierFireplace) * Time.deltaTime;
    }

    //Start listening to item
    public void SubscribeToNewItem(GameObject item)
    {
        item.GetComponent<Fireplace>().OnReachingFuelTierEvent += Psyche_OnReachingFuelTierEvent;
    }

    private void Psyche_OnReachingFuelTierEvent(float tier)
    {
        if(tier != 0)
        {
            multiplierFireplace = tier;
        }
        else
        {
            multiplierFireplace = 0;
        }
    }
}
