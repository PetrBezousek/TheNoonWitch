using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static AudioClip unrootFirstTime, motherScream, motherShout, noonWitchBreath, potCooking, tickTock, addWood, childEvil, childScreamStart, childScream1, childScream2, childScream3, clickCorrect, fireplaceRunOut, noonWitchScream, rootMinigameStart, windowKnock, windowClose, error, clockNoon, childGotToys;

    public static AudioClip[] tappityTap = new AudioClip[6];
    public static AudioClip[] scream = new AudioClip[3];
    private string nextTapSound;

    static AudioSource audioSrc;

    private bool isWalking;
    private bool isCooking;

    public static AudioSource pot;
    public static AudioSource player;
    public static AudioSource camera;

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        pot = GameObject.FindGameObjectWithTag("Pot").GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        tappityTap[0] = Resources.Load<AudioClip>("Tap1");
        tappityTap[1] = Resources.Load<AudioClip>("Tap2");
        tappityTap[2] = Resources.Load<AudioClip>("Tap3");
        tappityTap[3] = Resources.Load<AudioClip>("Tap4");
        tappityTap[4] = Resources.Load<AudioClip>("Tap5");
        tappityTap[5] = Resources.Load<AudioClip>("Tap6");
        scream[0] = Resources.Load<AudioClip>("childScream1");
        scream[1] = Resources.Load<AudioClip>("childScream2");
        scream[2] = Resources.Load<AudioClip>("childScream3");
        motherScream = Resources.Load<AudioClip>("MotherScreamAnger");
        potCooking = Resources.Load<AudioClip>("PotCooking");
        tickTock = Resources.Load<AudioClip>("TickTock");
        windowClose = Resources.Load<AudioClip>("WindowClose");
        windowKnock = Resources.Load<AudioClip>("WindowKnock");
        error = Resources.Load<AudioClip>("Error");
        rootMinigameStart = Resources.Load<AudioClip>("RootMinigameStart");
        noonWitchScream = Resources.Load<AudioClip>("NoonWitchScream");
        fireplaceRunOut = Resources.Load<AudioClip>("FireplaceRunOut");
        clickCorrect = Resources.Load<AudioClip>("ClickCorrect");
        addWood = Resources.Load<AudioClip>("AddWood");
        childGotToys = Resources.Load<AudioClip>("ChildGotToys");
        clockNoon = Resources.Load<AudioClip>("ClockNoon");
        childScream1 = Resources.Load<AudioClip>("ChildScream1");
        childScream2 = Resources.Load<AudioClip>("ChildScream2");
        childScream3 = Resources.Load<AudioClip>("ChildScream3");
        childEvil = Resources.Load<AudioClip>("ChildGrabEvil");
        childScreamStart = Resources.Load<AudioClip>("ChildScreamStart");
        noonWitchBreath = Resources.Load<AudioClip>("noonWitchBreath");
        motherShout = Resources.Load<AudioClip>("motherShout");
        unrootFirstTime = Resources.Load<AudioClip>("unrootFirstTime");
    }

    public AudioClip getRandomTapSound()
    {
        int rng = Random.Range(0, tappityTap.Length);
        return tappityTap[rng];
    }

    private void PlayTapSound()
    {
        if (isWalking)
        {
            AudioClip clip = getRandomTapSound();
            player.PlayOneShot(clip);
            Invoke("PlayTapSound", clip.length);
        }
    }
    private void PlayCookingSound()
    {
        if (!pot.isPlaying && isCooking)
        {
            pot.PlayOneShot(potCooking);
            Invoke("PlayCookingLoop", potCooking.length - 5f);
        }
    }
    private void PlayCookingLoop()
    {
        if (isCooking)
        {
            pot.PlayOneShot(potCooking);
            Invoke("PlayCookingLoop", potCooking.length - 5f);
        }
    }

    private AudioClip GetChildScreamSound()
    {
        int rng = Random.Range(0, scream.Length);
        return scream[rng];
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "MotherScream":
                audioSrc.PlayOneShot(motherScream);
                break;
            case "PotCooking":
                isCooking = true;
                PlayCookingSound();
                break;
            case "Tap":
                isWalking = true;
                PlayTapSound();
                break;
            case "TickTock":
                camera.PlayOneShot(tickTock);
                break;
            case "error":
                audioSrc.PlayOneShot(error);
                break;
            case "addWood":
                audioSrc.PlayOneShot(addWood);
                break;
            case "clickCorrect":
                audioSrc.PlayOneShot(clickCorrect);
                break;
            case "fireplaceRunOut":
                audioSrc.PlayOneShot(fireplaceRunOut);
                break;
            case "noonWitchScream":
                audioSrc.PlayOneShot(noonWitchScream);
                break; 
            case "rootMinigameStart":
                audioSrc.PlayOneShot(rootMinigameStart);
                break;
            case "windowClose":
                audioSrc.PlayOneShot(windowClose);
                break;
            case "windowKnock":
                audioSrc.PlayOneShot(windowKnock);
                break;
            case "childGotToys":
                audioSrc.PlayOneShot(childGotToys);
                break;
            case "clockNoon":
                audioSrc.PlayOneShot(clockNoon);
                break;
            case "childEvil":
                audioSrc.PlayOneShot(childEvil);
                break;
            case "childScreamStart":
                audioSrc.PlayOneShot(childScreamStart);
                break;
            case "childScream":
                audioSrc.PlayOneShot(GetChildScreamSound());
                break;
            case "noonWitchBreath":
                audioSrc.PlayOneShot(noonWitchBreath);
                break;
            case "motherShout":
                audioSrc.PlayOneShot(motherShout);
                break; 
            case "unrootFirstTime":
                audioSrc.PlayOneShot(unrootFirstTime);
                break;
        }
    }

    public void StopSound(string clip)
    {
        switch (clip)
        {
            case "MotherScream":
                audioSrc.Stop();
                break;
            case "PotCooking":
                pot.Stop();
                isCooking = false;
                break;
            case "Tap":
                isWalking = false;
                break;
            case "TickTock":
                camera.Stop();
                break;
        }
    }
}
