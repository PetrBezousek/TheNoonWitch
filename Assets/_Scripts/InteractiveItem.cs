using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour {
    
    public bool isPickable = true;
    public bool isUsable { get; set; }

    public enum Names {Husar, Kohout, WoodSmall,WoodBig,Fireplace,Window,Child, Latch, Kocarek, Table}
    public enum Types {Pickable, Place}
    
    public Names name;
    public Types type;
    [Space]
    [Header("Multiplier to players maximum speed (0.75 = tree-quarter of max speed)")]
    [Range(0,1)]
    public float weight;

    public GameObject owner;

    private bool highlight { get; set; }

	// Use this for initialization
	void Start () {
        highlight = false;
        isUsable = true;
    }

    public void SetOwner(GameObject newOwner)
    {

        //old one
        if(owner != null && owner.GetComponent<InteractiveItem>() && owner.GetComponent<InteractiveItem>().name == Names.Window)
        {
            owner.GetComponent<Window>().ChangeStateTo(Window.State.Closed);
        }

        //old one
        if (owner != null && owner.GetComponent<InteractiveItem>() && owner.GetComponent<InteractiveItem>().name == Names.Child)
        {
            owner.GetComponent<Child>().isHavingToy = false;
        }

        owner = newOwner;//switch
    }

    public void HighlightSelf()
    {
        highlight = true;
        GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 0.5f);
        // Debug.Log("Highlighted!");
    }

    public void UnHighlightSelf()
    {
        highlight = false;
        GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 1f);
        // Debug.Log("UnHighlighted!");
    }
    
}
