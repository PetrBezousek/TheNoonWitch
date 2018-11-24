using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static AudioClip motherScream, potCooking, tickTock;

    public static AudioClip[] tappityTap = new AudioClip[6];
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
        motherScream = Resources.Load<AudioClip>("MotherScreamAnger");
        potCooking = Resources.Load<AudioClip>("PotCooking");
        tickTock = Resources.Load<AudioClip>("TickTock");
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
