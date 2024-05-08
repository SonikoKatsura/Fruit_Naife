using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedMaterial : MonoBehaviour {
    [SerializeField] Material crossSectionMaterial;

    void Start() {
        string name = this.gameObject.name;

        if (crossSectionMaterial == null) {
            Debug.Log(name + " missing crossSectionMaterial");
        }
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
