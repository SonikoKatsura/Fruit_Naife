using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
//using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour {
    public Transform planeDebug;
    public GameObject target;
    public Material crossSectionMaterial;

    void Start() {
        
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            Slice(target);
        }
    }

    public void Slice(GameObject target) {
        SlicedHull hull = target.Slice(planeDebug.position, planeDebug.up);

        if(hull != null) {
            //Create top and bottom slices with a 
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

            //Destroy original
            Destroy(target);
        }
    }
}
