using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPosition : MonoBehaviour {

    [SerializeField]
    Transform parent;

    [SerializeField]
    float offsetX;
    [SerializeField]
    float offsetY;

    [SerializeField]
    bool isHidden;

    // Use this for initialization
    void Start () {
        if (isHidden)
        {
            Hide();
        }

        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += FixedPosition_OnUpdateEvent;
	}

    private void FixedPosition_OnUpdateEvent()
    {

        if(parent != null && !isHidden)
        {
            transform.position = new Vector3(parent.position.x + offsetX, parent.position.y + offsetY);
        }
    }

    public void SetParent(Transform newParent)
    {
        parent = newParent;
        transform.position = new Vector3(parent.position.x + offsetX, parent.position.y + offsetY);
    }

    public void SetParent(Transform newParent, float offsetXAxis, float offsetYAxis)
    {
        parent = newParent;
        offsetX = offsetXAxis;
        offsetY = offsetYAxis;
    }

    public void FlipX(bool willFaceRight)
    {
        if (willFaceRight && transform.localScale.x <0)
        {
            offsetX *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(parent.position.x + offsetX, parent.position.y + offsetY);
        }

        if (!willFaceRight && transform.localScale.x > 0)
        {
            offsetX *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(parent.position.x + offsetX, parent.position.y + offsetY);
        }
    }

    public void Hide()
    {
        if (!isHidden)
        {
            //UI
            if (GetComponent<CanvasRenderer>())
            {
                isHidden = true;
                GetComponent<CanvasRenderer>().SetAlpha(0f);//alpha
            
            }

            //Game object
            if (GetComponent<Renderer>())
            {
                isHidden = true;
                GetComponent<Renderer>().material.color =
                new Color(
                    GetComponent<Renderer>().material.color.r,
                    GetComponent<Renderer>().material.color.g,
                    GetComponent<Renderer>().material.color.b,
                    0f);//alpha
            }

        }
       
    }

    public void Show()
    {
        if (isHidden)
        {
            //UI
            if (GetComponent<CanvasRenderer>())
            {
                isHidden = false;
                GetComponent<CanvasRenderer>().SetAlpha(1f);//alpha
            }

            //Game object
            if (GetComponent<Renderer>())
            {
                isHidden = false;
                GetComponent<Renderer>().material.color =
                new Color(
                    GetComponent<Renderer>().material.color.r,
                    GetComponent<Renderer>().material.color.g,
                    GetComponent<Renderer>().material.color.b,
                    1f);//alpha
            }
        }
        
    }
}
