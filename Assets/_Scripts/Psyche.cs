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
    public float psycheCurr = 100;
    
    private float fireplace = 0;
    
    private float noonWitch = 0;
    
    private float child = 0;

    [SerializeField]
    private float psycheLossSpeed = 1;

    // Use this for initialization
    void Start () {
        
        InvokeRepeating("SubtractPsyche", 1f, 1f);
    }

    private void SubtractPsyche()
    {
        float psycheLoss = (fireplace + noonWitch + child) * psycheLossSpeed;
        if(psycheCurr != psycheCurr - psycheLoss)
        {
            if((psycheCurr - psycheLoss) < 0) { psycheCurr = 0; }
            else
            {
                psycheCurr -= psycheLoss;
            }
            //event for status bars
            OnPsycheChangedEvent(psycheCurr);
        }
        
    }

    //Start listening to item ... (if I wanted to make more then one fireplace at runtime, I need this method)
    public void SubscribeToFireplace(GameObject fireplace)
    {
        fireplace.GetComponent<Fireplace>().OnReachingFuelTierEvent += Psyche_OnReachingFuelTierEvent;
    }

    //Start listening to item
    public void SubscribeToNoonWitch(GameObject noonWitch)
    {
        noonWitch.GetComponent<WindowColision>().OnNoonWitchSpookEvent += Psyche_OnNoonWitchSpookEvent;
    }

    //Start listening to item
    public void SubscribeToChild(GameObject child)
    {
        child.GetComponent<Child>().OnUpdateScreamingEvent += Psyche_OnUpdateScreamingEvent;
    }

    private void Psyche_OnUpdateScreamingEvent(int screamStreak)
    {
        child = screamStreak * 2;
    }

    private void Psyche_OnNoonWitchSpookEvent(bool isSpooking)
    {
        if (isSpooking)
        {
            noonWitch = 5;
        }
        else
        {
            noonWitch = 0;
        }
    }

    private void Psyche_OnReachingFuelTierEvent(float tier)
    {
        if(tier < 50)
        {
            if(tier == 10)
            {
                fireplace = 2;
            }
            if (tier == 0)
            {
                fireplace = 5;
            }
        }
        else
        {
            fireplace = 0;
        }
       
    }
}
