using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RootMinigame : MonoBehaviour {

    public delegate void OnChildRoot();
    public event OnChildRoot OnChildRootEvent;

    [SerializeField]
    Text UIInfo;

    [SerializeField]
    GameObject particleSystem;


    [Space]
    [Header("How many keys player needs to click")]
    [SerializeField]
    public int difficultyChildRoot = 6;
    [SerializeField]
    public int difficultyEndgame = 6;

    [Space]
    [Header("How long till unroot after succesion")]
    [SerializeField]
    float secToUnrootIfSucceded = 1f;

    [Space]
    [Header("How long will be player rooted")]
    [SerializeField]
    float secToUnrootIfFailed = 5f;
    [Space]
    [Header("How long will player wait till new task")]
    [SerializeField]
    float secToGetNewTask = 3f;
    [Space]
    [Space]
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform child;

    [Space]
    [Header("How much will player be moved (Game is 18 units long)")]
    [SerializeField]
    float moveForwardX;
    [SerializeField]
    float moveBackwardX;
    [SerializeField]
    float childOffsetX;

    List<GameObject> task;

    [SerializeField]
    GameObject taskParent;

    [SerializeField]
    GameObject left;


    [SerializeField]
    Anima2D.SpriteMeshInstance[] pWholeBody;

    SoundManager sound;

    [SerializeField]
    GameObject right;

    [SerializeField]
    GameObject psyche;

    bool isTaskFailed = false;

    [SerializeField]
    bool isChildRoot = true;

    bool firstTime = true;

    //kvuli moznemu bugu pokud jsi v rootu pri prechodu do endgame
    public void cleanse()
    {
        foreach (Transform t in taskParent.transform)
        {
            Destroy(t.gameObject);
        }
        task.Clear();
        GetComponent<MovementByUserInputHorizontal>().enabled = true;
        GameObject.FindGameObjectWithTag("ChildBody").GetComponent<AnimationSettingsChild>().StartChildGrabIdleBlend();
        enabled = false;
    }

    private void Awake()
    {
        task = new List<GameObject>();
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void OnEnable()
    {   

        if (GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGame_8)
        {
            //set difficulty etc
            isChildRoot = false;
            GameObject.FindGameObjectWithTag("Child").GetComponent<Child>().SetGrabChance(0);
            float psyche = (GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheCurr / GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheMax)*100;

            if (psyche >= 50)
            {
                difficultyEndgame = 3;
            }
            else if (psyche >= 30)
            {
                difficultyEndgame = 5;
            }
            else if (psyche >= 15)
            {
                difficultyEndgame = 7;
            }
            else if (psyche >= 0)
            {
                difficultyEndgame = 10;
            }
        }
        else
        {
            sound.PlaySound("childEvil");
        }
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += RootMinigame_OnUpdateEvent;

        NewTask();
    }

    //Update
    private void RootMinigame_OnUpdateEvent()
    {
        if (!isTaskFailed)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (taskParent.transform.GetChild(0).GetComponent<Tag>().rootArrow == Tag.ArrowType.Left)
                {
                    sound.PlaySound("clickCorrect");
                    if (taskParent.transform.childCount >= 2)
                    {
                        GameObject highlight = Instantiate(particleSystem, taskParent.transform.GetChild(1), false);
                        Destroy(taskParent.transform.GetChild(0).gameObject);
                    }

                    if (taskParent.transform.childCount == 1) {
                        Destroy(taskParent.transform.GetChild(0).gameObject);
                        if (isChildRoot)
                        {
                            TaskFinished("Success", "Unroot", secToUnrootIfSucceded);
                        }
                        else
                        {
                            MovePlayerBackwards();
                            TaskFinished("Succcess", "NewTask", secToGetNewTask);
                        }
                    }
                }
                else
                {
                    sound.PlaySound("error");
                    if (isChildRoot)
                    {
                        foreach (Transform t in taskParent.transform)
                        {
                            Destroy(t.gameObject);
                        }

                        foreach (Anima2D.SpriteMeshInstance bodyPart in pWholeBody)
                        {
                            //lock state
                            bodyPart.color = new Color(0.3333333f, 0.2745098f, 0f);

                            //unlock  in X seconds
                            DOTween.To(() => bodyPart.color, x => bodyPart.color = x, new Color(1, 1, 1), secToUnrootIfFailed).SetEase(Ease.InBack);
                        }
                        TaskFinished("Fail!", "Unroot", secToUnrootIfFailed);
                    }
                    else
                    {
                        foreach (Transform t in taskParent.transform)
                        {
                            Destroy(t.gameObject);
                        }
                        MovePlayerForwards();
                        TaskFinished("Fail!", "NewTask", secToGetNewTask);
                    }

                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (taskParent.transform.GetChild(0).GetComponent<Tag>().rootArrow == Tag.ArrowType.Right)
                {
                    sound.PlaySound("clickCorrect");
                    if (taskParent.transform.childCount >= 2)
                    {
                        GameObject highlight = Instantiate(particleSystem, taskParent.transform.GetChild(1), false);
                        Destroy(taskParent.transform.GetChild(0).gameObject);
                    }

                    if (taskParent.transform.childCount == 1)
                    {
                        Destroy(taskParent.transform.GetChild(0).gameObject);
                        if (isChildRoot)
                        {
                            TaskFinished("Success", "Unroot", secToUnrootIfSucceded);
                        }
                        else
                        {
                            MovePlayerBackwards();
                            isTaskFailed = true;

                            TaskFinished("Success", "NewTask", secToGetNewTask);
                        }
                    }

                }
                else
                {
                    sound.PlaySound("error");
                    if (isChildRoot)
                    {
                        foreach(Transform t in taskParent.transform)
                        {
                            Destroy(t.gameObject);
                        }

                        foreach (Anima2D.SpriteMeshInstance bodyPart in pWholeBody)
                        {
                            //lock state
                            bodyPart.color = new Color(0.3333333f, 0.2745098f, 0f);

                            //unlock  in X seconds
                            DOTween.To(() => bodyPart.color, x => bodyPart.color = x, new Color(1, 1, 1), secToUnrootIfFailed).SetEase(Ease.InBack);
                        }
                        TaskFinished("Fail", "Unroot", secToUnrootIfFailed);
                    }
                    else
                    {
                        foreach (Transform t in taskParent.transform)
                        {
                            Destroy(t.gameObject);
                        }
                        MovePlayerForwards();
                        TaskFinished( "Fail","NewTask", secToGetNewTask);
                    }
                }
            }
        }

    }

    private void TaskFinished(string msg, string invokeNext, float afterXSeconds)
    {
        // TODO znic vsechny sipky

        isTaskFailed = true;
       // UIInfo.text = msg;//todo delete, replace with smth
        Invoke(invokeNext, afterXSeconds);
    }

    public void MinigameStartsSoon()
    {
        //anim/sounds
        //UIInfo.text = "Root!";
    }

    public void Unroot()
    {

        if (firstTime)
        {
            firstTime = false;

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                new List<string>() { Txt.iBodejz, Txt.zeNa, Txt.polednici },
                GameObject.FindGameObjectWithTag("PlayerHead").transform.position);
    
            sound.PlaySound("unrootFirstTime");

        }

        enabled = false;
        GetComponent<MovementByUserInputHorizontal>().enabled = true;
        if (GetComponent<MovementByUserInputHorizontal>().startChargedMove)
        {
            GetComponent<MovementByUserInputHorizontal>().anim.StartRunning();
        }
        GetComponent<PickItems>().enabled = true;
        isTaskFailed = false;
        GameObject.FindGameObjectWithTag("ChildBody").GetComponent<AnimationSettingsChild>().StartChildGrabIdleBlend();
    }

    private Color setColor(int difficulty)
    {
        float psyche = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheCurr / GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheMax;

        Color c;

        if (difficulty >= 10)
        {
            c = new Color(0.64f, 0.64f, 0.64f);
        }
        else if (difficulty >= 7)
        {
            c = new Color(0.44f, 0.56f, 0.64f);
        }
        else if (difficulty >= 5)
        {
            c = new Color(0.24f, 0.47f, 0.64f);
        }
        else if (difficulty >= 3)
        {
            c = new Color(0.04f, 0.38f, 0.64f);
        }
        else
        {
            c = new Color(0.04f, 0.38f, 0.64f);
        }

        return c;
    }

    private void NewTask()
    {
        //reset task list
        for (int i = 0; i < taskParent.transform.childCount; i++)
        {
            Destroy(taskParent.transform.GetChild(0));
        }

        for (int i = 0; i < (isChildRoot ? difficultyChildRoot : difficultyEndgame); i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                GameObject arrow = Instantiate(left, taskParent.transform);
                arrow.GetComponent<Image>().color = setColor(i);
            }
            else
            {
                GameObject arrow = Instantiate(right, taskParent.transform);
                arrow.GetComponent<Image>().color = setColor(i);
            }
        }
        if (isChildRoot)
        {
            difficultyChildRoot++;
        }
        else
        {
            // difficultyEndgame++;
        }
        isTaskFailed = false;

        GameObject highlight = Instantiate(particleSystem, taskParent.transform.GetChild(0), false);
    }

    private void MovePlayerForwards()
    {
        /*player.transform.position = new Vector3(
            player.transform.position.x + moveForwardX,
            player.transform.position.y,
            player.transform.position.z);*/
        if (GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGame_8)
        {
            player.DOMoveX(player.transform.position.x + moveForwardX, 1);
            child.DOMoveX(player.transform.position.x + moveForwardX + childOffsetX, 1);

        }
    }

    private void MovePlayerBackwards()
    {
        if(player.transform.position.x <= GetComponent<MovementByUserInputHorizontal>().boundXLeft)
        {
            player.DOMoveX(GetComponent<MovementByUserInputHorizontal>().boundXLeft, 1);
            child.DOMoveX(GetComponent<MovementByUserInputHorizontal>().boundXLeft + childOffsetX, 1);
        }
        else
        {
            player.DOMoveX(player.transform.position.x - moveBackwardX, 1);
            child.DOMoveX(player.transform.position.x - moveBackwardX + childOffsetX, 1);
        }
        /*  player.transform.position = new Vector3(
              player.transform.position.x - moveBackwardX,
              player.transform.position.y,
              player.transform.position.z);*/
    }

    private void OnDisable()
    {
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= RootMinigame_OnUpdateEvent;
    }
}
