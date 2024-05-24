using System.Collections;
using UnityEngine;

// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTI�N DE ESCENAS (vinculado a un GameObject vac�o "SceneManager")
// ---------------------------------------------------------------------------------
using UnityEngine.SceneManagement; // Se incluye la librer�a para el manejo de escenas
// OJO: al incluir esta librer�a, no se podr� usar el nombre "SceneManager" porque
// ya hay una clase Static con dicho nombre. Por eso la clase se llama "SCManager"


public class SCManager : MonoBehaviour {
    // Creamos una variable est�tica para almacenar la �nica instancia
    public static SCManager instance;

    [SerializeField] float WaitNextScene = 1f; //Tiempo de espera antes de pasar a la siguiente escena

    // M�todo Awake que se llama al inicio antes de que se active el objeto. �til para inicializar
    // variables u objetos que ser�n llamados por otros scripts (game managers, clases singleton, etc).
    private void Awake() {

        // ----------------------------------------------------------------
        // AQU� ES DONDE SE DEFINE EL COMPORTAMIENTO DE LA CLASE SINGLETON
        // Garantizamos que solo exista una instancia del SCManager
        // Si no hay instancias previas se asigna la actual
        // Si hay instancias se destruye la nueva
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        // ----------------------------------------------------------------

        // No destruimos el SceneManager aunque se cambie de escena
        DontDestroyOnLoad(gameObject);

    }

    // M�todo para cargar una nueva escena por nombre
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);  // Carga la nueva escena y quita la anterior (lo mismo que LoadSceneMode.Single)
    }

    // M�todo para cargar una nueva escena por nombre Sin Quitar la actual
    public void LoadSceneAdditive(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);    // Carga otra escena sin quitar la anterior
    }

    // M�todo para Descargar la escena aditiva y volver a la de fondo
    public void UploadSceneAdditive(string sceneName) {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    // M�todo para cerrar el juego
    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
       
    }

    // M�todo para ir a la siguiente escena esperando un tiempo
    public void LoadSceneWaiting(string sceneName) {
        StartCoroutine(WaitToNextScene(sceneName));
        
    }
    IEnumerator WaitToNextScene(string sceneName) {
        yield return new WaitForSeconds(WaitNextScene);
        LoadScene(sceneName);
    }
}

// -----------------------------------------
// EJEMPLOS DE USO �DESDE CUALQUIER SCRIPT!
// -----------------------------------------
//SCManager.instance.LoadScene("MainTitle");
//SCManager.instance.LoadScene("EndGame");