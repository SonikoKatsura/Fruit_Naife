using Oculus.Haptics;
using Oculus.Interaction.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTI�N DE AUDIO (vinculado a un GameObject vac�o "AudioManager")
// ---------------------------------------------------------------------------------
public class HapticManagerTestWaitAndDestroy : MonoBehaviour {

    // Instancia �nica del AudioManager (porque es una clase Singleton) STATIC
    public static HapticManagerTestWaitAndDestroy instance;

    private HapticClipPlayer player;
    public HapticClip clip;

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

    // M�todo privado para cargar los efectos de sonido directamente desde las carpetas
    private void LoadHapticClips() {
        // Los recursos (ASSETS) que se cargan en TIEMPO DE EJECUCI�N DEBEN ESTAR DENTRO de una carpeta denominada /Assets/Resources/SFX
        hapticsClips["Rumble1"] = Resources.Load<HapticClip>("Haptics/RumbleClip1");
        hapticsClips["Rumble2"] = Resources.Load<HapticClip>("Haptics/RumbleClip2");
    }

    void Start() {
        //player = new HapticClipPlayer(clip);
    }

    public void PlayHapticClip() {
        player.Play(Controller.Both);
    }
    public void PlayHapticClip(bool rightHand) {
        if (rightHand)
            player.Play(Controller.Right);
        else
            player.Play(Controller.Left);
       
        //PlayHapticClipAndDestroy(rightHand);
    }

    public void StopHaptics() {
        player.Stop();
    }

    #region Create new AudioSource, Play Sound, Wait and Destroy
    /*private void PlayHapticClipAndDestroy(Dictionary<string, HapticClip> dictionaryClips, string clipName, bool rightHand) {
        //Debug.Log(clipName + ", " + loop);

        // Obtiene el nuevo HapticClip
        //HapticClip clip = dictionaryClips[clipName];*/
    public void PlayHapticClipAndDestroy(/*Dictionary<string, HapticClip> dictionaryClips, string clipName,*/ bool rightHand) {
        // Crear un nuevo HapticClipPlayer
        HapticClipPlayer hapticPlayer = new HapticClipPlayer(clip);

        // Reproducir la vibraci�n
        if (rightHand)
            hapticPlayer.Play(Controller.Right);
        else
            hapticPlayer.Play(Controller.Left);

        //Corrutina que espere el tiempo del clip y lo borre
        float timeClip = hapticPlayer.clipDuration;

        Debug.Log("Play");
        // Autodestruir el HapticClipPlayer despu�s de que termine de reproducir el Clip
        StartCoroutine(WaitAndDisposeHapticClipPlayer(hapticPlayer, timeClip));
    }

    private IEnumerator WaitAndDisposeHapticClipPlayer(HapticClipPlayer hapticPlayer, float timeClip) {
        Debug.Log("Start");
        yield return new WaitForSeconds(timeClip);
        Debug.Log("End");
        hapticPlayer.Stop();
        hapticPlayer.Dispose();
    }
    #endregion
 }