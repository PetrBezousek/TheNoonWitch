using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Psyche : MonoBehaviour {

    public delegate void OnPsycheChanged(float currentPsyche);
    public event OnPsycheChanged OnPsycheChangedEvent;

    [SerializeField]
    private Animator mindAnimator;

    [SerializeField]
    [Range(0, 2)]
    float speedMindChange;

    [SerializeField]
    private GameObject UI;

    [Space]
    [Header("Maximum Psyche")]
    [SerializeField]
    public float psycheCurr = 100;

    public float psycheMax;

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

    [Space]
    [Header("VFX when something subtracts psyche")]
    [SerializeField]
    GameObject psychePSystem;
    [SerializeField]
    Transform psychePSystemFire;
    [SerializeField]
    Transform psychePSystemChild;
    [SerializeField]
    Transform psychePSystemNoonWitch;
    [SerializeField]
    Transform mind;

    // Use this for initialization
    void Start () {
        psycheMax = psycheCurr;
        InvokeRepeating("SubtractPsyche", 1f, 1f);
    }

    [SerializeField]
    private Color colorFire;
    [SerializeField]
    private Color colorChild;
    [SerializeField]
    private Color colorWitch;

    [SerializeField]
    private int fireplaceCounterMax = 3;
    private int fireplaceCounter = 0;
    [SerializeField]
    private int noonwitchCounterMax = 3;
    private int noonwitchCounter = 0;
    /*
    [SerializeField]
    private int childCounterMax = 2;
    private int childCounter = 0;*/

    private int currState = 100;
    private int lastState = 100;



    private void SubtractPsyche()
    {
        if(!(GetComponent<GamePhases>().currentPhase == GamePhases.Phase.Start_1 ||
            GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGame_8 ||
            GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGameWin_9 ||
            GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGameLoose))
        {
            float paramFireplace = fireplace;
            float paramChild = child;
            float paramNoonwitch = noonWitch;

            var psEmission = psychePSystem.GetComponent<ParticleSystem>().emission;
            var psRadius = psychePSystem.GetComponent<ParticleSystem>().shape;
            var psColor = psychePSystem.GetComponent<ParticleSystem>().main;
            if (fireplace > 0)
            {
                fireplaceCounter++;
                // zvysim davku a zvetsim particle effect
                if(fireplaceCounter == fireplaceCounterMax)
                {
                    fireplaceCounter = 0;
                    paramFireplace *= 4;
                }
                psEmission.rateOverTime = (paramFireplace / 3);
                //psRadius.radius = 0.01f * paramFireplace * 2;
                psColor.startColor = colorFire;
                GameObject effect = Instantiate(psychePSystem, psychePSystemFire);
                effect.transform.DOMove(mind.position, 2).OnComplete(() => { effect.GetComponent<destroyItself>().destroySelf(); });
            }
            if (noonWitch > 0)
            {
                noonwitchCounter++;
                // zvysim davku a zvetsim particle effect
                if (noonwitchCounter == noonwitchCounterMax)
                {
                    noonwitchCounter = 0;
                    paramNoonwitch *= 8;
                }
                psEmission.rateOverTime = (paramNoonwitch / 3);
                //psRadius.radius = 0.01f * paramNoonwitch * 2;
                psColor.startColor = colorWitch;
                GameObject effect = Instantiate(psychePSystem, psychePSystemNoonWitch);
                effect.transform.DOMove(mind.position, 2).OnComplete(() => { effect.GetComponent<destroyItself>().destroySelf(); });
            }
            if (child > 0)
            {
                psEmission.rateOverTime = paramChild / 3;
                //psRadius.radius = 0.01f * paramChild * 2;
                psColor.startColor = colorChild;
                GameObject effect = Instantiate(psychePSystem, psychePSystemChild);
                effect.transform.DOMove(mind.position, 2).OnComplete(() => { effect.GetComponent<destroyItself>().destroySelf(); });
            }

            float psycheLoss = (paramFireplace + paramNoonwitch + paramChild) * psycheLossSpeed;

            if (psycheCurr != psycheCurr - psycheLoss)
            {
                if ((psycheCurr - psycheLoss) < 0)
                {
                    psycheCurr = 0;

                    CancelInvoke("SubtractPsyche");
                    Debug.Log("Psyche < 0");
                    //zapni fázi
                    GetComponent<GamePhases>().StartPhase(GamePhases.Phase.EndGame_8);

                }
                else
                {
                    psycheCurr -= psycheLoss;
                }

                UI.GetComponent<UIManager>().PsycheCounter.GetComponent<Text>().text = (Mathf.Round((psycheCurr / psycheMax) * 100)).ToString() + "%";

                //event for status bars
                //OnPsycheChangedEvent(psycheCurr);

                if (((psycheCurr / psycheMax) * 100) >= 50)
                {
                    currState = 100;
                }
                else if (((psycheCurr / psycheMax) * 100) >= 30)
                {
                    currState = 50;
                }
                else if (((psycheCurr / psycheMax) * 100) >= 15)
                {
                    currState = 30;
                }
                else if (((psycheCurr / psycheMax) * 100) >= 0)
                {
                    currState = 15;
                }
                else
                {
                    currState = 0;
                }

                if(currState != lastState)
                {
                    mind.gameObject.GetComponent<setColor>().updateColor();
                    //mindAnimator.Play("mindChange", -1, 0f);
                    //mindAnimator.speed = speedMindChange;
                }

                lastState = currState;

            }
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
            UI.GetComponent<UIManager>().Child.GetComponent<Image>().DOColor(new Color(1f, 0.859f, 0.667f, 0f), 0.5f)
                .OnComplete(() => { UI.GetComponent<UIManager>().Child.SetActive(false); });
        }
        else
        {
            UI.GetComponent<UIManager>().ScreamStreakCount.GetComponent<Text>().text = screamStreak + "x";
            UIAnimChange(UI.GetComponent<UIManager>().Child.GetComponent<DOTweenAnimation>());

            if (screamStreak > 0 && screamStreak <= 2)
            {

                UI.GetComponent<UIManager>().Child.SetActive(true);
                UI.GetComponent<UIManager>().Child.GetComponent<Image>().DOColor(new Color(0.667f, 0.475f, 0.224f, 0.8f),0.2f);
            }
            else
            if (screamStreak > 2)
            {
                UI.GetComponent<UIManager>().Child.SetActive(true);
                UI.GetComponent<UIManager>().Child.GetComponent<Image>().DOColor(new Color(0.333f, 0.192f, 0f, 1f),0.2f);
            }
        }

        child = screamStreak * childScreamMultiplier;
    }

    private void Psyche_OnNoonWitchSpookEvent(bool isSpooking)
    {
        if (isSpooking)
        {
            UI.GetComponent<UIManager>().NoonWitch.SetActive(true);
            UI.GetComponent<UIManager>().NoonWitch.GetComponent<Image>().DOColor(new Color(0.333f, 0.275f, 0f, 1f),0.2f);
            UIAnimChange(UI.GetComponent<UIManager>().NoonWitch.GetComponent<DOTweenAnimation>());
            noonWitch = noonwitchSpooking;
        }
        else
        {
            UI.GetComponent<UIManager>().NoonWitch.GetComponent<Image>().DOColor(new Color(1f, 0.941f, 0.667f, 0f),0.5f)
                .OnComplete(() => { UI.GetComponent<UIManager>().NoonWitch.SetActive(false); });
            noonWitch = 0;
        }
    }

    private void Psyche_OnReachingFuelTierEvent(float tier)
    {
        if(tier == 100)
        {
            //first warning UI
            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().DOColor(new Color(0.533f,0.486f, 0.686f,0f),0.5f)
                .OnComplete(() => { UI.GetComponent<UIManager>().Fire.SetActive(false); });

            fireplace = 0;
        }
        if (tier == 50)
        {

            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().DOColor(new Color(0.533f, 0.486f, 0.686f, 0f), 0.5f)
                .OnComplete(() => { UI.GetComponent<UIManager>().Fire.SetActive(false); });
            fireplace = 0;

            //first warning UI (but player is not loosing psyche yet..)
            //UI.GetComponent<UIManager>().UIFire.GetComponent<Image>().color = new Color(0.533f, 0.486f, 0.686f, 0.8f);
        }
        if (tier == 10)
        {
            //second warning UI
            UI.GetComponent<UIManager>().Fire.SetActive(true);
            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().DOColor(new Color(0.251f, 0.188f, 0.459f,0f),0.2f);
            UIAnimChange(UI.GetComponent<UIManager>().Fire.GetComponent<DOTweenAnimation>());

            fireplace = fireplace10Percent;
        }
        if (tier == 0)
        {
            //third warning UI
            UI.GetComponent<UIManager>().Fire.SetActive(true);
            UI.GetComponent<UIManager>().Fire.GetComponent<Image>().DOColor(new Color(0.075f, 0.027f, 0.227f, 1),0.2f);
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
