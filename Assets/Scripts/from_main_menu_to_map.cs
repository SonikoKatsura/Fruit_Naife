using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class from_main_menu_to_map : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayButton() 
    {
        SceneManager.LoadScene("map"); // Call the LoadScene method of the SceneManager class to load a scene named "map"
    }

}
