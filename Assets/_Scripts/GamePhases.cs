using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhases : MonoBehaviour {

    public enum Phase { Start_1_DONT_USE,Fire_2,ChildScream_3,Latch_4,ChildRoot_5,CompleteHard_6_NO_CHANGE_YET,CompleteImpossible_7_NO_CHANGE_YET, EndGame_8_DONT_USE, EndGameWin_9_DONT_USE }

    [Header("Game starts in selected phase")]
    [Header("(do NOT activate components manually, only through this)")]
    [SerializeField] Phase startingPhase;


    [Space]
    [Header("Fire_2")]
    [SerializeField] InteractiveItem woodSmall;
    [SerializeField] InteractiveItem woodBig;

    [Space]
    [Header("ChildScream_3")]
    [SerializeField] Child child;
    [SerializeField] InteractiveItem husar;
    [SerializeField] InteractiveItem kocar;
    [SerializeField] InteractiveItem kohout;

    [Space]
    [Header("Latch_4")]
    [SerializeField] MovementBySimulatedInputHorizontal noonWitchMovement;
    [SerializeField] InteractiveItem latch;

    [Space]
    [Header("EndGame_8")]

    [SerializeField]
    GameObject noonWitchObject;
    [SerializeField] GameObject noonWitchBody;

    [Space]
    [Space]
    [SerializeField] UIManager UI;

    public Phase currentPhase;

    // Use this for initialization
    void Awake ()
    {
        StartPhase(startingPhase);
	}
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>().OnWoodAddedEvent += GamePhases_OnWoodAddedEvent;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnChildGotAllToysEvent += GamePhases_OnChildGotAllToysEvent;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnLatchWindowEvent += GamePhases_OnLatchWindowEvent;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RootMinigame>().OnChildRootEvent += GamePhases_OnChildRootEvent;
    }

    private void GamePhases_OnLatchWindowEvent()
    {
        StartPhase(Phase.ChildRoot_5);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnLatchWindowEvent -= GamePhases_OnLatchWindowEvent;
    }

    private void GamePhases_OnChildRootEvent()
    {
        StartPhase(Phase.CompleteHard_6_NO_CHANGE_YET);
        GameObject.FindGameObjectWithTag("Player").GetComponent<RootMinigame>().OnChildRootEvent -= GamePhases_OnChildRootEvent;
    }

    private void GamePhases_OnChildGotAllToysEvent()
    {
        StartPhase(Phase.Latch_4);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnChildGotAllToysEvent -= GamePhases_OnChildGotAllToysEvent;
    }

    private void GamePhases_OnWoodAddedEvent(bool smallWood)
    {
        StartPhase(Phase.ChildScream_3);
        GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>().OnWoodAddedEvent -= GamePhases_OnWoodAddedEvent;
    }


    //note: it could be no bueno if you dont start from begining -> going to further phases yould be broken
    public void StartPhase(Phase p)
    {
        switch (p)
        {
            case Phase.Fire_2:
                //new current phase
                currentPhase = Phase.Fire_2;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Add wood to fireplace";
                //enable some stuff
                woodSmall.isPickable = true;
                woodBig.isPickable = true;
                break;
            case Phase.ChildScream_3:
                if(startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.Fire_2);
                }
                //new current phase
                currentPhase = Phase.ChildScream_3;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Give child toys";
                //enable some stuff
                child.InvokeRepeating("ScreamGraduates", 0, child.screamGraduatesIn);
                kohout.isPickable = true;
                kocar.isPickable = true;
                husar.isPickable = true;
                break;
            case Phase.Latch_4:
                if (startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.ChildScream_3);
                }
                //new current phase
                currentPhase = Phase.Latch_4;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Latch the window";//"close" and after that tell playr to use latch
                //enable some stuff
                noonWitchMovement.enabled = true;
                latch.isPickable = true;
                break;
            case Phase.ChildRoot_5:
                if (startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.Latch_4);
                }
                //new current phase
                currentPhase = Phase.ChildRoot_5;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Get rooted by child";
                //enable some stuff
                child.SetGrabChance();
                child.SetNumberOfSkips();
                break;
            case Phase.CompleteHard_6_NO_CHANGE_YET:
                if (startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.ChildRoot_5);
                }
                //new current phase
                currentPhase = Phase.CompleteHard_6_NO_CHANGE_YET;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Hard mode begins";
                //enable
                GetComponent<UITime>().startClockHard = true;

                break;
            case Phase.CompleteImpossible_7_NO_CHANGE_YET:
                if (startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.CompleteHard_6_NO_CHANGE_YET);
                }
                //new current phase
                currentPhase = Phase.CompleteImpossible_7_NO_CHANGE_YET;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Impossible mode begins";
                //enable
                GetComponent<UITime>().startClockImpossible = true;

                break;
            case Phase.EndGame_8_DONT_USE:
                if (startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.CompleteImpossible_7_NO_CHANGE_YET);
                }
                //new current phase
                currentPhase = Phase.EndGame_8_DONT_USE;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Protect child";
                //enable stuff
                //dítě grab 100%
                GameObject.FindGameObjectWithTag("Child").GetComponent<Child>().SetGrabChance(100);
                //enable simulated move
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovementBySimulatedInputHorizontal>().enabled = true;
                //.. and some other stuff
                GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().UnSubscribeToNoonWitch(noonWitchObject);

                foreach(GameObject go in GameObject.FindGameObjectsWithTag("Window"))
                {
                    noonWitchObject.GetComponent<WindowColision>().UnSubscribeToNewItem(go);
                }

                noonWitchObject.GetComponent<WindowColision>().noonWitchWalking.SetActive(true);
                noonWitchObject.GetComponent<WindowColision>().noonWitchSpooking.SetActive(false);


                noonWitchMovement.enabled = true;
                noonWitchMovement.moveState = MovementBySimulatedInputHorizontal.Move.Left;
                noonWitchMovement.speed = 0.3f;

                noonWitchObject.transform.position = GameObject.FindGameObjectWithTag("PointDoor").transform.position;

                noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().sortingLayerName = "Player";//berle
                noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().sortingLayerName = "Player";//body

                //disable other minigames
                GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().enabled = false;
                UI.ArrowDown.GetComponent<FixedPosition>().Hide();
                child.CancelInvoke("ScreamGraduates");
                woodSmall.isPickable = false;
                woodBig.isPickable = false;
                kohout.isPickable = false;
                kocar.isPickable = false;
                husar.isPickable = false;
                latch.isPickable = false;
                child.SetNumberOfSkips(9999999);
                break;
            case Phase.EndGameWin_9_DONT_USE:
                if (startingPhase != Phase.Fire_2)
                {
                    StartPhase(Phase.EndGame_8_DONT_USE);
                }
                //new current phase
                currentPhase = Phase.EndGameWin_9_DONT_USE;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().text = "Father is back!";

                break;
        }
    }
}
