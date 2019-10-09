using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    public GameObject ArrowUp;
    [SerializeField]
    public GameObject ArrowDown;
    [SerializeField]
    public GameObject ArrowLeft;
    [SerializeField]
    public GameObject ArrowRight;

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
    [SerializeField]
    public GameObject PsycheCounter;
    [SerializeField]
    public GameObject PsycheIcon;
    [SerializeField]
    public GameObject StartFilter;
    [SerializeField]
    public GameObject[] StartArrows;
    [SerializeField]
    public GameObject StartText;

    [SerializeField]
    public GameObject Fade;

    [SerializeField]
    public GameObject flagCZ;
    [SerializeField]
    public GameObject flagENG;
    [SerializeField]
    public GameObject zapsplat;
    [SerializeField]
    public GameObject madeBy;

    public void hideStartUpStuff()
    {
        StartFilter.GetComponent<SpriteRenderer>().DOFade(0, 0.4f);
        for (int i = 0; i < StartArrows.Length; i++)
        {
            StartArrows[i].GetComponent<SpriteRenderer>().DOFade(0, 0.4f);
        }
        StartText.GetComponent<Text>().DOFade(0, 0.4f);
        madeBy.GetComponent<Text>().DOFade(0, 0.4f);
        flagENG.GetComponent<SpriteRenderer>().DOFade(0, 0.4f);
        zapsplat.GetComponent<Image>().DOFade(0, 0.4f);
        flagCZ.GetComponent<SpriteRenderer>().DOFade(0, 0.4f);
    }


    public void showStartUpStuffExceptItsRestart()
    {
        StartFilter.GetComponent<SpriteRenderer>().DOFade(1, 0.4f);
        for (int i = 0; i < 2; i++)
        {
            StartArrows[i].GetComponent<SpriteRenderer>().DOFade(1, 0.4f);
        }
        StartText.GetComponent<Text>().text = Txt.restart;
        StartText.GetComponent<Text>().DOFade(1, 0.4f);
        madeBy.GetComponent<Text>().DOFade(1, 0.4f);
        //flagENG.GetComponent<SpriteRenderer>().DOFade(1, 0.4f);
        zapsplat.GetComponent<Image>().DOFade(1, 0.4f);
        //flagCZ.GetComponent<SpriteRenderer>().DOFade(1, 0.4f);
    }

    public void hideUI()
    {
        ArrowDown.SetActive(false);
        Fire.SetActive(false);
        ArrowLeft.SetActive(false);
        ArrowRight.SetActive(false);
        NoonWitch.SetActive(false);
        Child.SetActive(false);
        ScreamStreakCount.SetActive(false);
        PsycheCounter.SetActive(false);
        PsycheIcon.SetActive(false);
    }

    public void showUI()
    {
        ArrowDown.SetActive(true);
        ArrowLeft.SetActive(false);
        ArrowRight.SetActive(false);
        Fire.SetActive(true);
        NoonWitch.SetActive(true);
        Child.SetActive(true);
        ScreamStreakCount.SetActive(true);
        PsycheCounter.SetActive(true);
        PsycheIcon.SetActive(true);
    }
}
