using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItems : MonoBehaviour {

    [SerializeField]
    [Range(0,30)]
    private int pickRange = 5;

    [SerializeField]
    private GameObject playersHand;

    public GameObject theClosestPlace;

    public GameObject theClosestPickable;

    public GameObject equipedItem;

    // Use this for initialization
    void Start () {

        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += PickItems_OnUpdateEvent; 

    }

    //Update
    private void PickItems_OnUpdateEvent()
    {
        if (theClosestPickable != null)
        {
            if (!IsInRangeWith(theClosestPickable)) {

                theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();
                theClosestPickable = null;
            }
        }
        if (theClosestPlace != null)
        {
            if (!IsInRangeWith(theClosestPlace) || !IsFulfilingTermsOfPlace(theClosestPlace))
            {
                theClosestPlace.GetComponent<InteractiveItem>().UnHighlightSelf();
                theClosestPlace = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UsePlace();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            PickUpItem();
        }
    }

    //Semi-Update (it happens every update) Info broadcasted from item
    private void PickItems_OnUpdateNotifyAboutItselfEvent(GameObject sender)
    {
        switch (sender.GetComponent<InteractiveItem>().type)
        {
            case InteractiveItem.Types.Pickable:
                if (sender.GetComponent<InteractiveItem>().isPickable)
                {
                    //Is item in range of player?
                    if (IsInRangeWith(sender))
                    {
                        if (theClosestPickable != null)
                        {
                            //is item closer?
                            if (Mathf.Abs(sender.transform.position.x - gameObject.transform.position.x) < Mathf.Abs(theClosestPickable.transform.position.x - gameObject.transform.position.x))
                            {
                                SetNewClosestItemAndHighlightStatus(sender);

                            }
                        }
                        else { SetNewClosestItemAndHighlightStatus(sender); }
                    }
                }
                break;
            case InteractiveItem.Types.Place:
                if (sender.GetComponent<InteractiveItem>().isUsable)
                {
                        //Is item in range of player?
                        if (IsInRangeWith(sender) && IsFulfilingTermsOfPlace(sender))
                        {
                            if (theClosestPlace != null)
                            {
                                //is item closer?
                                if (Mathf.Abs(sender.transform.position.x - gameObject.transform.position.x) < Mathf.Abs(theClosestPlace.transform.position.x - gameObject.transform.position.x))
                                {
                                    SetNewClosestPlaceAndHighlightStatus(sender);

                                }
                            }
                            else { SetNewClosestPlaceAndHighlightStatus(sender); }
                        }
                        
                         
                }
                break;
        }
       
    }

    //Start listening to item
    public void SubscribeToNewItem(GameObject item)
    {
        item.GetComponent<BroadcastItselfToPlayer>().OnUpdateNotifyAboutItselfEvent += PickItems_OnUpdateNotifyAboutItselfEvent;
    }

    //Stop listening to item
    public void UnSubscribeToNewItem(GameObject item)
    {
        item.GetComponent<BroadcastItselfToPlayer>().OnUpdateNotifyAboutItselfEvent -= PickItems_OnUpdateNotifyAboutItselfEvent;
    }

    //Pick nearest item and put on its position currently equiped item
    private void PickUpItem()
    {
        if(theClosestPickable != null)
        {
            if (equipedItem != null)
            {
                //přiřadím stav (kvůli tomu, aby se neposílali broadcasty)
                theClosestPickable.GetComponent<InteractiveItem>().isPickable = false;
                theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();
                equipedItem.GetComponent<InteractiveItem>().isPickable = true;

                //prhodím pozice logicky
                GameObject temp = equipedItem;
                equipedItem = theClosestPickable;
                theClosestPickable = temp;

                //prohodím pozice fyzicky      
                theClosestPickable.transform.parent = null;
                theClosestPickable.transform.position = equipedItem.transform.position;
                equipedItem.transform.parent = playersHand.transform;
                equipedItem.transform.localPosition = new Vector3(0, 0, 0);
                // equipedItem.transform.position = theClosestItem.transform.position;

                // equipedItem.transform.position = theClosestItem.transform.position;



                theClosestPickable = null;               
            }
            else//first time pickup (empty hand)
            {
                //stav
                theClosestPickable.GetComponent<InteractiveItem>().isPickable = false;
                theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();

                //logika
                equipedItem = theClosestPickable;
                theClosestPickable = null;

                //fyzika
                equipedItem.transform.parent = playersHand.transform;
                equipedItem.transform.localPosition = new Vector3(0, 0, 0);
                
            }
        }
    }

    private bool IsFulfilingTermsOfPlace(GameObject place)
    {
        if(equipedItem != null)
        {
            //add wood to the fire
            if(equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Wood && place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Fireplace)
            {
                return true;
            } 
            
        }
        // close/lock window 
            if(place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Window)
            {
                return true;
            }
        
        //Default false
        return false;
    }

    //make operation with given place
    private void UsePlace()
    {
        if (theClosestPlace != null)
        {
            if (IsFulfilingTermsOfPlace(theClosestPlace))
            {
                switch (theClosestPlace.GetComponent<InteractiveItem>().name)
                {
                    case InteractiveItem.Names.Fireplace:
                        theClosestPlace.GetComponent<Fireplace>().AddWood();
                        Destroy(equipedItem);
                        break;
                    case InteractiveItem.Names.Window:
                        
                            if((equipedItem != null) && (equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch))
                            {
                                theClosestPlace.GetComponent<Window>().ChangeStateTo(Window.State.Latched);
                                //dej petlici na okno
                            }
                            else if(theClosestPlace.GetComponent<Window>().windowState == Window.State.Opened)
                            {
                                theClosestPlace.GetComponent<Window>().ChangeStateTo(Window.State.Closed);
                            }
                            
                        break;
                }
               
            }
            
        }
    }

    //Found new closest place
    private void SetNewClosestPlaceAndHighlightStatus(GameObject item)
    {
        if (theClosestPlace != null)
        {
            theClosestPlace.GetComponent<InteractiveItem>().UnHighlightSelf();
        }
        item.GetComponent<InteractiveItem>().HighlightSelf();
        theClosestPlace = item;
    }

    //Found new closest item
    private void SetNewClosestItemAndHighlightStatus(GameObject item)
    {
        if (theClosestPickable != null)
        {
            theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();
        }
        item.GetComponent<InteractiveItem>().HighlightSelf();
        theClosestPickable = item;
    }

    //Is it though?
    private bool IsInRangeWith(GameObject item)
    {
        return (Mathf.Abs(item.transform.position.x - gameObject.transform.position.x) < pickRange)?true:false;
    }

}
