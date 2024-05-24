using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCSelectorBtn : MonoBehaviour {
    //EVENTO (DELEGADO)   --> Hit Barrel
    public delegate void RestartGame();
    public static event RestartGame OnRestartGame;    //(EVENTO)

    // M�todo para cargar una nueva escena por nombre
    public void LoadScene(string sceneName) {
        SCManager.instance.LoadScene(sceneName);  // Carga la nueva escena y quita la anterior (lo mismo que LoadSceneMode.Single)

        PlayHapticBoth();   // Vibration
    }
    // M�todo para cargar una nueva escena por nombre Sin Quitar la actual
    public void LoadSceneAdditive(string sceneName) {
        SCManager.instance.LoadSceneAdditive(sceneName);    // Carga otra escena sin quitar la anterior

        PlayHapticBoth();   // Vibration
    }

    public void MainMenu() {
        Debug.Log("Menu");
        SCManager.instance.LoadScene("MainMenu");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }

    public void PlayGame() {
        SCManager.instance.LoadScene("Game");
        AudioManager.instance.PlayMusic("GameTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }
    public void Playground() {
        SCManager.instance.LoadScene("Playground");
        AudioManager.instance.PlayMusic("GameTheme");
        AudioManager.instance.PlaySFX("Button");
        
        PlayHapticBoth();   // Vibration
    }
    public void LoadIntro() {
        SCManager.instance.LoadScene("Intro");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }
    public void LoadRanking() {
        SCManager.instance.LoadScene("Ranking");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }
    public void LoadNewScore() {
        SCManager.instance.LoadScene("NewScore");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }
    public void LoadConfig() {
        SCManager.instance.LoadScene("Settings");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }
    public void LoadCredits() {
        SCManager.instance.LoadScene("Credits");
        AudioManager.instance.PlayMusic("MainTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration
    }

    public void ResetGame() {
        SCManager.instance.LoadScene("Game");
        AudioManager.instance.PlayMusic("GameTheme");
        AudioManager.instance.PlaySFX("Button");

        PlayHapticBoth();   // Vibration

        // Event Restart Game
        if (OnRestartGame != null)
            OnRestartGame();
    }
    public void ExitGame() {
        AudioManager.instance.PlaySFX("Button");
        SCManager.instance.ExitGame();

        PlayHapticBoth();   // Vibration
    }

    private void PlayHapticBoth() {
        // Vibration
        HapticManager.instance.PlayHapticClipBoth("click");

        //PlayHapticBoth();   // Vibration
    }
}
