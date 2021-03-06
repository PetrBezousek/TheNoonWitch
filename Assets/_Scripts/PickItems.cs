﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PickItems : MonoBehaviour {

    public delegate void OnItemUsed(GameObject sender);
    public event OnItemUsed OnWindowUsedEvent;

    public delegate void OnChildGotAllToys();
    public event OnChildGotAllToys OnChildGotAllToysEvent;

    public delegate void OnLatchWindow();
    public event OnLatchWindow OnLatchWindowEvent;

    SoundManager sound;

    [SerializeField]
    UIManager UI;

    [SerializeField]
    GameObject latch;

    [Space]
    [Header("Grab item range in pixels (Game is 18 pixels long)")]
    [SerializeField]
    [Range(0,30)]
    private int pickRange = 5;

    [SerializeField]
    private GameObject playersHand;

    [SerializeField]
    private WindowColision noonWitch;

    public GameObject theClosestPlace;

    public GameObject theClosestPickable;

    public GameObject equipedItem;

    [SerializeField]
    private GamePhases gamePhases;

    private void Awake()
    {
        sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    //Update
    private void PickItems_OnUpdateEvent()
    {
        if (theClosestPickable != null)
        {
            if (!IsInRangeWith(theClosestPickable) || theClosestPlace != null) {

                if (theClosestPlace != null)
                {
                    UI.ArrowDown.GetComponent<FixedPosition>().SetParent(theClosestPlace.transform);
                    UI.ArrowDown.GetComponent<FixedPosition>().Show();
                }
                else
                {
                    UI.ArrowDown.GetComponent<FixedPosition>().Hide();

                    theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();
                    theClosestPickable = null;
                }
            }
        }
        if (theClosestPlace != null)
        {
            if (!IsInRangeWith(theClosestPlace) || !IsFulfilingTermsOfPlace(theClosestPlace))
            {
                UI.ArrowDown.GetComponent<FixedPosition>().Hide();

                theClosestPlace.GetComponent<InteractiveItem>().UnHighlightSelf();
                theClosestPlace = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            //both have nullcheck
            UsePlace();
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
                                if(sender.GetComponent<InteractiveItem>().name != InteractiveItem.Names.Latch)
                                {
                                    SetNewClosestItemAndHighlightStatus(sender);
                                }
                                

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
    //TODO Refactor it a bit
    private void PickUpItem()
    {
        if(theClosestPickable != null)
        {
            theClosestPickable.GetComponent<InteractiveItem>().SetOwner(gameObject);

            UI.ArrowDown.GetComponent<FixedPosition>().Hide();

            if (theClosestPickable.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch)
            {
                gamePhases.itsTimeForHighligtingLatch = false;
                if (theClosestPickable.transform.childCount > 0 && theClosestPickable.transform.GetChild(0))
                {
                    // Destroy(theClosestPickable);
                }
            }

            //is it toy?
            if (theClosestPickable.GetComponent<Rigidbody2D>())
            {
                theClosestPickable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }

            if (equipedItem != null)
            {
                
                //přiřadím stav (kvůli tomu, aby se neposílali broadcasty)
                theClosestPickable.GetComponent<InteractiveItem>().isPickable = false;
                theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();
                equipedItem.GetComponent<InteractiveItem>().isPickable = true;

                equipedItem.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
                equipedItem.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(101, 10001);

                //prhodím pozice logicky
                GameObject temp = equipedItem;
                equipedItem = theClosestPickable;
                theClosestPickable = temp;

                //prohodím pozice fyzicky     
                theClosestPickable.transform.parent = null;

                if(equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch)
                {
                    theClosestPickable.transform.position = new Vector3(
                        equipedItem.transform.position.x-0.6f,
                        -2.75f,//constanta -> průměrná souřadnice podlahy
                        equipedItem.transform.position.z);
                }
                else
                {
                    if (theClosestPickable.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Husar)
                    {
                        theClosestPickable.transform.position = new Vector3(equipedItem.transform.position.x, Random.Range(-2.6f, -2.3f));
                    }
                    else
                    {
                        theClosestPickable.transform.position = new Vector3(equipedItem.transform.position.x, Random.Range(-2.8f, -2.4f));
                    }
                }

                SetEquipedItemToPosition(playersHand.transform);
                
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

            UpdatePlayersSpeed();
        }
        else //no item around -> drop equiped item
        {
            /* No bueno s charge mechanikou
             * 
            if(equipedItem != null)
            {
                equipedItem.GetComponent<InteractiveItem>().isPickable = true;
                
                equipedItem.transform.parent = null;
                equipedItem.transform.position = new Vector3(transform.position.x, transform.position.y - 5);
                equipedItem = null;
                Invoke("UpdatePlayersSpeed", 0.5f);//Aby nemohl hráč přehazovat věc -> získat tím rychlost (u sure bout that,)
            }*/
        }
    }

    private bool IsFulfilingTermsOfPlace(GameObject place)
    {
        if(equipedItem != null)
        {
            //add wood to the fire
            if((equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.WoodSmall || equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.WoodBig)
                && place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Fireplace)
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
        && (equipedItem != null) && (equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch)
        && 
            (
            //latch phase
                (
                GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.Latch_4
                && noonWitch.GetComponent<WindowColision>().isKnocking && noonWitch.lastWindow == place
                )
            ||  
            //any other phase
                (!noonWitch.isSpooking && GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase != GamePhases.Phase.Latch_4)

            )
        )
        {
            return true;
        }
        // child 
        if ((place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Child)
        && (equipedItem != null) 
        && ((equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Husar)
        || equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Kohout
        || equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Kocarek))
        {
            return true;
        }
        //noonwitch
        if ((place.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Noonwitch)
        && (GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.EndGame_8))
        {

            GameObject.FindGameObjectWithTag("NoonWitch").transform.DOPause();
            //gameover
            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().StartPhase(GamePhases.Phase.EndGameLoose);
            //narative
            /*
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoom>().doCinematicNarative(
                new List<string>() { "Pro Kristovu drahou muku!", "klesá smyslů zbavena."  },
                GameObject.FindGameObjectWithTag("PlayerHead").transform.position);
                */
            // Invoke("showResetBtn", 4f);

            return false;
        }

        //Default false
        return false;
    }

    private void showResetBtn()
    {
        UI.showStartUpStuffExceptItsRestart();


        //GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().btnReset.GetComponent<Button>().interactable = true;
        //GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().btnReset.GetComponent<Image>().DOFade(1, 1);
    }

    public void HighlightLatchStart()
    {
        gamePhases.itsTimeForHighligtingLatch = true;

        InvokeRepeating("HighlightLatch", 5, 3);
    }

    private void HighlightLatch()
    {
        if (gamePhases.itsTimeForHighligtingLatch && ((equipedItem != null && equipedItem.GetComponent<InteractiveItem>().name != InteractiveItem.Names.Latch) || equipedItem == null))
        {
            if(GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GamePhases>().currentPhase == GamePhases.Phase.Latch_4)
            {
                UI.GameInfoTxt.GetComponent<Text>().DOText(Txt.zablokujOkno, 1); 
            }
            gamePhases.HighlightItem(latch);
        }
        else
        {
            CancelInvoke("HighlightLatch");
        }
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
                        theClosestPlace.GetComponent<Fireplace>().AddWood(equipedItem.GetComponent<InteractiveItem>().name);
                        Destroy(equipedItem);

                        sound.PlaySound("addWood");
                        
                        equipedItem = null;
                        break;
                    case InteractiveItem.Names.Window:
                        sound.PlaySound("windowClose");
                        if (equipedItem != null && equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Latch)
                        {
                            gamePhases.itsTimeForHighligtingLatch = false;
                            if (OnLatchWindowEvent != null)
                            {
                                OnLatchWindowEvent();
                            }
                            equipedItem.GetComponent<InteractiveItem>().SetOwner(theClosestPlace);
                        }
                        else
                        {
                        }


                        //window listens to this
                        OnWindowUsedEvent(gameObject);
                        break; 
                    case InteractiveItem.Names.Table:
                        /*
                         * Asi nebude no
                         * 
                        theClosestPlace.GetComponent<Table>().isItemOnTable = true;
                        equipedItem.GetComponent<InteractiveItem>().isPickable = false;

                        SetEquipedItemToPosition(theClosestPlace.transform);

                        equipedItem = null;*/
                        break;
                    case InteractiveItem.Names.Child:

                        if (equipedItem != null)
                        {
                            equipedItem.GetComponent<InteractiveItem>().SetOwner(theClosestPlace);
                        }

                        theClosestPlace.GetComponent<Child>().isHavingToy = true;
                        theClosestPlace.GetComponent<Child>().numberOfToysHaving++;

                        if (theClosestPlace.GetComponent<Child>().numberOfToysHaving == 3)
                        {
                            if (OnChildGotAllToysEvent != null)
                            {

                                sound.PlaySound("childGotToys");
                                OnChildGotAllToysEvent();
                            }
                        }

                        //theClosestPlace.GetComponent<Child>().CheckScream();
               
                        
                        //position
                        equipedItem.transform.parent = theClosestPlace.transform;
                        equipedItem.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
                        if (equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Husar)
                        {
                            equipedItem.transform.localPosition = new Vector3(-2, 0, 0);
                        }else if(equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Kohout)
                        {
                            equipedItem.transform.localPosition = new Vector3(1, -0.05f, 0);
                        }
                        else if(equipedItem.GetComponent<InteractiveItem>().name == InteractiveItem.Names.Kocarek)
                        {
                            equipedItem.transform.localPosition = new Vector3(6, -0.1f, 0);
                        }


                        equipedItem.GetComponent<InteractiveItem>().isPickable = false;
                        equipedItem = null;//'couse I used it just now 
                        break;
                }
                UpdatePlayersSpeed();
               
            }
            
        }
    }

    private void OnEnable()
    {
        //subscribe to Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent += PickItems_OnUpdateEvent;

    }

    private void OnDisable()
    {
        //UNsubscribe from Update
        GameObject.FindGameObjectWithTag("GameLogic").GetComponent<UpdateManager>().OnUpdateEvent -= PickItems_OnUpdateEvent;
        
    }

    #region Helper Methods

    private void UpdatePlayersSpeed()
    {
        if(equipedItem != null)
        {
            GetComponent<MovementByUserInputHorizontal>().DebuffSpeed(equipedItem.GetComponent<InteractiveItem>().weight);
        }
        else
        {
            GetComponent<MovementByUserInputHorizontal>().DebuffSpeed(1);//reset - no debuff
        }
    }

    private void SetEquipedItemToPosition(Transform newParent)
    {
        if(equipedItem != null)
        {
            equipedItem.transform.parent = newParent;
            equipedItem.transform.localPosition = new Vector3(0, 0, 0);
            equipedItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            equipedItem.GetComponent<SpriteRenderer>().sortingOrder = 1005;//Behind FHand1 a 2
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
            UI.ArrowDown.GetComponent<FixedPosition>().Hide();
        }
        item.GetComponent<InteractiveItem>().HighlightSelf();

        UI.ArrowDown.GetComponent<FixedPosition>().SetParent(item.transform);
        UI.ArrowDown.GetComponent<FixedPosition>().Show();

        theClosestPlace = item;
    }

    //Found new closest item
    private void SetNewClosestItemAndHighlightStatus(GameObject item)
    {
        //place has priority -> if there is usable place
        if(theClosestPlace == null)
        {
            if (theClosestPickable != null)
            {
                theClosestPickable.GetComponent<InteractiveItem>().UnHighlightSelf();
                UI.ArrowDown.GetComponent<FixedPosition>().Hide();
            }
            item.GetComponent<InteractiveItem>().HighlightSelf();
        
            UI.ArrowDown.GetComponent<FixedPosition>().SetParent(item.transform);
            UI.ArrowDown.GetComponent<FixedPosition>().Show();
        
            theClosestPickable = item;
        }
        
    }

    //Is it though?
    private bool IsInRangeWith(GameObject item)
    {
        return (Mathf.Abs(item.transform.position.x - gameObject.transform.position.x) < pickRange)?true:false;
    }
#endregion
    
}
