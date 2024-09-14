using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DataManager : singleton<DataManager>
{
    public string currentPlayerId { get; set; }
    private List<HighscoreEntry> highscores = new List<HighscoreEntry>();
    private const int MaxHighscores = 5;
    private const string SaveFileName = "highscores.json";

    [System.Serializable]
    public class HighscoreEntry
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    private class SaveData
    {
        public List<HighscoreEntry> highscores;
    }

    public override void Awake()
    {
        base.Awake();
        LoadHighscores();
    }

    public void AddOrUpdateHighscore(string playerName, int score)
    {
        var existingEntry = highscores.FirstOrDefault(h => h.playerName == playerName);
        if (existingEntry != null)
        {
            if (score > existingEntry.score)
            {
                existingEntry.score = score;
            }
        }
        else
        {
            highscores.Add(new HighscoreEntry { playerName = playerName, score = score });
        }

        highscores = highscores.OrderByDescending(h => h.score).Take(MaxHighscores).ToList();
        SaveHighscores();
    }

    private void SaveHighscores()
    {
        SaveData saveData = new SaveData { highscores = highscores };
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSaveFilePath(), json);
    }

    private void LoadHighscores()
    {
        string path = GetSaveFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            highscores = saveData.highscores;
        }
    }

    private string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

    public string GetFormattedHighscores()
    {
        return string.Join("\n", highscores.Select((h, i) => $"{i + 1}. {h.playerName}: {h.score}"));
    }
}