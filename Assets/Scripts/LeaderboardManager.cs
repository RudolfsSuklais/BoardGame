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
        var result = new Dictionary<string, LeaderboardEntry>();

        if (!File.Exists(FilePath))
            return result;

        foreach (string line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split('|');
            if (parts.Length != 4) continue;

            if (!int.TryParse(parts[1], out int wins)) continue;
            if (!int.TryParse(parts[2], out int bestScore)) continue;

            if (!DateTime.TryParse(
                parts[3],
                null,
                DateTimeStyles.RoundtripKind,
                out DateTime date))
                continue;

            result[parts[0]] = new LeaderboardEntry
            {
                Name = parts[0],
                Wins = wins,
                BestScore = bestScore,
                LastWinDate = date
            };
        }

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

