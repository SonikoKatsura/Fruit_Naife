using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class score_script : MonoBehaviour
{
    public GameObject ScoreText;
    public int score;

    private void OnTriggerEnter(Collider other)
    {
        score += 1;
        ScoreText.GetComponent<Text>().text = "SCORE: " + score;
    }

}
