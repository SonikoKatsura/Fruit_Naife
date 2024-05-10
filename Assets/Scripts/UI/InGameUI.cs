using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Naife;

public class InGameUI : MonoBehaviour {
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] TextMeshProUGUI livesTxt;

    void Start() {

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.Log("No hay GameManager");
    }

    void Update() {
        pointsTxt.text = gameManager.GetPoints().ToString();
        livesTxt.text = gameManager.GetLives().ToString();
    }
}
