using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public TextMeshProUGUI textTimer; // Referencia al objeto Texto en la UI

    private float tiempoPasado = 0f;
    private bool cronometroActivo = false;

    void Start() {
        // Comenzar el cronómetro automáticamente
        //StartTimer();
    }

    void Update() {
        if (cronometroActivo) {
            tiempoPasado += Time.deltaTime;
            UpdateTextTimer();
        }
    }

    public void StartTimer() {
        cronometroActivo = true;
    }

    public void StopTimer() {
        cronometroActivo = false;
    }

    public void ResetTimer() {
        tiempoPasado = 0f;
        UpdateTextTimer();
    }

    public float GetFloatTimer() {
        return tiempoPasado;
    }

    public string GetTransformTextTimer() {
        int minutos = Mathf.FloorToInt(tiempoPasado / 60f);
        int segundos = Mathf.FloorToInt(tiempoPasado % 60f);
        int milisegundos = Mathf.FloorToInt((tiempoPasado * 1000) % 1000) / 100; // Dividir por 100 para reducir el nº de digitos;

        // Formatear el tiempo en el formato MM:SS:MS
        string tiempoFormateado = string.Format("{0:0}:{1:00}:{2:0}", minutos, segundos, milisegundos);
        return tiempoFormateado;
    }

    private void UpdateTextTimer() {
        // Actualizar el texto en la UI
        textTimer.text = GetTransformTextTimer();
    }
}
