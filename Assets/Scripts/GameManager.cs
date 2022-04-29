using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string playerName;

    public SerializableList<HighScore> highScores;

    private const int scoreCount = 10;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerName = "";
        LoadScores();
    }

    [System.Serializable]
    public class SerializableList<T>
    {
        public SerializableList()
        {
            list = new List<T>();
        }

        public List<T> list;
    }

    [System.Serializable]
    public class HighScore : IComparable
    {
        public HighScore(string _name, int _score)
        { 
            name = _name;
            score = _score;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            HighScore otherScore = obj as HighScore;
            if (otherScore != null)
                return this.score.CompareTo(otherScore.score);

            return 0;
        }

        public string name;
        public int score;
    }

    public void SubmitScore(int score)
    {
        HighScore scoreEntry = new HighScore(playerName, score);
        highScores.list.Add(scoreEntry);
        highScores.list.Sort();
        highScores.list.Reverse();

        if (highScores.list.Count > scoreCount)
        {
            highScores.list.RemoveRange(scoreCount, highScores.list.Count - scoreCount);
        }

        SaveScores();
    }

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(highScores);
        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/highscores.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            highScores = JsonUtility.FromJson<SerializableList<HighScore>>(json);
        }

        if (highScores == null)
        {
            highScores = new SerializableList<HighScore>();
        }

        while (highScores.list.Count < scoreCount)
        {
            highScores.list.Add(new HighScore("Empty", 0));
        }
    }
}
