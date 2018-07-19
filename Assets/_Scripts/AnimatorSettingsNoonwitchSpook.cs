using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimatorSettingsNoonwitchSpook : MonoBehaviour {

    [SerializeField]
    private Animator anim;

    [SerializeField]
    [Range(0, 2)]
    float speedIdle;
    
    private string currentAnim;
    
    private void Start()
    {
        //TODO color changing dotween
        DOTween.To(() => GetComponent<Anima2D.SpriteMeshInstance>().color,
            x => GetComponent<Anima2D.SpriteMeshInstance>().color = x,
            new Color(
                0.502f,
                0.322f,
                0.082f,
                1), 5).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InBounce);
        StartIdle();
    }

    public void StartIdle()
    {

        anim.Play("NoonwitchSpook");
        anim.speed = speedIdle;
        currentAnim = "NoonwitchSpook";
    }

}
