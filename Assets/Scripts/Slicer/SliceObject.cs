using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceObject : MonoBehaviour {

    [SerializeField] Transform startSlicePoint;
    [SerializeField] Transform endSlicePoint;
    [SerializeField] VelocityEstimator VelocityEstimator;

    [SerializeField] Material defaultCrossSectionMaterial;
    [SerializeField] float cutForce = 500;

    private Material crossSectionMaterial;

    private void Start() {
        crossSectionMaterial = defaultCrossSectionMaterial;
    }

    public void Slice(GameObject target) {
        // Normalized perpendicular vector between up and forward knife vector
        Vector3 velocity = VelocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        // Slice cut
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if(hull != null) {
            // Get & Set current fruit slice material
            GetSetCrossSectionMaterial(target);

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

    // Gets current object mateial
    private void GetSetCrossSectionMaterial(GameObject target) {
        SlicedMaterial slicedMaterial = target.GetComponent<SlicedMaterial>();

        if (slicedMaterial != null) {
            Material fruitSectionMaterial = slicedMaterial.GetCrossSectionMaterial();

            if (fruitSectionMaterial != null) {
                crossSectionMaterial = fruitSectionMaterial;
            } else {
                crossSectionMaterial = defaultCrossSectionMaterial;
            }
        }
        else {
            Debug.LogWarning("Missing SlicedMaterial component");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.gameObject.CompareTag("Fruit") || other.gameObject.CompareTag("Barrel")) {
                Slice(other.gameObject);
            }
            Debug.Log(other.gameObject.name);
        }
    }
}
