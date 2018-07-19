using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fireplace : MonoBehaviour {

    //Creating event
    public delegate void OnReachingFuelTier(float tier);
    public event OnReachingFuelTier OnReachingFuelTierEvent;

    public delegate void OnWoodAdded(bool smallWood);
    public event OnWoodAdded OnWoodAddedEvent;

    [SerializeField]
    private GameObject fuelStatus;

    [SerializeField]
    private GameObject fire;

    private DOTweenAnimation fireAnim;

    private float fuelMax = 100;

    [SerializeField]
    private GameObject pot;

    ParticleSystem.MainModule steamSettings;

    [Space]
    [Header("Current fuel")]
    [SerializeField]
    private float fuelCurr;

    private float fuelLastUpdate;

    [Space]
    [Header("Speed in fuel/second")]
    [SerializeField]
    private float burnSpeed = 5;


    // Use this for initialization
    void Start ()
    {
        steamSettings = GameObject.FindGameObjectWithTag("PotSteam").GetComponent<ParticleSystem>().main;

        fireAnim = fire.GetComponent<DOTweenAnimation>();

        fuelCurr = fuelMax;
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += Fireplace_OnUpdateEvent;
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToFireplace(gameObject);
    }

    //Update
    private void Fireplace_OnUpdateEvent()
    {
        float fuelLastUpdateTemp = fuelCurr;

        //if fire is burning
        if (fuelCurr > 0)
        {
            fuelCurr -= burnSpeed * Time.deltaTime;
        }
        else { fuelCurr = 0; }
        //0%
        if((fuelCurr <= 0) && !(fuelLastUpdate <= 0))
        {
            fireAnim.DOPause();

            fire.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0, 1);

            pot.GetComponent<DOTweenAnimation>().DORewind();
            pot.GetComponent<DOTweenAnimation>().DOPause();

            SoundManager.StopSound("PotCooking");

            steamSettings.startColor = new Color(0.533f, 0.486f, 0.686f, 0f);

            OnReachingFuelTierEvent(0);
            fuelCurr = 0;
        }
        //if fire is at 10%
        if ((fuelCurr <= 10)&&!(fuelLastUpdate <= 10))
        {
            fire.GetComponent<SpriteRenderer>().color = new Color(0.33f, 0.275f, 0,1);

            steamSettings.startColor = new Color(0.533f, 0.486f, 0.686f, 0.25f);

            pot.GetComponent<DOTweenAnimation>().DORewind();
            pot.GetComponent<DOTweenAnimation>().DOPause();
            
            SoundManager.pot.volume = SoundManager.pot.volume / 2;

            fireAnim.DOPause();
            fireAnim.DORestartById("10");
            fireAnim.DOPlayById("10");

            OnReachingFuelTierEvent(10);
        }
        //50%
        if ((fuelCurr > 10 && fuelCurr <= 50) && !(fuelLastUpdate > 10 && fuelLastUpdate <= 50))
        {
            fire.GetComponent<SpriteRenderer>().color = new Color(0.831f, 0.761f, 0.416f, 1);

            steamSettings.startColor = new Color(0.533f, 0.486f, 0.686f, 0.5f);

            pot.GetComponent<DOTweenAnimation>().DORestartById("Boiling");
            pot.GetComponent<DOTweenAnimation>().DOPlayById("Boiling");

            SoundManager.PlaySound("PotCooking");

            fireAnim.DOPause();
            fireAnim.DORestartById("50");
             fireAnim.DOPlayById("50");

            OnReachingFuelTierEvent(50);
           
        }
        if((fuelCurr > 50 && fuelCurr < 100) && !(fuelLastUpdate > 50 && fuelLastUpdate < 100))
        {
            fire.GetComponent<SpriteRenderer>().color = new Color(1, 0.941f, 0.667f, 1);

            steamSettings.startColor = new Color(0.533f, 0.486f, 0.686f, 1);

            pot.GetComponent<DOTweenAnimation>().DORestartById("Boiling");
            pot.GetComponent<DOTweenAnimation>().DOPlayById("Boiling");
            
            SoundManager.PlaySound("PotCooking");

            fireAnim.DOPause();
            fireAnim.DORestartById("100");
             fireAnim.DOPlayById("100");

            OnReachingFuelTierEvent(100);
        }

        fuelLastUpdate = fuelLastUpdateTemp;

        fuelStatus.GetComponent<Text>().text = Mathf.Floor((fuelCurr / fuelMax)*100).ToString();
    }

    public void AddWood(InteractiveItem.Names name)
    {
        

        if(name == InteractiveItem.Names.WoodSmall)
        {
            OnWoodAddedEvent(true);

            fuelCurr += 20;
            fuelLastUpdate = 101;//to force an update
            if (fuelCurr > 100) { fuelCurr = 100; }

        }
        if (name == InteractiveItem.Names.WoodBig)
        {
            OnWoodAddedEvent(false);
            
            fuelCurr += 50;
            fuelLastUpdate = 101;//to force an update
            if (fuelCurr > 100) { fuelCurr = 100; }

        }
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
