using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{

    // Public AudioSource component that will play the music.
    public AudioSource AudioSource;

    // Private float variable to store the music volume, initialized to 1 (full volume).
    private float musicvolume = 1f;

    // Public Slider UI component to adjust the volume.
    public Slider volumeSlider;

    // Start is called before the first frame update.
    void Start()
    {
        // Play the audio as soon as the game starts.
        AudioSource.Play();

        // Retrieve the previously saved volume level from PlayerPrefs (if it exists).
        musicvolume = PlayerPrefs.GetFloat("volume", 1f); // Default value is 1f if "volume" key doesn't exist.

        // Set the AudioSource volume to the retrieved value.
        AudioSource.volume = musicvolume;

        // Set the Slider's value to match the current volume level.
        volumeSlider.value = musicvolume;
    }

    // Update is called once per frame.
    void Update()
    {
        // Continuously set the AudioSource volume to the current value of musicvolume.
        AudioSource.volume = musicvolume;

        // Save the current volume level to PlayerPrefs to persist between sessions.
        PlayerPrefs.SetFloat("volume", musicvolume);
    }

    // Public method to set the volume, can be called from UI elements like the Slider.
    public void SetVolume(float volume)
    {
        // Update the musicvolume variable with the new value.
        musicvolume = volume;
    }


}
