using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootMinigame : MonoBehaviour {

    [SerializeField]
    Text UI;

    [SerializeField]
    int difficulty = 6;

    [SerializeField]
    float secToUnrootIfFailed = 5f;

    List<char> task;

    bool isTaskFailed = false;

    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += RootMinigame_OnUpdateEvent;

        task = new List<char>();

        for (int i = 0; i < difficulty; i++)
        {
            if(Random.Range(0,2) == 0)
            {
                task.Add('↑');
            }
            else
            {
                task.Add('↓');
            }
        }


        //just a UI stuff
        //....
        string s = "";
        foreach (char character in task)
        {
            s += character;
        }
        UI.text = s;
        //....
    }

    //Update
    private void RootMinigame_OnUpdateEvent()
    {
        if (!isTaskFailed)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(task[0] == '↑')
                {
                    task.RemoveAt(0);

                    //just a UI stuff
                    //....
                    string s = "";
                    foreach(char character in task){
                        s += character;
                    }
                    UI.text = s;
                    //....

                    if (task.Count == 0) { Unroot(); }
                }
                else
                {
                    isTaskFailed = true;
                    UI.text = "FAILED!";
                    Invoke("Unroot", secToUnrootIfFailed);
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (task[0] == '↓')
                {
                    task.RemoveAt(0);

                    //just a UI stuff
                    //....
                    string s = "";
                    foreach (char character in task)
                    {
                        s += character;
                    }
                    UI.text = s;
                    //....
                    if (task.Count == 0) { Unroot(); }
                }
                else
                {
                    isTaskFailed = true;
                    UI.text = "FAILED!";
                    Invoke("Unroot", secToUnrootIfFailed);
                }
            }
        }
        
    }

    public void MinigameStartsSoon()
    {
        //anim/sounds
        UI.text = "Root!";
    }

    private void Unroot()
    {
        enabled = false;
        GetComponent<MovementByUserInputHorizontal>().enabled = true;
        GetComponent<PickItems>().enabled = true;
        isTaskFailed = false;
        UI.text = "Released!";
    }

    private void OnDisable()
    {
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= RootMinigame_OnUpdateEvent;
       
    }
}
