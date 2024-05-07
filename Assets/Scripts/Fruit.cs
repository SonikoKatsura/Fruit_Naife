using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
    [SerializeField] int amountOfPoints = 1; 
    [SerializeField] Material crossSectionMaterial;

    void Start() {
        string name = this.gameObject.name;

        if (crossSectionMaterial == null) {
            Debug.Log(name + " missing crossSectionMaterial");
        }
    }

    public int GetAmountOfPoints() {
        return amountOfPoints;
    }

    public Material GetCrossSectionMaterial() {
        if (crossSectionMaterial == null) {
            return null;
        }
        else {
            return crossSectionMaterial;
        }
    }
}
