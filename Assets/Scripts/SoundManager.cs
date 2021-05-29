using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip windSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        windSound = Resources.Load<AudioClip>("strong wind blowing");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
