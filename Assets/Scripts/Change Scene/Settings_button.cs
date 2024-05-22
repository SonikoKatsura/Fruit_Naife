using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings_button : MonoBehaviour
{
    // Start is called before the first frame update
    public void SettingsButton()
    {
        SceneManager.LoadScene("Settings"); // Call the LoadScene method of the SceneManager class to load a scene named "map"
    }
}
