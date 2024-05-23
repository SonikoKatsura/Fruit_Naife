using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour {

    public void LinkBrowser(string enlace) {
        Application.OpenURL(enlace);
        Debug.Log("Abriendo: " + enlace);
    }
}
