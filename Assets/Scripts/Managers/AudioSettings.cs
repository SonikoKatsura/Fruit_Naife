using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider music_slider;
    [SerializeField] Slider sfx_slider;



    private float savedMusicVolume = 1;
    private float savedSFXVolume = 1;
    private float selectedMusicVolume = 1;
    private float selectedSFXVolume = 1;

    void Start()
    {
        savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
        savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        selectedMusicVolume = savedMusicVolume;
        selectedSFXVolume = savedSFXVolume;
        LoadMusicVolume(savedMusicVolume);
        LoadSFXVolume(savedMusicVolume);
    }
    void LoadMusicVolume(float volume)
    {
        music_slider.value = volume;
    }
    void LoadSFXVolume(float volume)
    {
        sfx_slider.value = volume;
    }
    public void ChangeMusicVolume()
    {
        if (selectedMusicVolume != music_slider.value)
        {
            selectedMusicVolume = music_slider.value;
            AudioManager.instance.ChangeMusicVolume(music_slider.value);
            PlayerPrefs.SetFloat("MusicVolume", selectedMusicVolume);
        }
    }

    public void ChangeSFXVolume()
    {
        if (selectedSFXVolume != sfx_slider.value)
        {
            selectedSFXVolume = sfx_slider.value;
            AudioManager.instance.ChangeMusicVolume(sfx_slider.value);
            PlayerPrefs.SetFloat("SFXVolume", selectedSFXVolume);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}