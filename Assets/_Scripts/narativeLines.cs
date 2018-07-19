using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class narativeLines : MonoBehaviour {

    private List<string> lines;

    [SerializeField]
    private GameObject spawn;

    [SerializeField]
    private GameObject mid;

    [SerializeField]
    private GameObject midStay;

    [SerializeField]
    private GameObject right;
    

    [SerializeField]
    private GameObject prefab;

    public void setLines(List<string> narative)
    {
        lines = narative;
    }

    private void Start()
    {
            spawn = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().txtNarativeSpawn;
    }
    public void showNarative()
    {
        //spawn every line of narative
        foreach(string line in lines)
        {
            GameObject nextLine = prefab;
            nextLine.GetComponent<Text>().text = line;
            nextLine.GetComponent<NarativeTxtTween>().mid = mid.transform.position;
            nextLine.GetComponent<NarativeTxtTween>().midStay = midStay.transform.position;
            nextLine.GetComponent<NarativeTxtTween>().right = right.transform.position;
            nextLine.GetComponent<NarativeTxtTween>().spawn = spawn;
            

            Instantiate(nextLine, spawn.transform, false);
        }

    }



}
