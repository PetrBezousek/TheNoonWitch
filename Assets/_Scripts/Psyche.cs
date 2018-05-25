using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Psyche : MonoBehaviour {

    public delegate void OnPsycheChanged(float currentPsyche);
    public event OnPsycheChanged OnPsycheChangedEvent;

    [SerializeField]
    private GameObject psycheStatus;
    
    [SerializeField]
    private float psycheCurr = 1000;

    [SerializeField]
    private float fireplace = 0;

    [SerializeField]
    private float psycheLossSpeed = 1;

    // Use this for initialization
    void Start () {
        
        InvokeRepeating("SubtractPsyche", 1f, 1f);
    }

    private void SubtractPsyche()
    {
        if(psycheCurr != psycheCurr - fireplace)
        {
            if((psycheCurr - fireplace) < 0) { psycheCurr = 0; }
            else
            {
                psycheCurr -= fireplace;
            }
            //event for status bars
            OnPsycheChangedEvent(psycheCurr);
        }
        
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
            fireplace = tier;
        }
        else
        {
            fireplace = 0;
        }
    }
}
