using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click : MonoBehaviour
{
    public AudioSource clicksound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playThisSoundEffect()
    {
        clicksound.Play();
    }
}
