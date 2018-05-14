using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour {

    //Creating event
    public delegate void OnReachingFuelTier(float tier);
    public event OnReachingFuelTier OnReachingFuelTierEvent;

    [SerializeField]
    private float fuel;

    [SerializeField]
    private float burnSpeed;

    // Use this for initialization
    void Start () {
        fuel = 100;
        burnSpeed = 2;
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += Fireplace_OnUpdateEvent;
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToNewItem(gameObject);
    }

    //Update
    private void Fireplace_OnUpdateEvent()
    {
        //if fire is burning
        if(fuel > 0)
        {
            fuel -= burnSpeed * Time.deltaTime;
        }
        else { fuel = 0; }
        //if fire is at 10%
        if (fuel <= 10)
        {
            OnReachingFuelTierEvent(1);
        }
        if (fuel > 10 && fuel <= 50)
        {
            OnReachingFuelTierEvent(0.5f);
        }
        if (fuel > 50 && fuel < 100)
        {
            OnReachingFuelTierEvent(0);
        }
    }

    public void AddWood()
    {
        fuel += 30;
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("GameLogic") && GameObject.FindGameObjectWithTag("Player"))
        {
            //unsubscribe to Update
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= Fireplace_OnUpdateEvent;
        }
    }
}
