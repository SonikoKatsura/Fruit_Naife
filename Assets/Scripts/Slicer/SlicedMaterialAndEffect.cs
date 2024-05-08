using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedMaterialAndEffect : MonoBehaviour {
    [SerializeField] Material slicedSectionMaterial;

    [SerializeField] ParticleSystem slicedParticles;

    void Start() {
        string name = this.gameObject.name;

        if (slicedSectionMaterial == null) {
            Debug.Log(name + " missing slicedSectionMaterial");
        }
        if (slicedParticles == null) {
            Debug.Log(name + " missing slicedParticles");
        }
    }

    public Material GetSlicedSectionMaterial() {
        if (slicedSectionMaterial == null) {
            return null;
        }
        else {
            return slicedSectionMaterial;
        }
    }

    public ParticleSystem GetSlicedParticles() {
        if (slicedParticles == null) {
            return null;
        }
        else {
            return slicedParticles;
        }
    }
}
