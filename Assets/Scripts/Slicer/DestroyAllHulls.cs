using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllHulls : MonoBehaviour {
    [Header("Destroy Sliced Parts After Delay")]
    [SerializeField] bool destroyAfterDelay = true;
    [SerializeField] float startFadeOut = 3f;
    [SerializeField] float fadeOutDuration = 2f;

    public void FadeOutHulls() {
        // Destruir todos los objetos llamados "Upper_Hull" en la escena
        DestroyObjectsWithName("Upper_Hull");

        // Destruir todos los objetos llamados "Lower_Hull" en la escena
        DestroyObjectsWithName("Lower_Hull");

        // Reinicia las vidas
        ResetLives();
    }

    private void DestroyObjectsWithName(string objectName) {
        #region tag
        /*// Buscar todos los objetos en la escena con el nombre especificado
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectName);

        // Destruir cada objeto encontrado
        foreach (GameObject obj in objects) {
            FadeOutAndDestroy(obj);
            //Destroy(obj);
        }*/
        #endregion

        #region Object name
        // Buscar y destruir todos los objetos con el nombre especificado
        GameObject[] objects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objects) {
            if (obj.name == objectName) {
                FadeOutAndDestroy(obj);
            }
        }
        #endregion
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

    private void ResetLives() {
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        gameManager.ResetLives();
    }
}
