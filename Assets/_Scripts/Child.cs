using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour {

    public delegate void OnStartScreaming(int screamStreak);//0 = not screaming
    public event OnStartScreaming OnUpdateScreamingEvent;

    public bool isHavingToy = false;

    int numberOfSkips = 2;
    int currentSkipsLeft;

    [SerializeField]
    float grabRange = 1;

    [SerializeField]
    public GameObject[] toys;
    public int numberOfToysHaving = 0;//Ale mohl by začít s jednou např.

    [SerializeField]
    int forceX;
    [SerializeField]
    int forceY;

    int screamStreak = 0;


	// Use this for initialization
	void Start () {
        currentSkipsLeft = numberOfSkips;

        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToChild(gameObject);
        InvokeRepeating("ScreamGraduates", 5f, 5f);
	}
	
    private void ScreamGraduates()
    {
        screamStreak++;

        CheckScream();
    }

    //tohle se mi nelíbí!
    public void CheckScream()
    {

        if (numberOfToysHaving == toys.Length)
        {
            if(currentSkipsLeft > 0)
            {
                screamStreak = 0;

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

    private void Child_OnUpdateNotifyAboutItselfEvent(GameObject player)
    {
        if(Mathf.Abs(player.transform.position.x-transform.position.x) < grabRange)
        {
            Debug.Log("Am grabing you bich!");
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
