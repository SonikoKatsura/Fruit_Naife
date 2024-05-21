using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData {

    public string name;
    public string time;
    public int score;

    public PlayerData(string name, string time, int score)
    {
        this.name = name;
        this.time = time;
        this.score = score;

    }
}

[Serializable]
public class PlayerDataList
{
    public List<PlayerData> playerData;

    public PlayerDataList() {
        playerData = new List<PlayerData>();
    }
}
