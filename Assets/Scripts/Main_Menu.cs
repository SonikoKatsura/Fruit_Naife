using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void MenuButton()
    {
        SceneManager.LoadScene("Main Menu"); // Call the LoadScene method of the SceneManager class to load a scene named "map"
    }
}
