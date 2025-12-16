using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Globalization;

public static class LeaderboardManager
{
    static string FilePath =>
        Path.Combine(Application.persistentDataPath, "leaderboard.txt");

    // Formāts: Name|Wins|BestScore|LastWinDate(ISO)
    public static void AddWin(string playerName, int score)
    {
        Debug.Log("SAVING LEADERBOARD TO:");
Debug.Log(FilePath);

        var data = LoadInternal();

        if (!data.ContainsKey(playerName))
        {
            data[playerName] = new LeaderboardEntry
            {
                Name = playerName,
                Wins = 0,
                BestScore = 0,
                LastWinDate = DateTime.MinValue
            };
        }

        var entry = data[playerName];

        entry.Wins++;
        entry.LastWinDate = DateTime.UtcNow;

        if (score > entry.BestScore)
            entry.BestScore = score;

        SaveInternal(data);
    }

    public static List<LeaderboardEntry> Load()
    {
        var list = new List<LeaderboardEntry>(LoadInternal().Values);

        // KĀ LEADERBOARD, NEVIS KĀ RANDOM SARAKSTS
        list.Sort((a, b) =>
        {
            int scoreCompare = b.BestScore.CompareTo(a.BestScore);
            if (scoreCompare != 0) return scoreCompare;

            return b.LastWinDate.CompareTo(a.LastWinDate);
        });

        return list;
    }

    // ==========================
    // INTERNAL
    // ==========================

  static Dictionary<string, LeaderboardEntry> LoadInternal()
{
    Debug.Log("=== LOAD LEADERBOARD START ===");
    Debug.Log("PATH: " + FilePath);

    var result = new Dictionary<string, LeaderboardEntry>();

    if (!File.Exists(FilePath))
    {
        Debug.LogError("❌ FILE DOES NOT EXIST");
        return result;
    }

    string[] lines = File.ReadAllLines(FilePath);
    Debug.Log("LINES COUNT: " + lines.Length);

    foreach (string line in lines)
    {
        Debug.Log("RAW LINE: [" + line + "]");

        if (string.IsNullOrWhiteSpace(line))
        {
            Debug.Log("⏭ EMPTY LINE SKIPPED");
            continue;
        }

        string[] parts = line.Split('|');
        Debug.Log("PARTS COUNT: " + parts.Length);

        if (parts.Length != 4)
        {
            Debug.LogError("❌ WRONG FORMAT, EXPECTED 4 PARTS");
            continue;
        }

        if (!int.TryParse(parts[1], out int wins))
        {
            Debug.LogError("❌ WINS PARSE FAIL: " + parts[1]);
            continue;
        }

        if (!int.TryParse(parts[2], out int bestScore))
        {
            Debug.LogError("❌ SCORE PARSE FAIL: " + parts[2]);
            continue;
        }

        if (!DateTime.TryParseExact(
            parts[3],
            "O",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime date))
        {
            Debug.LogError("❌ DATE PARSE FAIL: " + parts[3]);
            continue;
        }

        Debug.Log("✅ ENTRY LOADED: " + parts[0]);

        result[parts[0]] = new LeaderboardEntry
        {
            Name = parts[0],
            Wins = wins,
            BestScore = bestScore,
            LastWinDate = date
        };
    }

    Debug.Log("=== LOAD FINISHED | ENTRIES: " + result.Count + " ===");
    return result;
}



    static void SaveInternal(Dictionary<string, LeaderboardEntry> data)
    {
        using StreamWriter sw = new StreamWriter(FilePath, false);

        foreach (var e in data.Values)
        {
            sw.WriteLine(
                $"{e.Name}|{e.Wins}|{e.BestScore}|{e.LastWinDate:O}"
            );
        }
    }

    public static void Clear()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
public class LeaderboardEntry
{
    public string Name;
    public int Wins;
    public int BestScore;
    public DateTime LastWinDate;
}

