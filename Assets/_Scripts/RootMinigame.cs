using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMinigame : MonoBehaviour {

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
                task.Add('x');
            }
            else
            {
                task.Add('c');
            }
        }


        //just a debug stuff
        //....
        string s = "";
        foreach (char character in task)
        {
            s += character;
        }
        Debug.Log(s);
        //....
    }

    //Update
    private void RootMinigame_OnUpdateEvent()
    {
        if (!isTaskFailed)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if(task[0] == 'x')
                {
                    task.RemoveAt(0);

                    //just a debug stuff
                    //....
                    string s = "";
                    foreach(char character in task){
                        s += character;
                    }
                    Debug.Log(s);
                    //....
                
                    if (task.Count == 0) { Unroot(); }
                }
                else
                {
                    isTaskFailed = true;
                    Debug.Log("FAILED!");
                    Invoke("Unroot", secToUnrootIfFailed);
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (task[0] == 'c')
                {
                    task.RemoveAt(0);

                    //just a debug stuff
                    //....
                    string s = "";
                    foreach (char character in task)
                    {
                        s += character;
                    }
                    Debug.Log(s);
                    //....
                    if (task.Count == 0) { Unroot(); }
                }
                else
                {
                    isTaskFailed = true;
                    Debug.Log("FAILED!");
                    Invoke("Unroot", secToUnrootIfFailed);
                }
            }
        }
        
    }

    private void Unroot()
    {
        enabled = false;
        GetComponent<MovementByUserInputHorizontal>().enabled = true;
        GetComponent<PickItems>().enabled = true;
        isTaskFailed = false;
        Debug.Log("Released!");
    }

    private void OnDisable()
    {
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= RootMinigame_OnUpdateEvent;
       
    }
}
