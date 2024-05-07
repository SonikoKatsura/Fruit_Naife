using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceObject_Test : MonoBehaviour {
    public Transform planeDebug;
    public GameObject target;

    public Material crossSectionMaterial;
    public float cutForce = 2000;

    void Update() {
        if (Input.GetKeyDown("space")) {
            Slice(target);
        }
    }

    public void Slice(GameObject target) {
        SlicedHull hull = target.Slice(planeDebug.position, planeDebug.up);

        if(hull != null) {
            //Create top and bottom slices with a cut Material
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(lowerHull);

            //Destroy original
            Destroy(target);
        }
    }

    // Add force
    public void SetupSlicedComponent(GameObject slicedobject) {
        Rigidbody rb = slicedobject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedobject.AddComponent< MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedobject.transform.position, 1);
    }
}
