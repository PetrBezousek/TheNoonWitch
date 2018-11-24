using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField]
    public GameObject ArrowUp;
    [SerializeField]
    public GameObject ArrowDown;

    [SerializeField]
    public GameObject ButtonStart;

    [SerializeField]
    public GameObject Fire;
    [SerializeField]
    public GameObject NoonWitch;
    [SerializeField]
    public GameObject Child;
    [SerializeField]
    public GameObject ScreamStreakCount;
    [SerializeField]
    public GameObject GameInfoTxt;

    public void hideUI()
    {
        ArrowDown.SetActive(false);
        Fire.SetActive(false);
        NoonWitch.SetActive(false);
        Child.SetActive(false);
        ScreamStreakCount.SetActive(false);
    }

    public void showUI()
    {
        ArrowDown.SetActive(true);
        Fire.SetActive(true);
        NoonWitch.SetActive(true);
        Child.SetActive(true);
        ScreamStreakCount.SetActive(true);
    }
}
