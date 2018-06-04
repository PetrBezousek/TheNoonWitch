using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour {

    public delegate void OnStartScreaming(int screamStreak);//0 = not screaming
    public event OnStartScreaming OnUpdateScreamingEvent;

    public bool isHavingToy = false;

    [SerializeField]
    int numberOfSkips = 2;
    int currentSkipsLeft;

    [SerializeField]
    float grabRange = 1;
    [SerializeField]
    float grabChance = 25;

    [SerializeField]
    public GameObject[] toys;
    public int numberOfToysHaving = 0;//Ale mohl by začít s jednou např.

    [SerializeField]
    int forceX;
    [SerializeField]
    int forceY;

    int screamStreak = 0;

    bool isInGrabingMood =false;
    bool isInRange = false;


    // Use this for initialization
    void Start () {
        currentSkipsLeft = numberOfSkips;

        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToChild(gameObject);
        InvokeRepeating("ScreamGraduates", 5f, 5f);
	}
	
    private void ScreamGraduates()
    {
        screamStreak++;
        if(screamStreak > -2)//možná udělat strop streaku, např. 4 s tím že od 3 už bude i chytat
        {
            isInGrabingMood = true;
        }
        CheckScream();
    }
    
    public void CheckScream()
    {

        if (numberOfToysHaving == toys.Length)
        {
            if(currentSkipsLeft > 0)
            {
                screamStreak = 0;
                isInGrabingMood = false;

                currentSkipsLeft--;
            }
            else
            {
                screamStreak = 1;

                currentSkipsLeft = numberOfSkips;
                
                numberOfToysHaving = 0;

                foreach(GameObject toy in toys)
                {
                    toy.GetComponent<InteractiveItem>().isPickable = true;//ano, hráč může chytit hračku v letu
                    toy.GetComponent<InteractiveItem>().SetOwner(null);
                    toy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;//set back to static when player picks up toy
                    toy.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
                    
                }

            }
        }
        if (OnUpdateScreamingEvent != null)
        {
            OnUpdateScreamingEvent(screamStreak);
        }

    }
 
    //Player broadcasts itself to child
    private void Child_OnUpdateNotifyAboutItselfEvent(GameObject player)
    {
        if(isInGrabingMood 
            && !isInRange
            && Mathf.Abs(player.transform.position.x-transform.position.x) < grabRange)
        {
            isInRange = true;//so child cant grab player infinitely
            float rng = Random.value;
            Debug.Log(rng + "  chance: " + grabChance/100);
            if(rng < grabChance / 100)
                {
                player.GetComponent<MovementByUserInputHorizontal>().enabled = false;//cant move now
                player.GetComponent<PickItems>().enabled = false;//cant pick items

                player.GetComponent<RootMinigame>().enabled = true;//start minigame

            }
            
        }

        //if out of range
        if(Mathf.Abs(player.transform.position.x - transform.position.x) > grabRange)
        {
            isInRange = false;//child can grab player now again
        }
    }

    public void SubscribeToPlayersBroadcast(GameObject player)
    {
        player.GetComponent<BroadcastItselfToChild>().OnUpdateNotifyAboutItselfEvent += Child_OnUpdateNotifyAboutItselfEvent;
    }

    public void UnsubscribeFromPlayersBroadcast(GameObject player)
    {
        player.GetComponent<BroadcastItselfToChild>().OnUpdateNotifyAboutItselfEvent -= Child_OnUpdateNotifyAboutItselfEvent;
    }
}
