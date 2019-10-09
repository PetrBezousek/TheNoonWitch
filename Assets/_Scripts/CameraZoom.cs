using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CameraZoom : MonoBehaviour {

    public bool isCinematicPlaying = false;

    public float boundaryLeft;
    public float boundaryRight;

    public float boundaryUp;
    public float boundaryDown;

    public GameObject blackBarDown;
    public GameObject blackBarUp;

    public bool isZoomedIn = false;

    public List<string> narative;

    public GameObject prefabTxtNarative;
    public GameObject txtNarativeSpawn;

    public bool isGoingToThrowToys = false;

    [SerializeField]
    private UIManager UI;

    public void doCinematicNarative(List<string> _narative, Vector3 zoomPoint)
    {
        if (!GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().skipCinematics && !isCinematicPlaying)
        {
            isCinematicPlaying = true;
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().StopSound("Tap");
            UI.hideUI();
            UI.ArrowDown.SetActive(false);
            narative = _narative;
            zoomOnObject(zoomPoint);
        }
    }


    public void doCinematicNarative(List<string> _narative)
    {
        if (!GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().skipCinematics)
        {
            isCinematicPlaying = true;
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().StopSound("Tap");
            UI.hideUI();
            UI.ArrowDown.SetActive(false);
            narative = _narative;
            OnZoomedIn();
        }

    }

    public void OnZoomedIn()
    {
        GetComponent<narativeLines>().setLines(narative);
        GetComponent<narativeLines>().showNarative();

        Time.timeScale = 0.1f;

        //throw
        if (isGoingToThrowToys)
        {
            GameObject.FindGameObjectWithTag("Child").GetComponent<Child>().SetNumberOfSkips();
            GameObject.FindGameObjectWithTag("Child").GetComponent<Child>().CheckScream();
            isGoingToThrowToys = false;
        }

        GameObject.FindGameObjectWithTag("SlowMotionFilter").GetComponent<Image>().DOColor(new Color(0.831f, 0.761f, 0.416f, 0.4f), 0.1f);
    }



    public void zoomOnObject(Vector3 position)
    {
        //camera needs to be in house
            float x = position.x;
            if (x < boundaryLeft) { x = boundaryLeft; }
            if (x > boundaryRight) { x = boundaryRight; }
        float y = position.y;
        if (y < boundaryDown) { y = boundaryDown; }
        if (y > boundaryUp) { y = boundaryUp; }
        
        
        if (isZoomedIn)
        {
            //just reposition
            transform.DOMove(new Vector3(x,y), 0.4f).OnComplete(OnZoomedIn);
           
        }
        else
        {
            isZoomedIn = true;

            //black bars slide in
            blackBarDown.GetComponent<DOTweenAnimation>().DORestartById("moveIn");
            blackBarDown.GetComponent<DOTweenAnimation>().DOPlayById("moveIn");
            blackBarUp.GetComponent<DOTweenAnimation>().DORestartById("moveIn");
            blackBarUp.GetComponent<DOTweenAnimation>().DOPlayById("moveIn");

            //camera move
            GetComponent<DOTweenAnimation>().DORestartById("zoomIn");
            GetComponent<DOTweenAnimation>().DOPlayById("zoomIn");
            transform.DOMove(new Vector3(x, y,-2), 0.4f).OnComplete(OnZoomedIn);
        }

    }

    public void zoomOut()
    {
        isCinematicPlaying = false;
        if (GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase != GamePhases.Phase.EndGame_8)
        {
            UI.showUI();
        }

        isZoomedIn = false;
        
        //camera move
        transform.DOMove(new Vector3(0, 0,-1), 0.5f);
        GetComponent<DOTweenAnimation>().DOPlayBackwardsById("zoomIn");

        //black bars slide out
        blackBarDown.GetComponent<DOTweenAnimation>().DOPlayBackwardsById("moveIn");
        blackBarUp.GetComponent<DOTweenAnimation>().DOPlayBackwardsById("moveIn");
    }
}
