using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class LeaderboardUI : MonoBehaviour
{
    public Transform contentParent;
    public TextMeshProUGUI entryPrefab;
    

    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

  void Refresh()
{
    foreach (Transform child in contentParent)
        Destroy(child.gameObject);

    List<LeaderboardEntry> entries = LeaderboardManager.Load();

    Debug.Log($"LEADERBOARD ENTRIES: {entries.Count}");

    foreach (var e in entries)
    {
        Debug.Log($"ADD UI: {e.Name}");

        var item = Instantiate(entryPrefab, contentParent);
        item.text = "TEST TEXT";
item.color = Color.white;
item.fontSize = 36;

        item.gameObject.SetActive(true);

       item.text =
    $"{e.Name} | " +
    $"SCORE: {e.BestScore} | " +
    $"WINS: {e.Wins} | " +
    $"{e.LastWinDate:yyyy-MM-dd}";

    }
}

}
