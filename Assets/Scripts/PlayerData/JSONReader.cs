using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;


public class JSONReader : MonoBehaviour {
    [SerializeField] TMP_Text[] playerNames;
    [SerializeField] TMP_Text[] playerTimes;
    [SerializeField] TMP_Text[] playerScores;
    string dataFilePath = "ranking.json";

    void Start() {
        ReadData();
    }

    //public void SaveData() {
    //    List<PlayerData> playersList = new List<PlayerData>();
    //    playersList.Add(new PlayerData("Abby", 39));
    //    playersList.Add(new PlayerData("Bobby", 96));

    //    PlayerDataList playerDataList = new PlayerDataList();
    //    playerDataList.playerData = playersList;

    //    string jsonData = JsonUtility.ToJson(playerDataList, true);
    //    PlayerPrefs.SetString("PlayerList", jsonData);

    //    File.WriteAllText(dataFilePath, jsonData);
    //}

    public void ReadData() {
        if (File.Exists(dataFilePath)) {
            string jsonData = File.ReadAllText(dataFilePath);
            PlayerDataList playerDataList = JsonUtility.FromJson<PlayerDataList>(jsonData);
            SetData(playerDataList);
        }
    }

    public void SetData(PlayerDataList playerDataList) {
        playerDataList.playerData.Sort((x, y) => x.score.CompareTo(y.score));

        for(int i = 0; i < 3 && i < playerDataList.playerData.Count; i++) {
            playerNames[i].text = playerDataList.playerData[i].name;
            playerTimes[i].text = playerDataList.playerData[i].time;
            playerScores[i].text = playerDataList.playerData[i].score.ToString();
        }
    }
}