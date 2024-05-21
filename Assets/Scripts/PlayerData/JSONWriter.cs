using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;


public class JSONWriter : MonoBehaviour {
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TMP_Text playerTime;
    string dataFilePath = "ranking.json";

    void Start() {
        playerTime.text = $"{PlayerPrefs.GetInt("score", 0)} secs.";
    }

    public void SaveData() {
        PlayerDataList playerDataList = ReadData();

        if (playerDataList == null) playerDataList = new PlayerDataList();
        if (playerName.text == "") playerName.text = "Anonymous";
        //playerDataList.playerData.Add(new PlayerData(playerName.text, PlayerPrefs.GetInt("score", 0)));

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