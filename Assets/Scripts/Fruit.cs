using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
    [SerializeField] int amountOfPoints = 1; 

    public int GetAmountOfPoints() {
        return amountOfPoints;
    }
}
