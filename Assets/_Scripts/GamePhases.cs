using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GamePhases : MonoBehaviour {

    [SerializeField] public GameObject btnReset;
    public bool skipCinematics = false;

    public enum Phase { Start_1, Fire_2, ChildScream_3, Latch_4, ChildRoot_5,
        CompleteHard_6_NO_CHANGE_YET, CompleteImpossible_7_NO_CHANGE_YET, EndGame_8, EndGameWin_9, EndGameLoose }

    [SerializeField]
    Txt.Language lang;

    [Header("Game starts in selected phase")]
    [Header("(do NOT activate components manually, only through this)")]
    [SerializeField] Phase startingPhase;


    [Space]
    [Header("Fire_2")]
    [SerializeField] GameObject goWoodSmall;
    [SerializeField] GameObject goWoodBig;
    [SerializeField] InteractiveItem woodSmall;
    [SerializeField] InteractiveItem woodBig;
    [SerializeField] MovementByUserInputHorizontal playerMovement;
    [SerializeField] GameObject tutorArrowLeft;
    [SerializeField] GameObject tutorArrowRight;
    [SerializeField] Fireplace fireplace;

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
    [SerializeField] GameObject psycheStatus;


    [Space]
    [Header("EndGame_8")]
    [SerializeField]
    AnimatorSettings playerAnim;
    [SerializeField] PickItems playerPickItem;

    [SerializeField]
    GameObject noonWitchObject;
    [SerializeField]
    RootMinigame playerRootMinigame;
    [SerializeField] GameObject noonWitchBody;

    [Space]
    [Space]
    [SerializeField] UIManager UI;

    [SerializeField] GameObject playerWalkCircle;


    public bool itsTimeForHighligtingLatch = false;

    public Phase currentPhase;

    SoundManager sound;

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    // Use this for initialization
    void Awake()
    {
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        Txt.language = lang;
        Txt.updateTextLanguage();
        StartPhase(startingPhase);
    }

    public void openZapsplat()
    {
        //onmouse button up open website
        openWindow("https://www.zapsplat.com");
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>().OnWoodAddedEvent += GamePhases_OnWoodAddedEvent;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnChildGotAllToysEvent += GamePhases_OnChildGotAllToysEvent;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnLatchWindowEvent += GamePhases_OnLatchWindowEvent;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RootMinigame>().OnChildRootEvent += GamePhases_OnChildRootEvent;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            switch (currentPhase)
            {
                case Phase.Start_1:
                    UI.hideStartUpStuff();
                    StartPhase(Phase.Fire_2);
                    break;
                case Phase.EndGameLoose:
                case Phase.EndGameWin_9:
                    restart();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            switch (currentPhase)
            {
                case Phase.Start_1:
                    Txt.language = Txt.Language.CZ;
                    Txt.updateTextLanguage();
                    UI.StartText.GetComponent<Text>().text = Txt.start;
                    UI.madeBy.GetComponent<Text>().text = Txt.madeBy;
                    break;
                case Phase.EndGameLoose:
                case Phase.EndGameWin_9:
                    /*
                    Txt.language = Txt.Language.CZ;
                    Txt.updateTextLanguage();
                    UI.StartText.GetComponent<Text>().text = Txt.restart;
                    */
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            switch (currentPhase)
            {
                case Phase.Start_1:
                    Txt.language = Txt.Language.ENG;
                    Txt.updateTextLanguage();
                    UI.StartText.GetComponent<Text>().text = Txt.start;
                    UI.madeBy.GetComponent<Text>().text = Txt.madeBy;
                    break;
                case Phase.EndGameLoose:
                case Phase.EndGameWin_9:
                    /*
                    Txt.language = Txt.Language.ENG;
                    Txt.updateTextLanguage();
                    UI.StartText.GetComponent<Text>().text = Txt.restart;
                    */
                    break;
            }
        }
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [SerializeField]
    GameObject pSystemHighlight;
    [SerializeField]
    GameObject testItem;

    public void HighlightItem(GameObject item)
    {
        GameObject resized = pSystemHighlight;
        var emi = resized.GetComponent<ParticleSystem>().shape;
        emi.radius = item.GetComponent<SpriteRenderer>().bounds.size.x / 3f;


        GameObject highlight = Instantiate(resized, item.transform, false);
        Destroy(highlight, 1.8f);
        item.GetComponent<SpriteRenderer>().DOColor(new Color(1, 85.9f, 66.7f, 0.88f), 0.6f).SetEase(Ease.InBounce).SetLoops(3, LoopType.Yoyo);
    }

    private void GamePhases_OnLatchWindowEvent()
    {
        Invoke("StartPhaseChildRoot", 2f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().OnLatchWindowEvent -= GamePhases_OnLatchWindowEvent;
    }

    public void StartPhaseChildRoot()
    {
        StartPhase(Phase.ChildRoot_5);
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

    private void showTutorArrows()
    {
        if (playerMovement.isNotMoving)
        {
            tutorArrowLeft.GetComponent<FixedPosition>().Show();
            tutorArrowRight.GetComponent<FixedPosition>().Show();
        }
    }

    private void highlightWood()
    {
        if(!(
            GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().equipedItem != null &&
                (
                GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.WoodBig ||
                GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.WoodSmall
                )
            )
            &&
            (GetComponent<GamePhases>().currentPhase == Phase.Fire_2)
            )
        {
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.pridejDrevoDoOhne, 1);
                HighlightItem(goWoodBig);
                HighlightItem(goWoodSmall);
                UI.ArrowDown.GetComponent<FixedPosition>().Show();
        }
    }

    private void closeFire()
    {
        if(currentPhase == Phase.Fire_2)
        {
            fireplace.fuelCurr = 0;
        }
    }

    public void buttonClickStartGame(GameObject self)
    {
        StartPhase(Phase.Fire_2);
        Destroy(self);
    }

    //note: it could be no bueno if you dont start from begining -> going to further phases yould be broken
    public void StartPhase(Phase p)
    {
        switch (p)
        {
            case Phase.Start_1:
                currentPhase = Phase.Start_1;
                // UI.ButtonStart.GetComponent<Image>().DOFade(1, 2);
                break;
            case Phase.Fire_2:
                //new current phase
                currentPhase = Phase.Fire_2;
                UI.ArrowDown.GetComponent<FixedPosition>().Hide();
                //enable some stuff
                woodSmall.isPickable = true;
                woodBig.isPickable = true;
                Invoke("showTutorArrows", 3);
                Invoke("highlightWood", 9);
                Invoke("closeFire", 6);
                break;
            case Phase.ChildScream_3:
                if(startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.Fire_2);
                }

                //GameObject.FindGameObjectWithTag("PsycheStatusBck").GetComponent<PsycheStatus>().StartThatTimerBro();
                sound.PlaySound("childScreamStart");
                //narative
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                    new List<string>() { Txt.uLavice, Txt.zPlnaHrdla},
                    GameObject.FindGameObjectWithTag("ChildBody").transform.position);

                //new current phase
                currentPhase = Phase.ChildScream_3;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.dejDitetiHracky, 1).SetDelay(5);
                //enable some stuff
                child.InvokeRepeating("ScreamGraduates", 0, child.screamGraduatesIn);
                kohout.isPickable = true;
                kocar.isPickable = true;
                husar.isPickable = true;
                break;
            case Phase.Latch_4:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.ChildScream_3);
                }
                //narative
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                     new List<string>() { Txt.mlcHle, Txt.hrejsi},
                     GameObject.FindGameObjectWithTag("PlayerHead").transform.position);

                //protoze dones diteti hracky uz tam nema co delat
                UI.GameInfoTxt.GetComponent<Text>().DOText("", 1);

                //new current phase
                currentPhase = Phase.Latch_4;
                //new UI phase info
                //enable some stuff
                noonWitchMovement.enabled = true;
                latch.isPickable = true;

                break;
            case Phase.ChildRoot_5:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.Latch_4);
                }

                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().isGoingToThrowToys = true;//this has to be BEFORE narative code
               //narative
               GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                    new List<string>() { Txt.nezKohout, Txt.bouchBac },
                    GameObject.FindGameObjectWithTag("ChildBody").transform.position);
                //new current phase
                currentPhase = Phase.ChildRoot_5;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.nechSeChytitDitetem, 1).SetDelay(5);
                //enable some stuff
                child.SetGrabChance(100);
                //enable
                GetComponent<UITime>().startClockHard = true;
                GetComponent<UITime>().StartThatTimerBro();
                StartPhase(Phase.CompleteHard_6_NO_CHANGE_YET);
                break;
            case Phase.CompleteHard_6_NO_CHANGE_YET:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.ChildRoot_5);
                }
                DOTween.To(() => playerMovement.maxFadeValue,
                    x => playerMovement.maxFadeValue = x,
                    0, 60);

                //new current phase
                currentPhase = Phase.CompleteHard_6_NO_CHANGE_YET;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.hardMode, 1).SetDelay(2);

                break;
            case Phase.CompleteImpossible_7_NO_CHANGE_YET:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.CompleteHard_6_NO_CHANGE_YET);
                }
                //new current phase
                currentPhase = Phase.CompleteImpossible_7_NO_CHANGE_YET;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.hardMode, 1).SetDelay(5);
                //enable
                GetComponent<UITime>().startClockImpossible = true;

                break;
            case Phase.EndGame_8:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.CompleteImpossible_7_NO_CHANGE_YET);
                }
                UI.hideUI();
                playerRootMinigame.Unroot();

                Debug.Log("END GAME 8 START");
                //narative
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                    new List<string>() { Txt.pojdSi, Txt.pojdVem},
                    GameObject.FindGameObjectWithTag("PlayerHead").transform.position);


                playerAnim.StartScream();
                sound.PlaySound("MotherScream");

                //new current phase
                currentPhase = Phase.EndGame_8;
                //new UI phase info
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.ochranDite, 1).SetDelay(5);
                //enable stuff
                //dítě grab 100%

                //.. and some other stuff
                GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().UnSubscribeToNoonWitch(noonWitchObject);

                foreach(GameObject go in GameObject.FindGameObjectsWithTag("Window"))
                {
                    noonWitchObject.GetComponent<WindowColision>().UnSubscribeToNewItem(go);
                }
                noonWitchObject.GetComponent<WindowColision>().enabled = false;
                noonWitchObject.GetComponent<MovementBySimulatedInputHorizontal>().enabled = false;
                //walking mode
                noonWitchObject.GetComponent<WindowColision>().noonWitchWalking.SetActive(true);
                noonWitchObject.GetComponent<WindowColision>().noonWitchSpooking.SetActive(false);

                //reposition noonwitch
                noonWitchObject.transform.DOMove(GameObject.FindGameObjectWithTag("PointDoor").transform.position, 7).OnComplete(StartKnocking);

                /*noonWitchMovement.enabled = true;
                noonWitchMovement.moveState = MovementBySimulatedInputHorizontal.Move.Right;
                noonWitchMovement.speed = 2f;*/

                //throw equiped items
                if (playerPickItem.equipedItem)
                {
                    GameObject item = playerPickItem.equipedItem;
                    playerPickItem.equipedItem = null;
                    if (!item.GetComponent<Rigidbody2D>())
                    {
                        item.AddComponent<Rigidbody2D>();
                    }
                    else
                    {
                        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    }
                    if (playerAnim.gameObject.transform.localScale.x > 0)
                    {
                        item.GetComponent<Rigidbody2D>().AddForce(new Vector2(400, 300));
                        Destroy(item, 1);
                    }
                    else
                    {
                        item.GetComponent<Rigidbody2D>().AddForce(new Vector2(-400, 300));
                        Destroy(item, 1);
                    }
                }
                GameObject.FindGameObjectWithTag("Player").GetComponent<RootMinigame>().cleanse();

                //disable other minigames
                GameObject.FindGameObjectWithTag("Player").GetComponent<PickItems>().enabled = false;
                GameObject.FindGameObjectWithTag("Child").GetComponent<Child>().SetGrabChance(-1);

                while (GameObject.FindGameObjectWithTag("Child").transform.childCount > 2)
                {
                    GameObject.FindGameObjectWithTag("Child").transform.GetChild(2).parent = null;
                }
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
            case Phase.EndGameWin_9:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.EndGame_8);
                }
                sound.PlaySound("clockNoon");
                //new current phase
                currentPhase = Phase.EndGameWin_9;
                //new UI phase info
                //UI.GameInfoTxt.GetComponent<Text>().DOText("Father is back!", 1).SetDelay(5);

                //Noon witch FadeOut
                DOTween.To(() => noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color,
                    x => noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color = x,
                    new Color(
                        noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
                        noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
                        noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
                        0), 3);
                DOTween.To(() => noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color,
                    x => noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color = x,
                    new Color(
                        noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
                        noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
                        noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
                        0), 3);

                Invoke("winEpilog", 2f);
                break;
            case Phase.EndGameLoose:
                if (startingPhase != Phase.Start_1)
                {
                    StartPhase(Phase.EndGameWin_9);
                }
                sound.PlaySound("clockNoon");
                //new current phase
                currentPhase = Phase.EndGameLoose;


                playerAnim.StartFalling();
                GameObject.FindGameObjectWithTag("ChildBody").GetComponent<AnimationSettingsChild>().StartChildFalling();

                //narative
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                    new List<string>() { Txt.proKristovu, Txt.klesaSmyslu },
                    GameObject.FindGameObjectWithTag("PlayerHead").transform.position);
                Invoke("looseEpilog", 2f);
                break;
        }
    }

    private void looseEpilog()
    {
        UI.Fade.transform.GetComponent<Image>().DOFade(1f, 2.5f);
        Invoke("looseEpilogNarative", 3f);
    }
    private void winEpilog()
    {
        UI.Fade.transform.GetComponent<Image>().DOFade(1f, 2.5f);
        Invoke("winEpilogNarative", 3f);
    }

    private void looseEpilogNarative()
    {
        UI.showStartUpStuffExceptItsRestart();
        //narative
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
            new List<string>() { Txt.klikaCvakla, Txt.tataVchazi, Txt.veMdlobach, Txt.kNadram, Txt.matkuVzkrisil, Txt.avsakDite });

        //Invoke("showMother", 3f);
        //Invoke("showCross", 3f);
    }

    private void winEpilogNarative()
    {
        UI.showStartUpStuffExceptItsRestart();

        //narative
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
            new List<string>() { Txt.klikaCvakla, Txt.tataVchazi, Txt.marneHleda, Txt.diteRve, Txt.matkaRadostne, Txt.poledniceSvou });

        //Invoke("showMother", 3f);
        //Invoke("showCross", 3f);
    }

    private void showMother()
    {
        UI.Fade.transform.GetComponent<Image>().DOFade(1f, 3f);
        Invoke("looseEpilogNarative", 3f);
    }

    private void showCross()
    {
        UI.Fade.transform.GetComponent<Image>().DOFade(1f, 3f);
        Invoke("looseEpilogNarative", 3f);
    }

    public void StartKnocking()
    {
       /* noonWitchBody.transform.GetChild(0).transform.localScale = new Vector3(
            noonWitchBody.transform.GetChild(0).transform.localScale.x * -1,
            noonWitchBody.transform.GetChild(0).transform.localScale.y,
            noonWitchBody.transform.GetChild(0).transform.localScale.z);
        noonWitchBody.transform.GetChild(1).transform.localScale = new Vector3(
            noonWitchBody.transform.GetChild(1).transform.localScale.x * -1,
            noonWitchBody.transform.GetChild(1).transform.localScale.y,
            noonWitchBody.transform.GetChild(1).transform.localScale.z);

        noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color = new Color(
            noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
            noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
            noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
            0);
        noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color = new Color(
            noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
            noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
            noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
            0);
            */
        GameObject.FindGameObjectWithTag("Door").GetComponent<DOTweenAnimation>().DORestartById("Knock");
        GameObject.FindGameObjectWithTag("Door").GetComponent<DOTweenAnimation>().DOPlayById("Knock");


        //enable simulated move
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovementToThePoint>().enabled = true;

    }

    public void StartFinalMinigame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<RootMinigame>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovementByUserInputHorizontal>().enabled = false;

        GameObject.FindGameObjectWithTag("NoonWitch").GetComponent<InteractiveItem>().isUsable = true;

        //Noon witch FadeIn
        DOTween.To(() => noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color,
            x => noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color = x,
            new Color(
                noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
                noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
                noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
                1), 5);
        DOTween.To(() => noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color,
            x => noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color = x,
            new Color(
                noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
                noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
                noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
                1), 5);

        //zvuk otevreni dveri




        sound.PlaySound("noonWitchBreath");
        sound.StopSound("Tap");

        playerWalkCircle.SetActive(false);

        //narative
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
            new List<string>() {Txt.dejSem},
            new Vector3(GameObject.FindGameObjectWithTag("NoonWitch").transform.position.x, GameObject.FindGameObjectWithTag("NoonWitch").transform.position.y + 1.2f));


        Invoke("secondNarative", 1f);

        //to the front
        noonWitchBody.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().sortingLayerName = "Player";//berle
        noonWitchBody.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().sortingLayerName = "Player";//body

        noonWitchObject.transform.DOMove(GameObject.FindGameObjectWithTag("PointWall").transform.position, 40).SetEase(Ease.InOutCubic);

        Invoke("youWon", 28f);
        //start moving left
        // noonWitchMovement.enabled = true;
        //noonWitchMovement.moveState = MovementBySimulatedInputHorizontal.Move.Left;
        //!!! přes dotween..
        //noonWitchMovement.speed = 0.3f;

        //enable some kind of witch colision script
    }

    private void secondNarative()
    {
        sound.PlaySound("motherShout");
        //narative
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
            new List<string>() {Txt.odpustHrichy },
            GameObject.FindGameObjectWithTag("PlayerHead").transform.position);
    }

    private void youWon()
    {
        if(currentPhase == Phase.EndGame_8)
        {
            StartPhase(Phase.EndGameWin_9);
        }
    }

}
