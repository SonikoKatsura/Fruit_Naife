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
        //SCManager.instance.LoadScene("MainMenu");
        //AudioManager.instance.PlayMusic("MenuMusicLoop");
    }

    public void PlayGame() {
        SCManager.instance.LoadScene("Game");
        //AudioManager.instance.PlaySFX("start_level");
        //AudioManager.instance.PlayMusic("MusicLoop");
    }
    public void Ranking() {
        SCManager.instance.LoadScene("Ranking");
    }
    public void Config() {
        SCManager.instance.LoadScene("Settings");
    }
    public void Credits() {
        SCManager.instance.LoadScene("Credits");
    }

    public void ResetGame() {
        SCManager.instance.LoadScene("Level1");
        //AudioManager.instance.PlaySFX("start_level");
        //AudioManager.instance.PlayMusic("MusicLoop");
    }
    public void ExitGame() {
        SCManager.instance.ExitGame();
    }
}
