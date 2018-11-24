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
    int difficultyChildRoot = 6;
    [SerializeField]
    int difficultyEndgame = 6;

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
    [Header("How much will player be moved (Game is 18 pixels long)")]
    [SerializeField]
    float moveForwardX;
    [SerializeField]
    float moveBackwardX;

    List<GameObject> task;

    [SerializeField]
    GameObject taskParent;

    [SerializeField]
    GameObject left;

    [SerializeField]
    GameObject right;

    bool isTaskFailed = false;

    [SerializeField]
    bool isChildRoot = true;
    

    private void OnEnable()
    {
        if(GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGame_8_DONT_USE)
        {
            //set difficulty etc
            isChildRoot = false;
            GameObject.FindGameObjectWithTag("Child").GetComponent<Child>().SetGrabChance(0);
        }
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += RootMinigame_OnUpdateEvent;

        NewTask();
    }

    //Update
    private void RootMinigame_OnUpdateEvent()
    {
        if (!isTaskFailed)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (taskParent.transform.GetChild(0).GetComponent<Tag>().rootArrow == Tag.ArrowType.Left)
                {
                    if(taskParent.transform.childCount >= 2)
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
                    if (isChildRoot)
                    {
                        foreach (Transform t in taskParent.transform)
                        {
                            Destroy(t.gameObject);
                        }
                        TaskFinished("Fail!", "Unroot", secToUnrootIfFailed);
                    }
                    else
                    {
                        MovePlayerForwards();
                        TaskFinished("Fail!", "NewTask", secToGetNewTask);
                    }
                    
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (taskParent.transform.GetChild(0).GetComponent<Tag>().rootArrow == Tag.ArrowType.Right)
                {
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

                    if (isChildRoot)
                    {
                        foreach(Transform t in taskParent.transform)
                        {
                            Destroy(t.gameObject);
                        }
                        TaskFinished("Fail", "Unroot", secToUnrootIfFailed);
                    }
                    else
                    {
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
            }
            else
            {
                GameObject arrow = Instantiate(right, taskParent.transform);
            }
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
        player.DOMoveX(player.transform.position.x + moveForwardX, 1);
        child.DOMoveX(player.transform.position.x + moveForwardX, 1);
    }

    private void MovePlayerBackwards()
    {
        if(player.transform.position.x <= GetComponent<MovementByUserInputHorizontal>().boundXLeft - moveBackwardX)
        {
            player.DOMoveX(GetComponent<MovementByUserInputHorizontal>().boundXLeft, 1);
            child.DOMoveX(GetComponent<MovementByUserInputHorizontal>().boundXLeft, 1);
        }
        else
        {
            player.DOMoveX(player.transform.position.x - moveBackwardX, 1);
            child.DOMoveX(player.transform.position.x - moveBackwardX, 1);
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
