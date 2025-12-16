using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject leaderboardPanel;

    public void OpenLeaderboard()
    {
        leaderboardPanel.SetActive(true);

        var ui = leaderboardPanel.GetComponent<LeaderboardUI>();
        if (ui != null)
            ui.Show();
        else
            Debug.LogError("❌ LeaderboardUI missing on LeaderboardPanel");

        mainMenuPanel.SetActive(false);
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
