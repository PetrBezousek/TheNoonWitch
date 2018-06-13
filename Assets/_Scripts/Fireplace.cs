using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fireplace : MonoBehaviour {

    //Creating event
    public delegate void OnReachingFuelTier(float tier);
    public event OnReachingFuelTier OnReachingFuelTierEvent;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private GameObject woodSmall;
    [SerializeField]
    private GameObject woodBig;

    [SerializeField]
    private GameObject fuelStatus;

    private float fuelMax = 100;

    [SerializeField]
    private float fuelCurr;

    [SerializeField]
    private float burnSpeed = 5;

    // Use this for initialization
    void Start () {
        fuelCurr = fuelMax;
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += Fireplace_OnUpdateEvent;
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToFireplace(gameObject);
    }

    //Update
    private void Fireplace_OnUpdateEvent()
    {
        //if fire is burning
        if(fuelCurr > 0)
        {
            fuelCurr -= burnSpeed * Time.deltaTime;
        }
        else { fuelCurr = 0; }
        //0%
        if(fuelCurr <= 0)
        {
            OnReachingFuelTierEvent(0);
            fuelCurr = 0;
        }
        //if fire is at 10%
        if (fuelCurr <= 10)
        {
            OnReachingFuelTierEvent(10);
        }
        //50%
        if (fuelCurr > 10 && fuelCurr <= 50)
        {
            OnReachingFuelTierEvent(50);
           //...show notification
        }
        if (fuelCurr > 50 && fuelCurr < 100)
        {
            OnReachingFuelTierEvent(100);
        }
        

        fuelStatus.GetComponent<Text>().text = Mathf.Floor((fuelCurr / fuelMax)*100) + "%";
    }

    public void AddWood(InteractiveItem.Names name)
    {
        if(name == InteractiveItem.Names.WoodSmall)
        {
            GameObject newWood = Instantiate(woodSmall, spawnPoint.transform);
            newWood.transform.position = new Vector3(newWood.transform.position.x + 1, newWood.transform.position.y , 0);
            fuelCurr += 20;
            if (fuelCurr > 100) { fuelCurr = 100; }

        }
        if (name == InteractiveItem.Names.WoodBig)
        {
            GameObject newWood = Instantiate(woodBig, spawnPoint.transform);
            newWood.transform.position = new Vector3(newWood.transform.position.x - 1, newWood.transform.position.y, 0);
            fuelCurr += 50;
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
