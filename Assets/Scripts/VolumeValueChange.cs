using Oculus.Interaction.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{

    private AudioSource audioSRC;
    private float musicvolume = 1f;

    void Start()
    {
       audioSRC  = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        audioSRC.volume = musicvolume;
    }
    public void SetVolume(float volume)
    {
        musicvolume = volume;
    }


}
