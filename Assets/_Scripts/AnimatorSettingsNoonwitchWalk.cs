using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSettingsNoonwitchWalk : MonoBehaviour {

    [SerializeField]
    private Animator anim;

    [SerializeField]
    [Range(0, 2)]
    float speedIdle;

    private string currentAnim;

    private void Start()
    {
        StartWalk();
    }

    public void StartWalk()
    {

        anim.Play("NoonwitchWalk");
        anim.speed = speedIdle;
        currentAnim = "NoonwitchWalk";
    }

}
