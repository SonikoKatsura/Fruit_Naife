using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
//using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour {

    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator VelocityEstimator;
    public LayerMask sliceableLayer;

    public Material crossSectionMaterial;
    public float cutForce = 1000;

    void FixedUpdate() {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit) {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target) {
        // Normalized perpendicular vector between up and forward knife vector
        Vector3 velocity = VelocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        // Slice cut
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if(hull != null) {
            //Create top and bottom slices with a 
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
