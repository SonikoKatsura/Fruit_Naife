using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTIÓN DE AUDIO (vinculado a un GameObject vacío "AudioManager")
// ---------------------------------------------------------------------------------
public class HapticManagerTest : MonoBehaviour {

    // Instancia única del AudioManager (porque es una clase Singleton) STATIC
    public static HapticManagerTest instance;

    private HapticClipPlayer player;
    public HapticClip clip;

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
    }

    void Start() {
        player = new HapticClipPlayer(clip);
    }

    public void PlayHapticClip() {
        player.Play(Controller.Both);
    }
    public void PlayHapticClip(bool rightHand) {
        if (rightHand)
            player.Play(Controller.Right);
        else
            player.Play(Controller.Left);
    }

    public void StopHaptics() {
        player.Stop();
    }

    private void OnDestroy() {
        player.Dispose();
    }

    private void OnApplicationQuit() {
        Haptics.Instance.Dispose();
    }
}