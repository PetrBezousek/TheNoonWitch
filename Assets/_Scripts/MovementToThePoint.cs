using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementToThePoint : MonoBehaviour {

    [SerializeField]
    private GameObject body;

    [SerializeField]
    private float animTime;

    [SerializeField]
    private float point;

    private void OnEnable()
    {
        if(transform.position.x < GameObject.FindGameObjectWithTag("Child").transform.position.x)
        {
            body.GetComponent<FixedPosition>().FlipX(true);
        }
        else
        {
            body.GetComponent<FixedPosition>().FlipX(false);
        }

        body.GetComponent<AnimatorSettings>().StartRunningFinaly();
        transform.DOMoveX(point, animTime).OnComplete(OnGetToChild);
        
    }
    

    public void OnGetToChild()
    {
        body.GetComponent<FixedPosition>().FlipX(true);
        
        body.GetComponent<AnimatorSettings>().StartPickingUpChild();       
    }
}
