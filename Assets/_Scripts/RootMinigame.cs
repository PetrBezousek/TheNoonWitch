using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootMinigame : MonoBehaviour {

    public delegate void OnChildRoot();
    public event OnChildRoot OnChildRootEvent;

    [SerializeField]
    Text UIInfo;

    [Space]
    [Header("How many keys player needs to click")]
    [SerializeField]
    int difficultyChildRoot = 6;
    [SerializeField]
    int difficultyEndgame = 6;

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

    [Space]
    [Header("How much will player be moved (Game is 18 pixels long)")]
    [SerializeField]
    float moveForwardX;
    [SerializeField]
    float moveBackwardX;

    List<char> task;

    bool isTaskFailed = false;

    [SerializeField]
    bool isChildRoot = true;
    

    private void OnEnable()
    {
        if(GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGame_8_DONT_USE)
        {
            //set difficulty etc
            isChildRoot = false;
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
                if (task[0] == '←')
                {
                    task.RemoveAt(0);

                    //just a UI stuff
                    //....
                    string s = "";
                    foreach(char character in task){
                        s += character;
                    }
                    UIInfo.text = s;
                    //....

                    if (task.Count == 0) {
                        if (isChildRoot)
                        {
                            Unroot();
                        }
                        else
                        {
                            MovePlayerBackwards();
                            isTaskFailed = true;
                            Invoke("NewTask", secToGetNewTask);
                        }
                    }
                }
                else
                {
                    if (isChildRoot)
                    {
                        isTaskFailed = true;
                        UIInfo.text = "FAILED!";
                        Invoke("Unroot", secToUnrootIfFailed);
                    }
                    else
                    {
                        isTaskFailed = true;
                        UIInfo.text = "FAILED!";
                        MovePlayerForwards();
                        Invoke("NewTask", secToGetNewTask);
                    }
                    
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (task[0] == '→')
                {
                    task.RemoveAt(0);

                    //just a UI stuff
                    //....
                    string s = "";
                    foreach (char character in task)
                    {
                        s += character;
                    }
                    UIInfo.text = s;
                    //....
                    if (task.Count == 0)
                    {
                        if (isChildRoot)
                        {
                            Unroot();
                        }
                        else
                        {
                            MovePlayerBackwards();
                            isTaskFailed = true;
                            Invoke("NewTask", secToGetNewTask);
                        }
                    }

                }
                else
                {

                    if (isChildRoot)
                    {
                        isTaskFailed = true;
                        UIInfo.text = "FAILED!";
                        Invoke("Unroot", secToUnrootIfFailed);
                    }
                    else
                    {
                        isTaskFailed = true;
                        UIInfo.text = "FAILED!";
                        MovePlayerForwards();
                        Invoke("NewTask", secToGetNewTask);
                    }
                }
            }
        }
        
    }

    public void MinigameStartsSoon()
    {
        //anim/sounds
        UIInfo.text = "Root!";
    }

    private void Unroot()
    {
        enabled = false;
        GetComponent<MovementByUserInputHorizontal>().enabled = true;
        GetComponent<PickItems>().enabled = true;
        isTaskFailed = false;
        UIInfo.text = "Released!";
        Invoke("ClearTxt", 1);
    }

    private void NewTask()
    {
        task = new List<char>();

        for (int i = 0; i < (isChildRoot ? difficultyChildRoot : difficultyEndgame); i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                task.Add('←');
            }
            else
            {
                task.Add('→');
            }
        }


        //just a UI stuff
        //....
        string s = "";
        foreach (char character in task)
        {
            s += character;
        }
        UIInfo.text = s;
        //....

        isTaskFailed = false;
    }

    private void MovePlayerForwards()
    {
        player.transform.position = new Vector3(
            player.transform.position.x + moveForwardX,
            player.transform.position.y,
            player.transform.position.z);
    }

    private void MovePlayerBackwards()
    {
        player.transform.position = new Vector3(
            player.transform.position.x - moveBackwardX,
            player.transform.position.y,
            player.transform.position.z);
    }

    private void ClearTxt()
    {
        UIInfo.text = "";
        if (OnChildRootEvent != null)
        {
            OnChildRootEvent();
        }
    }

    private void OnDisable()
    {
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= RootMinigame_OnUpdateEvent;
       
    }
}
