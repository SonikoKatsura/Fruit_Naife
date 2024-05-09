using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Unity.VisualScripting;

public class SliceObject : MonoBehaviour {

    [SerializeField] Transform startSlicePoint;
    [SerializeField] Transform endSlicePoint;
    [SerializeField] VelocityEstimator VelocityEstimator;

    [SerializeField] Material defaultCrossSectionMaterial;
    [SerializeField] ParticleSystem defaultCrossSectionParticles;
    [SerializeField] float cutForce = 500;

    [Header("Destroy Sliced Parts After Delay")]
    [SerializeField] bool destroyAfterDelay = true;
    [SerializeField] float startFadeOut = 3f;
    [SerializeField] float fadeOutDuration = 2f;

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

            // Destroy sliced parts
            if (destroyAfterDelay) {
                FadeOutAndDestroy(upperHull);
                FadeOutAndDestroy(lowerHull);
            }            
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

    #region FadeOut and destroy after delay
    private void FadeOutAndDestroy(GameObject obj) {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null) {
            Material[] materials = renderer.materials;
            foreach (Material material in materials) {
                StartCoroutine(FadeMaterial(material, fadeOutDuration));
            }
        }
        // Destroy after delay
        Destroy(obj, startFadeOut + fadeOutDuration);
    }

    private IEnumerator FadeMaterial(Material material, float duration) {
        // Waits before start fading
        yield return new WaitForSeconds(startFadeOut);

        Color color = material.color;
        float startAlpha = color.a;
        float startTime = Time.time;

        // Fade
        while (Time.time < startTime + duration) {
            float t = (Time.time - startTime) / duration;
            color.a = Mathf.Lerp(startAlpha, 0f, t);
            material.color = color;
            yield return null;
        }

        // Complete transparency at the end
        color.a = 0f;
        material.color = color;
    }
    #endregion

    #region Destroy after delay
    /*private void DestroAfterDelay(GameObject obj) {
        Destroy(obj, destroySlicedPartsDelay);
    }*/
    #endregion
}
