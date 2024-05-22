using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Go_play : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlaygButton()
    {
        SceneManager.LoadScene("Game"); // Call the LoadScene method of the SceneManager class to load a scene named "map"
    }
}
