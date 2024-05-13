using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cronometro : MonoBehaviour {
    public TextMeshProUGUI textCrono; // Referencia al objeto Texto en la UI

    private float tiempoPasado = 0f;
    private bool cronometroActivo = false;

    void Start() {
        // Comenzar el cronómetro automáticamente
        //StartCrono();
    }

    void Update() {
        if (cronometroActivo) {
            tiempoPasado += Time.deltaTime;
            UpdateTextCrono();
        }
    }

    public void StartCrono() {
        cronometroActivo = true;
    }

    public void StopCrono() {
        cronometroActivo = false;
    }

    public void ResetCrono() {
        tiempoPasado = 0f;
        UpdateTextCrono();
    }

    public float GetFloatCrono() {
        return tiempoPasado;
    }

    public string GetTransformTextCrono() {
        int minutos = Mathf.FloorToInt(tiempoPasado / 60f);
        int segundos = Mathf.FloorToInt(tiempoPasado % 60f);
        int milisegundos = Mathf.FloorToInt((tiempoPasado * 1000) % 1000) / 100; // Dividir por 100 para reducir el nº de digitos;

        // Formatear el tiempo en el formato MM:SS:MS
        string tiempoFormateado = string.Format("{0:0}:{1:00}:{2:0}", minutos, segundos, milisegundos);
        return tiempoFormateado;
    }

    private void UpdateTextCrono() {
        // Actualizar el texto en la UI
        textCrono.text = GetTransformTextCrono();
    }
}
