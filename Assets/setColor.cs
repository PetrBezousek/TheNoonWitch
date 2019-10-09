using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    private Animator mindAnimator;

    [SerializeField]
    private Sprite mind1;
    [SerializeField]
    private Sprite mind2;
    [SerializeField]
    private Sprite mind3;
    [SerializeField]
    private Sprite mind4;
    [SerializeField]
    private Sprite mind5;

    private void Update()
    {
    }

    [SerializeField]
    UIManager UI;

    public void updateColor()
    {
        float psycheCurr = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheCurr;
        float psycheMax = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<Psyche>().psycheMax;

        if (((psycheCurr / psycheMax) * 100) >= 50)
        {
            UI.GetComponent<UIManager>().PsycheIcon.GetComponent<SpriteRenderer>().sprite = mind1;
        }
        else if (((psycheCurr / psycheMax) * 100) >= 30)
        {
            UI.GetComponent<UIManager>().PsycheIcon.GetComponent<SpriteRenderer>().sprite = mind2;
        }
        else if (((psycheCurr / psycheMax) * 100) >= 15)
        {
            UI.GetComponent<UIManager>().PsycheIcon.GetComponent<SpriteRenderer>().sprite = mind3;
        }
        else if (((psycheCurr / psycheMax) * 100) >= 0)
        {
            UI.GetComponent<UIManager>().PsycheIcon.GetComponent<SpriteRenderer>().sprite = mind4;
        }
        else
        {
            UI.GetComponent<UIManager>().PsycheIcon.GetComponent<SpriteRenderer>().sprite = mind5;
        }

        

    }
}
