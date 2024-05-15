using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTIÓN DE AUDIO (vinculado a un GameObject vacío "AudioManager")
// ---------------------------------------------------------------------------------
public class HapticManager : MonoBehaviour {

    // Instancia única del AudioManager (porque es una clase Singleton) STATIC
    public static HapticManager instance;

    // En vez de usar un vector de HapticClips (que podría ser) vamos a utilizar un Diccionario
    // en el que cargaremos directamente los recursos desde la jerarquía del proyecto
    // Cada entrada del diccionario tiene una string como clave y un HapticClip como valor
    public Dictionary<string, HapticClip> hapticsClips = new Dictionary<string, HapticClip>();

    // Método Awake que se llama al inicio antes de que se active el objeto. Útil para inicializar
    // variables u objetos que serán llamados por otros scripts (game managers, clases singleton, etc).
    private void Awake() {
        // ----------------------------------------------------------------
        // AQUÍ ES DONDE SE DEFINE EL COMPORTAMIENTO DE LA CLASE SINGLETON
        // Garantizamos que solo exista una instancia del AudioManager
        // Si no hay instancias previas se asigna la actual
        // Si hay instancias se destruye la nueva
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        // ----------------------------------------------------------------

        // No destruimos el AudioManager aunque se cambie de escena
        DontDestroyOnLoad(gameObject);

        // Cargamos los AudioClips en los diccionarios
        LoadHapticClips();

    }

    private void Start() {
        /* Comentar si se añaden más músicas - gestionarlo desde el GameManager */
        //PlayMusic("Title");  // Reproduce la música principal
    }

    // Método privado para cargar los efectos de sonido directamente desde las carpetas
    private void LoadHapticClips() {
        // Los recursos (ASSETS) que se cargan en TIEMPO DE EJECUCIÓN DEBEN ESTAR DENTRO de una carpeta denominada /Assets/Resources/SFX
        hapticsClips["Explosion"] = Resources.Load<HapticClip>("SFX/Explosion");
    }

    // Método de la clase singleton para reproducir efectos de sonido
    public void PlayHapticClip(string clipName) {
        if (hapticsClips.ContainsKey(clipName)) {
            // Metodo que crea un un nuevo audioSource, reproduce el sonido, espera a que termine y lo borra
            //PlaySoundAndDestroy(hapticsClips, clipName, false);
        }
        else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de hapticsClips.");
    }

    /*#region Create new AudioSource, Play Sound, Wait and Destroy
    private void PlaySoundAndDestroy(Dictionary<string, AudioClip> dictionaryClips, string clipName, bool loop) {
        //Debug.Log(clipName + ", " + loop);
        // Crear un nuevo AudioSource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        // Asignar el clip al nuevo AudioSource
        newAudioSource.clip = dictionaryClips[clipName];
        // Reproducir el sonido
        newAudioSource.Play();

        if (loop) {
            newAudioSource.loop = true;
        }
        else {
            //Corrutina que espere el tiempo del clip y lo borre
            float timeSound = newAudioSource.clip.length;

            // Autodestruir el AudioSource después de que termine de reproducir el sonido
            Destroy(newAudioSource, timeSound);
        }
    }
    #endregion

    public float GetSFXDuration(string clipName) {
        return hapticsClips[clipName].length;
    }*/
}