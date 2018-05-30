using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItems : MonoBehaviour {

    public delegate void OnItemUsed(GameObject sender);
    public event OnItemUsed OnWindowUsedEvent;

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
    
    //Pick nearest item and put on its position currently equiped item
    private void PickUpItem()
    {
        if(theClosestPickable != null)
        {
            theClosestPickable.GetComponent<InteractiveItem>().SetOwner(gameObject);

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
                SetEquipedItemToPosition(playersHand.transform);
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
                SetEquipedItemToPosition(playersHand.transform);
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
        if((place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Window)
        &&(place.GetComponent<Window>().windowState == Window.State.Opened)
        ||
        (place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Window)
        && (place.GetComponent<Window>().windowState == Window.State.Closed)
        && (equipedItem != null) && (equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch))
        {
            return true;
        }
        // child 
        if ((place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Child)
        && (equipedItem != null) 
        && (!place.GetComponent<Child>().isHavingToy)
        && ((equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Husar)
        || equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Kohout))
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

                        if (equipedItem != null)
                        {
                            equipedItem.GetComponent<InteractiveItem>().SetOwner(theClosestPlace);
                        }

                        //window listens to this
                        OnWindowUsedEvent(gameObject);
                        break;
                    case InteractiveItem.Names.Child:

                        if (equipedItem != null)
                        {
                            equipedItem.GetComponent<InteractiveItem>().SetOwner(theClosestPlace);
                        }

                        theClosestPlace.GetComponent<Child>().isHavingToy = true;
                        theClosestPlace.GetComponent<Child>().Scream();

                        //position
                        equipedItem.transform.parent = theClosestPlace.transform;
                        equipedItem.transform.localPosition = new Vector3(0, 0, 0);

                        equipedItem.GetComponent<InteractiveItem>().isPickable = false;
                        equipedItem = null;//'couse I used it just now 
                        break;
                }
               
            }
            
        }
    }
    
    #region Helper Methods

    private void SetEquipedItemToPosition(Transform newParent)
    {
        if(equipedItem != null)
        {
            equipedItem.transform.parent = newParent;
            equipedItem.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    //Start listening to item
    public void SubscribeToNewItemBroadcast(GameObject item)
    {
        item.GetComponent<BroadcastItselfToPlayer>().OnUpdateNotifyAboutItselfEvent += PickItems_OnUpdateNotifyAboutItselfEvent;
    }

    //Stop listening to item
    public void UnSubscribeToNewItemBroadcast(GameObject item)
    {
        item.GetComponent<BroadcastItselfToPlayer>().OnUpdateNotifyAboutItselfEvent -= PickItems_OnUpdateNotifyAboutItselfEvent;
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
#endregion
    
}
