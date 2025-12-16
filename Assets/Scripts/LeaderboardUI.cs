using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject entryPrefab;

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
    Debug.Log("=== LEADERBOARD UI REFRESH ===");

    if (contentParent == null)
    {
        Debug.LogError("❌ contentParent is NULL");
        return;
    }

    if (entryPrefab == null)
    {
        Debug.LogError("❌ entryPrefab is NULL");
        return;
    }

    foreach (Transform child in contentParent)
        Destroy(child.gameObject);

    List<LeaderboardEntry> entries = LeaderboardManager.Load();

    Debug.Log("ENTRIES RECEIVED IN UI: " + entries.Count);

    foreach (var e in entries)
    {
        Debug.Log("UI ADD ENTRY: " + e.Name);

        GameObject go = Instantiate(entryPrefab, contentParent);
        go.SetActive(true);

        var text = go.GetComponentInChildren<TextMeshProUGUI>(true);

        if (text == null)
        {
            Debug.LogError("❌ NO TextMeshProUGUI FOUND IN PREFAB");
            continue;
        }

        text.text =
            $"{e.Name} | " +
            $"Score: {e.BestScore} | " +
            $"Wins: {e.Wins} | " +
            $"{e.LastWinDate:yyyy-MM-dd}" ;
    }

    Debug.Log("=== UI REFRESH DONE ===");
}


}
