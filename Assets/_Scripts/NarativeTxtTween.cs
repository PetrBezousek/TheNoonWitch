using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class NarativeTxtTween : MonoBehaviour {

    public GameObject spawn;
    public Vector3 mid;
    public Vector3 midStay;
    public Vector3 right;
    
    public void moveIn()
    {
        transform.DOMove(mid,0.03f).OnComplete(moveMidStay);
    }
    
    public void moveMidStay()
    {
        transform.DOMove(midStay, 0.18f).OnComplete(moveOut).SetEase(Ease.InBack);
    }

    public void moveOut()
    {
        transform.DOMove(right, 0.03f).OnComplete(showNextOne).SetEase(Ease.InBack);
    }

    public void showNextOne()
    {
        //if some line is left
        if (spawn.transform.childCount > 1)
        {
            spawn.transform.GetChild(1).GetComponent<NarativeTxtTween>().moveIn();
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().zoomOut();

           // GameObject.FindGameObjectWithTag("Player").GetComponent<MovementByUserInputHorizontal>().enabled = true;
            GameObject.FindGameObjectWithTag("SlowMotionFilter").GetComponent<Image>().DOColor(new Color(0.831f,0.761f,0.416f,0f), 0.5f);
            Time.timeScale = 1;
        }

        destroy();

    }
    public void destroy()
    {
        Destroy(gameObject);
    }
    

    private void Start()
    {

        if (spawn.transform.GetChild(0).gameObject == gameObject)
        {

            moveIn();
        }
    }
    
}
