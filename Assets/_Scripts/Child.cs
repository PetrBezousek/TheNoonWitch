using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour {

    public delegate void OnStartScreaming(int screamStreak);//0 = not screaming
    public event OnStartScreaming OnUpdateScreamingEvent;


    public bool isHavingToy = false;

    [Space]
    [Header("How often childs scream updates in seconds (with each update, scream psyche loss graduates) (it is set onStart)")]
    [SerializeField]
    public float screamGraduatesIn = 5;

    [Space]
    [Header("How many updates skip after getting all 3 toys (before throwing them away)")]
    [SerializeField]
    int numberOfSkips = 2;


    int currentSkipsLeft = 99999;

    [Space]
    [Header("Grab item range in pixels (Game is 18 pixels long)")]
    [SerializeField]
    float grabRange = 1;
    [Space]
    [Header("Chance to root player if he comes by (in %)")]
    [SerializeField]
    float grabChancePercent = 50;

    float grabChance = 0;

    [SerializeField]
    public GameObject[] toys;
    public int numberOfToysHaving = 0;//Ale mohl by začít s jednou např.

    [Space]
    [Header("Force with which child throws toys (X horizontal, Y vertical)")]
    [SerializeField]
    int forceX;
    [SerializeField]
    int forceY;

    [SerializeField]
    AnimationSettingsChild anim;

    int screamStreak = 0;

    bool isInGrabingMood =false;
    bool isInRange = false;
    RootMinigame minigame;

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (GameObject toy in toys)
            {
                toy.GetComponent<InteractiveItem>().isPickable = true;//ano, hráč může chytit hračku v letu
                toy.GetComponent<InteractiveItem>().SetOwner(null);
                toy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;//set back to static when player picks up toy
                toy.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));

            }
        }*/

    }

    // Use this for initialization
    void Start () {

        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().SubscribeToChild(gameObject);
        
	}

    SoundManager sound;
    // Use this for initialization
    void Awake()
    {
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    public void SetNumberOfSkips(int num)
    {
        numberOfSkips = num;
    }
    public void SetNumberOfSkips()
    {
        currentSkipsLeft = 0;
    }

    public void SetGrabChance()
    {
        grabChance = grabChancePercent;
    }

    public void SetGrabChance(float percentage)
    {
        grabChance = percentage;
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
        else
        {
            sound.PlaySound("childScream");
            anim.StartChildScream();
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
          //  Debug.Log(rng + "  chance: " + grabChance/100);
            if(rng < grabChance / 100)
            {
                if(grabChance == 100) { SetGrabChance(); }

                player.GetComponent<MovementByUserInputHorizontal>().Stop();//player movement is stopped, not paused
                player.GetComponent<MovementByUserInputHorizontal>().enabled = false;//cant move now
                player.GetComponent<PickItems>().enabled = false;//cant pick items
                minigame = player.GetComponent<RootMinigame>();//save reference for invoke

                minigame.MinigameStartsSoon();

                anim.StartChildGrab();
                GameObject.FindGameObjectWithTag("PlayerBody").GetComponent<AnimatorSettings>().StartStay();

                Invoke("StartMinigame", 0.75f);

            }
            
        }

        //if out of range
        if(Mathf.Abs(player.transform.position.x - transform.position.x) > grabRange)
        {
            isInRange = false;//child can grab player now again
        }
    }

    private void StartMinigame()
    {
        minigame.enabled = true;//start minigame
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
