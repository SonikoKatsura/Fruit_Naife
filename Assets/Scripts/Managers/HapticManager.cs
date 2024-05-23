using Oculus.Haptics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTI�N DE AUDIO (vinculado a un GameObject vac�o "AudioManager")
// ---------------------------------------------------------------------------------
public class HapticManager : MonoBehaviour {
    private HapticClipPlayer player;
    private HapticClip clip;


    // Instancia �nica del AudioManager (porque es una clase Singleton) STATIC
    public static HapticManager instance;

    // En vez de usar un vector de HapticClips (que podr�a ser) vamos a utilizar un Diccionario
    // en el que cargaremos directamente los recursos desde la jerarqu�a del proyecto
    // Cada entrada del diccionario tiene una string como clave y un HapticClip como valor
    public Dictionary<string, HapticClip> hapticsClips = new Dictionary<string, HapticClip>();

    // M�todo Awake que se llama al inicio antes de que se active el objeto. �til para inicializar
    // variables u objetos que ser�n llamados por otros scripts (game managers, clases singleton, etc).
    private void Awake() {
        // ----------------------------------------------------------------
        // AQU� ES DONDE SE DEFINE EL COMPORTAMIENTO DE LA CLASE SINGLETON
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
        /* Comentar si se a�aden m�s m�sicas - gestionarlo desde el GameManager */
        //PlayMusic("Title");  // Reproduce la m�sica principal
    }

    // M�todo privado para cargar los efectos de sonido directamente desde las carpetas
    private void LoadHapticClips() {
        // Los recursos (ASSETS) que se cargan en TIEMPO DE EJECUCI�N DEBEN ESTAR DENTRO de una carpeta denominada /Assets/Resources/SFX
        hapticsClips["sword"] = Resources.Load<HapticClip>("Haptics/sword_hit");
        hapticsClips["explosion"] = Resources.Load<HapticClip>("Haptics/explosion");
        hapticsClips["explosion_16"] = Resources.Load<HapticClip>("Haptics/explosion_16");
        hapticsClips["explosion_2"] = Resources.Load<HapticClip>("Haptics/explosion_2");

        hapticsClips["click"] = Resources.Load<HapticClip>("Haptics/click");

        hapticsClips["Rumble1"] = Resources.Load<HapticClip>("Haptics/RumbleClip1");
        hapticsClips["Rumble2"] = Resources.Load<HapticClip>("Haptics/RumbleClip2");
    }

    // M�todo de la clase singleton para reproducir vibraciones
    public void PlayHapticClipBoth(string clipName) {
        if (hapticsClips.ContainsKey(clipName)) {
            // Obtiene el clip del diccionario
            clip = hapticsClips[clipName];

            // Asigna el clip al HapticClipPlayer
            player = new HapticClipPlayer(clip);

            // Play
            player.Play(Controller.Both);
        }
        else Debug.LogWarning("El HapticClip " + clipName + " no se encontr� en el diccionario de hapticsClips.");
    }
    public void PlayHapticClip(bool rightHand) {
        if (rightHand)
            player.Play(Controller.Right);
        else
            player.Play(Controller.Left);
    }
    public void PlayHapticClip(string clipName, bool rightHand) {
        if (hapticsClips.ContainsKey(clipName)) {
            // Obtiene el clip del diccionario
            clip = hapticsClips[clipName];

            // Asigna el clip al HapticClipPlayer
            player = new HapticClipPlayer(clip);

            if (rightHand)
                player.Play(Controller.Right);
            else
                player.Play(Controller.Left);
        }
        else Debug.LogWarning("El HapticClip " + clipName + " no se encontr� en el diccionario de hapticsClips.");
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