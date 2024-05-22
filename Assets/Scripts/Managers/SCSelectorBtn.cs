using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCSelectorBtn : MonoBehaviour {
    // Método para cargar una nueva escena por nombre
    public void LoadScene(string sceneName) {
        SCManager.instance.LoadScene(sceneName);  // Carga la nueva escena y quita la anterior (lo mismo que LoadSceneMode.Single)
    }
    // Método para cargar una nueva escena por nombre Sin Quitar la actual
    public void LoadSceneAdditive(string sceneName) {
        SCManager.instance.LoadSceneAdditive(sceneName);    // Carga otra escena sin quitar la anterior
    }

    public void MainMenu() {
        Debug.Log("Menu");
        SCManager.instance.LoadScene("MainMenu");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");
    }

    public void PlayGame() {
        SCManager.instance.LoadScene("map");
        AudioManager.instance.PlayMusic("GameTheme");
        AudioManager.instance.PlaySFX("Button");
    }
    public void LoadRanking() {
        SCManager.instance.LoadScene("Ranking");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");
    }
    public void LoadConfig() {
        SCManager.instance.LoadScene("Settings");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");
    }
    public void LoadCredits() {
        SCManager.instance.LoadScene("Credits");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");
    }

    public void ResetGame() {
        SCManager.instance.LoadScene("map");
        AudioManager.instance.PlayMusic("GameTheme");
        AudioManager.instance.PlaySFX("Button");
    }
    public void ExitGame() {
        AudioManager.instance.PlaySFX("Button");
        SCManager.instance.ExitGame();
    }
}
