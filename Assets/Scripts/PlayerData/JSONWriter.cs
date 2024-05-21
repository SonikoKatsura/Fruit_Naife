using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;


public class JSONWriter : MonoBehaviour {
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TMP_Text playerTime;
    [SerializeField] TMP_Text playerScore;
    string dataFilePath = "ranking.json";

    void Start() {
        playerTime.text = DataManager.instance.timeText;
        playerScore.text = DataManager.instance.score.ToString();
    }

    public void SaveData() {
        PlayerDataList playerDataList = ReadData();

        if (playerDataList == null) playerDataList = new PlayerDataList();
        if (playerName.text == "") playerName.text = "Anonymous";
        playerDataList.playerData.Add(new PlayerData(playerName.text, DataManager.instance.timeText , DataManager.instance.score));

        string jsonData = JsonUtility.ToJson(playerDataList, true);
        PlayerPrefs.SetString("PlayerList", jsonData);

        File.WriteAllText(dataFilePath, jsonData);

    }

    PlayerDataList ReadData() {
        if (File.Exists(dataFilePath)) {
            string jsonData = File.ReadAllText(dataFilePath);
            PlayerDataList playerDataList = JsonUtility.FromJson<PlayerDataList>(jsonData);
            return playerDataList;
        }
        return null;
    }
}