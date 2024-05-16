using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Naife;

public class InGameUI : MonoBehaviour {
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI pointsTxt;

    [Header("Lives")]
    //[SerializeField] TextMeshProUGUI livesTxt;
    [SerializeField] GameObject Heart_1;
    [SerializeField] GameObject Heart_2;
    [SerializeField] GameObject Heart_3;

    void Start() {
        if (gameManager == null) {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
                Debug.Log("No hay GameManager");
        }
    }

    void Update() {
        pointsTxt.text = gameManager.GetPoints().ToString();
        HideShowHearts();
        //livesTxt.text = gameManager.GetLives().ToString();
    }

    private void HideShowHearts() {
        int lives = gameManager.GetLives();
        switch (lives) {
            case 3:
                Heart_1.SetActive(true);
                Heart_2.SetActive(true);
                Heart_3.SetActive(true);
                break;
            case 2:
                Heart_3.SetActive(false);
                break;
            case 1:
                Heart_2.SetActive(false);
                break;
            case 0:
                Heart_1.SetActive(false);
                break;
            default:
                // code block
                break;
        }
    }
}
