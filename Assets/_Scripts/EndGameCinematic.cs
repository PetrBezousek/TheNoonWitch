using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameCinematic : MonoBehaviour {

    [SerializeField]
    GameObject fade;

    [SerializeField]
    GameObject dad;

    [SerializeField]
    GameObject noonwitch;

    [SerializeField]
    int fadeTime;

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Win1();
        }*/
    }

    public void Win1()
    {
        //fade out
        fade.GetComponent<Image>()
            .DOFade(1, fadeTime)
            .SetEase(Ease.OutExpo)
            .OnComplete(Win3A);
    }

    public void Win2()
    {
        //DING DING 4x
        //odbije 12 nezávisle na cinematicu??
        
        //- pokud hráč "vyhraje" před příchodem polednice, tak začne odbíjet 
        //    hned tak přijde
        
        //- pokud hráč "prohraje" ještě pře vypršením času, tak až poté odbije

    }

    public void Win3A()
    {
        //fade in
        fade.GetComponent<Image>()
            .DOFade(0, fadeTime)
            .SetEase(Ease.OutExpo);


        //zoom na hodiny (12)

        Invoke("Win3B", 2);
    }

    public void Win3B()
    {
        //fade out
        fade.GetComponent<Image>()
            .DOFade(1, fadeTime)
            .SetEase(Ease.OutExpo)
            .OnComplete(Win4);

        //door open (sound)
    }

    public void Win4()
    {
        //fade in
        fade.GetComponent<Image>()
            .DOFade(0, fadeTime)
            .SetEase(Ease.OutExpo);

        //otec ve dveřích
        dad.SetActive(true);

        //polednice mizi
        DOTween.To(() => noonwitch.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color,
            x => noonwitch.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color = x,
            new Color(
                noonwitch.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
                noonwitch.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
                noonwitch.transform.GetChild(0).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
                0), 3);
        DOTween.To(() => noonwitch.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color,
            x => noonwitch.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color = x,
            new Color(
                noonwitch.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.r,
                noonwitch.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.g,
                noonwitch.transform.GetChild(1).GetComponent<Anima2D.SpriteMeshInstance>().color.b,
                0), 3);
   
        Invoke("Win5", 5);
    }

    public void Win5()
    {
        //fade out
        fade.GetComponent<Image>()
            .DOFade(1, fadeTime)
            .SetEase(Ease.OutExpo)
            .OnComplete(Win6);
    }

    public void Win6()
    {
        //fade in
        fade.GetComponent<Image>()
            .DOFade(0, fadeTime)
            .SetEase(Ease.OutExpo);

        //kolaz rodiny

        //otec v obklopeni rodiny - pozadi uz neni hra ale praznda obrazovka -> titulky
    }

    public void Win7()
    {
        //roll the credits!!
        
    }

    public void Loose1()
    {
        //(matka klesla smyslů zbavena)
        //fade out
        fade.GetComponent<Image>()
            .DOFade(1, fadeTime)
            .SetEase(Ease.OutExpo)
            .OnComplete(Loose2);
    }

    public void Loose2()
    {
        //DING, Ding
        fade.GetComponent<Image>()
            .DOFade(0, fadeTime)
            .SetEase(Ease.OutExpo);
        Invoke("Loose3", 2);

        //hodiny
    }

    public void Loose3()
    {
        //fade in
        fade.GetComponent<Image>()
            .DOFade(0, fadeTime)
            .SetEase(Ease.OutExpo);

        //poledice zmizela
        noonwitch.SetActive(false);

        //door open (sound)

        //otec ve dveřích
        dad.SetActive(true);
    }

    public void Loose4()
    {
        //fade out
        fade.GetComponent<Image>()
            .DOFade(1, fadeTime)
            .SetEase(Ease.OutExpo)
            .OnComplete(Loose5);
    }

    public void Loose5()
    {
        //fade in
        fade.GetComponent<Image>()
            .DOFade(0, fadeTime)
            .SetEase(Ease.OutExpo);

        //kolaz rodiny

        //pozadi uz neni hra ale praznda obrazovka -> titulky
        //dítě mizí, místo něj se zjeví náhrobek
    }
}
