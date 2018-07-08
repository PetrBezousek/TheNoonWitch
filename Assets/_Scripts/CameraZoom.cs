using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraZoom : MonoBehaviour {

    public float boundaryLeft;
    public float boundaryRight;

    public GameObject blackBarDown;
    public GameObject blackBarUp;

    public bool isZoomedIn = false;


    public GameObject txtNarative;
    public GameObject txtNarativeSpawn;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zoomOnObject(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            zoomOut();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(txtNarative,txtNarativeSpawn.transform,false);
        }
    }

    public void zoomOnObject(Vector3 position)
    {
        //camera needs to be in house
            float x = position.x;
            if (x < boundaryLeft) { x = boundaryLeft; }
            if (x > boundaryRight) { x = boundaryRight; }

        if (isZoomedIn)
        {
            //just reposition
            transform.DOMoveX(x, 0.8f);
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
            transform.DOMoveX(x, 0.8f);          
        }

    }

    public void zoomOut()
    {
        isZoomedIn = false;
        
        //camera move
        transform.DOMoveX(0, 1);
        GetComponent<DOTweenAnimation>().DOPlayBackwardsById("zoomIn");

        //black bars slide out
        blackBarDown.GetComponent<DOTweenAnimation>().DOPlayBackwardsById("moveIn");
        blackBarUp.GetComponent<DOTweenAnimation>().DOPlayBackwardsById("moveIn");
    }
}
