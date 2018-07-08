using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Psyche : MonoBehaviour {

    public delegate void OnPsycheChanged(float currentPsyche);
    public event OnPsycheChanged OnPsycheChangedEvent;

    [SerializeField]
    private GameObject UI;

    [Space]
    [Header("Maximum Psyche")]
    [SerializeField]
    public float psycheCurr = 100;
    
    private float fireplace = 0;
    
    private float noonWitch = 0;
    
    private float child = 0;
    [Space]
    [Header("psycheLoss/sec = (fireplace + noonWitch + child) * psycheLossSpeed")]

    [Space]
    [Header("Every X seconds child scream rises by 1")]
    [Header("child = screamStreak * childScreamMultiplier")]
    [SerializeField]
    private float childScreamMultiplier = 2;
    [Space]
    [Header("If fire is about to burn out")]
    [SerializeField]
    private float fireplace10Percent = 6;
    
    [Header("If fire is burnt out")]
    [SerializeField]
    private float fireplace0Percent = 15;

    [Space]
    [Header("If noon witch is looking through window")]
    [SerializeField]
    private float noonwitchSpooking = 5;
    [Space]
    [Header("Multiplier to other psyche loss")]
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
            if((psycheCurr - psycheLoss) < 0)
            {
                psycheCurr = 0;

                CancelInvoke("SubtractPsyche");

                //zapni fázi
                GetComponent<GamePhases>().StartPhase(GamePhases.Phase.EndGame_8_DONT_USE);

            }
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
    //Stop listening to item
    public void UnSubscribeToNoonWitch(GameObject noonWitch)
    {
        noonWitch.GetComponent<WindowColision>().OnNoonWitchSpookEvent -= Psyche_OnNoonWitchSpookEvent;
    }

    //Start listening to item
    public void SubscribeToChild(GameObject child)
    {
        child.GetComponent<Child>().OnUpdateScreamingEvent += Psyche_OnUpdateScreamingEvent;
    }

    private void Psyche_OnUpdateScreamingEvent(int screamStreak)
    {
        

        if(screamStreak == 0)
        {
            UI.GetComponent<UIManager>().ScreamStreakCount.GetComponent<Text>().text = "";//clear text
            UI.GetComponent<UIManager>().Child.GetComponent<Image>().color = new Color(1f, 0.859f, 0.667f, 0.3f);
        }
        else
        {
            UI.GetComponent<UIManager>().ScreamStreakCount.GetComponent<Text>().text = screamStreak + "x";
             UIAnimChange(UI.GetComponent<UIManager>().Child.GetComponent<DOTweenAnimation>());

            if (screamStreak > 0 && screamStreak <= 2)
            {
                UI.GetComponent<UIManager>().Child.GetComponent<Image>().color = new Color(0.667f, 0.475f, 0.224f, 1f);
            }
            else
            if (screamStreak > 2)
            {
                UI.GetComponent<UIManager>().Child.GetComponent<Image>().color = new Color(0.333f, 0.192f, 0f, 1f);
            }
        }

        child = screamStreak * childScreamMultiplier;
    }

    private void Psyche_OnNoonWitchSpookEvent(bool isSpooking)
    {
        if (isSpooking)
        {
            UI.GetComponent<UIManager>().NoonWitch.GetComponent<Image>().color = new Color(0.333f, 0.275f, 0f, 1f);
            UIAnimChange(UI.GetComponent<UIManager>().NoonWitch.GetComponent<DOTweenAnimation>());
            noonWitch = noonwitchSpooking;
        }
        else
        {
            UI.GetComponent<UIManager>().NoonWitch.GetComponent<Image>().color = new Color(1f, 0.941f, 0.667f, 0.3f);
            noonWitch = 0;
        }
    }

    private void Psyche_OnReachingFuelTierEvent(float tier)
    {
        if(tier == 100)
        {
            //first warning UI
            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().color = new Color(0.533f,0.486f, 0.686f,0.3f);

            fireplace = 0;
        }
        if (tier == 50)
        {
            //first warning UI (but player is not loosing psyche yet..)
            //UI.GetComponent<UIManager>().UIFire.GetComponent<Image>().color = new Color(0.533f, 0.486f, 0.686f, 0.8f);
        }
        if (tier == 10)
        {
            //second warning UI
            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().color = new Color(0.251f, 0.188f, 0.459f,1);
            UIAnimChange(UI.GetComponent<UIManager>().Fire.GetComponent<DOTweenAnimation>());

            fireplace = fireplace10Percent;
        }
        if (tier == 0)
        {
            //third warning UI
            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().color = new Color(0.075f, 0.027f, 0.227f, 1);
            UIAnimChange(UI.GetComponent<UIManager>().Fire.GetComponent<DOTweenAnimation>());

            fireplace = fireplace0Percent;
        }
        
    }

    private void UIAnimChange(DOTweenAnimation tweenComponent)
    {
        tweenComponent.DORestartById("Shake");
        tweenComponent.DOPlayById("Shake");
    }
}
