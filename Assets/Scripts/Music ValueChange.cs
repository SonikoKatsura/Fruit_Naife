using Oculus.Interaction.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{

    public AudioSource AudioSource;
    private float musicvolume = 1f;
    public Slider volumeSlider;

    void Start()
    {
       AudioSource.Play();
        musicvolume = PlayerPrefs.GetFloat("volume");
        AudioSource.volume = musicvolume;
        volumeSlider.value = musicvolume;  
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource.volume = musicvolume;
        PlayerPrefs.SetFloat("volume", musicvolume);
    }
    public void SetVolume(float volume)
    {
        musicvolume = volume;
    }


}
