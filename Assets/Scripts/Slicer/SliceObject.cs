using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using static UnityEngine.GraphicsBuffer;

public class SliceObject : MonoBehaviour {

    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator VelocityEstimator;

    public Material defaultCrossSectionMaterial;
    public float cutForce = 1000;

    private Material crossSectionMaterial;

    /*void FixedUpdate() {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit) {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }*/

    private void Start() {
        crossSectionMaterial = defaultCrossSectionMaterial;
    }
    public void Slice(GameObject target) {
        // Normalized perpendicular vector between up and forward knife vector
        Vector3 velocity = VelocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        Debug.DrawRay(endSlicePoint.position - startSlicePoint.position, velocity, Color.green);
        planeNormal.Normalize();

        // Slice cut
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if(hull != null) {
            Debug.Log(hull.ToString());

            // Get & Set current fruit slice material
            //GetSetCrossSectionMaterial(target);

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
        Fruit Fruit = target.GetComponent<Fruit>(); // Obtener el componente Fruit del GameObject target
        if (Fruit != null) { // Verificar si el componente Fruit se encontró correctamente
                                   // Acceder al valor de crossSectionMaterial
            Material fruitSectionMaterial = Fruit.GetCrossSectionMaterial();
            Debug.Log(fruitSectionMaterial);

            if (fruitSectionMaterial != null) {
                crossSectionMaterial = fruitSectionMaterial;
            } else {
                crossSectionMaterial = defaultCrossSectionMaterial;
            }
        }
        else {
            Debug.LogWarning("El GameObject no tiene el componente Fruit adjunto.");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            if (other.gameObject.CompareTag("Fruit")) {
                Slice(other.gameObject);
            }
        }
    }
}
