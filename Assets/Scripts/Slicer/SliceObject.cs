using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceObject : MonoBehaviour {

    [SerializeField] Transform startSlicePoint;
    [SerializeField] Transform endSlicePoint;
    [SerializeField] VelocityEstimator VelocityEstimator;

    [SerializeField] Material defaultCrossSectionMaterial;
    [SerializeField] ParticleSystem defaultCrossSectionParticles;
    [SerializeField] float cutForce = 500;

    private Material crossSectionMaterial;
    private ParticleSystem crossSectionParticles;

    private void Start() {
        crossSectionMaterial = defaultCrossSectionMaterial;
        crossSectionParticles = defaultCrossSectionParticles;
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
            GetSetCrossSectionMaterialAndEffect(target);

            //Create top and bottom slices with a cut Material
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(lowerHull);

            // PLay Particle system Effect
            Instantiate(crossSectionParticles, transform.position, Quaternion.identity);

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

    // Gets current object material and effect
    private void GetSetCrossSectionMaterialAndEffect(GameObject target) {
        SlicedMaterialAndEffect slicedMatEffect = target.GetComponent<SlicedMaterialAndEffect>();

        if (slicedMatEffect != null) {
            // Sliced Material
            Material currentSectionMaterial = slicedMatEffect.GetSlicedSectionMaterial();
            if (currentSectionMaterial != null)
                crossSectionMaterial = currentSectionMaterial;
            else
                crossSectionMaterial = defaultCrossSectionMaterial;

            // Sliced particles
            ParticleSystem currentSectionEffect = slicedMatEffect.GetSlicedParticles();
            if (currentSectionEffect != null)
                crossSectionParticles = currentSectionEffect;
            else
                crossSectionParticles = defaultCrossSectionParticles;
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
