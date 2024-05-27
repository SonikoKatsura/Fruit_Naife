using UnityEngine;
using TMPro;

public class Crono : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textCrono;

    [SerializeField] float colorChange = 5f;
    [SerializeField] private Color color = Color.red;

    private float tiempoRestante;

    private bool cronometroActivo = false;

    void Update() {
        if (cronometroActivo && tiempoRestante > 0) {
            if (text) text.gameObject.SetActive(true);   // Show Text
            textCrono.gameObject.SetActive(true);   // Show Crono

            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0) {
                tiempoRestante = 0;
                cronometroActivo = false;
                
                Debug.Log("¡Tiempo agotado!");
                if (text) text.gameObject.SetActive(false);   // Hide Text
                textCrono.gameObject.SetActive(false);   // Hide Crono
            }
            UpdateTextCrono();
        }
    }

    public string GetTransformTextCrono() {
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
        int milisegundos = Mathf.FloorToInt((tiempoRestante * 1000) % 1000) / 100; // Dividir por 100 para reducir el nº de digitos

        // Formatear el tiempo en el formato SS:MS
        string tiempoFormateado = string.Format("{0:00}:{1:0}", segundos, milisegundos);
        return tiempoFormateado;
    }

    private void UpdateTextCrono() {
        // Actualizar el texto en la UI
        if (textCrono != null) {
            /*if (tiempoRestante <= colorChange) {//00e600 cc0000
                textCrono.text = $"<color={color}>{GetTransformTextCrono()}";
            }
            else {
                textCrono.text = GetTransformTextCrono();
            }*/
            textCrono.text = GetTransformTextCrono();
            if (tiempoRestante <= colorChange)
                textCrono.color = color;
        }
    }

    public void StartCrono(float duration) {
        textCrono.color = Color.white;
        tiempoRestante = duration;
        cronometroActivo = true;
    }

    public void StopCrono() {
        cronometroActivo = false;
    }
}
