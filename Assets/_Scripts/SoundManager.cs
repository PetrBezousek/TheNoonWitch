using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

   public static AudioClip motherScream, potCooking;

    static AudioSource audioSrc;

    public static AudioSource pot;

    private void Awake()
    {
        pot = GameObject.FindGameObjectWithTag("Pot").GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        motherScream = Resources.Load<AudioClip>("MotherScreamAnger");
        potCooking = Resources.Load<AudioClip>("PotCooking");
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "MotherScream":
                audioSrc.PlayOneShot(motherScream);
                break;
            case "PotCooking":
                if (!pot.isPlaying)
                {
                    pot.Play();
                }
                break;
        }
    }

    public static void StopSound(string clip)
    {
        switch (clip)
        {
            case "MotherScream":
                audioSrc.Stop();
                break;
            case "PotCooking":
                pot.Stop();
                break;
        }
    }
}
