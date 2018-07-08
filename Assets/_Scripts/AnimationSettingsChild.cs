﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class AnimationSettingsChild : MonoBehaviour {

    [SerializeField]
    private Animator anim;

    [SerializeField]
    [Range(0, 2)]
    float speedIdle;

    [SerializeField] SpriteMeshInstance leftHand;
    [SerializeField] SpriteMeshInstance rightHand;


    [SerializeField]
    [Range(0, 2)]
    float speedChildScream;
    [SerializeField]
    [Range(0, 2)]
    float speedChildGrab;
    [SerializeField]
    [Range(0, 2)]
    float speedChildGrabMax;
    [SerializeField]
    [Range(0, 10)]
    float speedChildGrabIdleBlend;


    private string currentAnim;


    private void Start()
    {
        anim.Play("ChildIdle");
        anim.speed = speedIdle;
        currentAnim = "ChildIdle";
    }

    public void StartIdle()
    {

        anim.Play("ChildIdle");
        anim.speed = speedIdle;
        currentAnim = "ChildIdle";
    }
    public void StartChildScream()
    {
        if (currentAnim == "ChildIdle")
        {
            anim.Play("ChildScream");
            anim.speed = speedChildScream;
            currentAnim = "ChildScream";
        }
    }

    public void StartChildGrab()
    {
        if (currentAnim != "ChildGrab")
        {
            anim.Play("ChildGrab");
            anim.speed = speedChildGrab;
            currentAnim = "ChildGrab";
        }
    }

    public void StartChildGrabMax()
    {
        anim.Play("ChildGrabMax");
        anim.speed = speedChildGrabMax;
        currentAnim = "ChildGrabMax";
    }
    public void StartChildGrabIdleBlend()
    {
        anim.Play("ChildGrabIdleBlend");
        anim.speed = speedChildGrabIdleBlend;
        currentAnim = "ChildGrabIdleBlend";
    }

    public void putHandsInFront()
    {
        leftHand.sortingLayerName = "Player";
        leftHand.sortingOrder = 2000;

        rightHand.sortingLayerName = "Player";
        rightHand.sortingOrder = 2001;
    }

    public void putHandsInBack()
    {
        leftHand.sortingLayerName = "Items";
        leftHand.sortingOrder = 151;

        rightHand.sortingLayerName = "Items";
        rightHand.sortingOrder = 149;
    }
}
