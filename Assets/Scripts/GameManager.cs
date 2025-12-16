using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Settings")]
public GameObject settingsCanvas;

    
    [Header("Pause")]
    public GameObject pauseCanvas;
    public GameObject leaderboardCanvas;
    bool isPaused = false;

    [Header("Music")]
    public AudioSource gameMusic;
    public AudioSource winMusic;

    [Header("Core")]
    public DiceRollScript dice;
    public Transform[] tiles;
    public CameraController cameraController;
    public CountdownUI countdownUI;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    float gameTimer;
    bool timerRunning;

    [Header("Win")]
    public GameObject winScreen;
    public TextMeshProUGUI winText;

    [Header("Fight Audio")]
    public AudioSource fightAudioSource;
    public AudioClip fightClip;

    List<GameObject> playersList = new();
    Dictionary<int, List<GameObject>> tileOccupants = new();

    int currentPlayer = 0;
    bool waitingForDice = true;
    bool fightInProgress = false;
    public bool introPlaying = true;
    public bool gameEnded = false;
    int diceRollCount = 0;


    // ==========================
    // PLAYER REGISTER
    // ==========================

    public void RegisterPlayer(GameObject player)
    {
        playersList.Add(player);

        if (!tileOccupants.ContainsKey(0))
            tileOccupants[0] = new List<GameObject>();

        tileOccupants[0].Add(player);
        ReapplySlotsForTile(0);

        if (playersList.Count >= 2 && introPlaying)
            StartCoroutine(GameIntroFlow());
    }

    // ==========================
    // UPDATE
    // ==========================

    void Update()
    {
        if (timerRunning)
        {
            gameTimer += Time.deltaTime;
            if (timerText != null)
                timerText.text = FormatTime(gameTimer);
        }

        if (gameEnded || introPlaying || fightInProgress) return;

        if (!waitingForDice && dice.isLanded)
        {
            waitingForDice = true;
            StartCoroutine(MoveCurrentPlayer(int.Parse(dice.diceFaceNum)));
        }
    }

    // ==========================
    // MOVE
    // ==========================

    IEnumerator MoveCurrentPlayer(int steps)
    {
        diceRollCount++;

        GameObject player = playersList[currentPlayer];
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        int lastTile = tiles.Length - 1;
        int direction = 1;

        RemoveFromTile(player, pm.tileIndex);
        ReapplySlotsForTile(pm.tileIndex);

        cameraController.FocusPlayer(player.transform);

        for (int i = 0; i < steps; i++)
        {
            if (pm.tileIndex == lastTile)
                direction = -1;

            pm.tileIndex = Mathf.Clamp(pm.tileIndex + direction, 0, lastTile);

            Vector3 target = tiles[pm.tileIndex].position;
            while (Vector3.Distance(player.transform.position, target) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    target,
                    pm.moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            yield return new WaitForSeconds(0.15f);
        }

        AddToTile(player, pm.tileIndex);
        ReapplySlotsForTile(pm.tileIndex);

        if (pm.tileIndex == lastTile)
        {
            StartCoroutine(WinFlow(player));
            yield break;
        }

        if (tileOccupants[pm.tileIndex].Count > 1)
            yield return StartCoroutine(HandleFight(pm.tileIndex, player));

        NextTurn();
    }

    // ==========================
    // TURN
    // ==========================

    void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % playersList.Count;
        dice.ResetDice();
        waitingForDice = false;
        cameraController.FocusDice(dice.transform);
    }

    // ==========================
    // FIGHT
    // ==========================

    IEnumerator HandleFight(int tileIndex, GameObject challenger)
    {
        fightInProgress = true;

        var occupants = tileOccupants[tileIndex];
        var opponents = new List<GameObject>(occupants);
        opponents.Remove(challenger);

        GameObject opponent = opponents[Random.Range(0, opponents.Count)];

        cameraController.FocusPlayer(challenger.transform);
        fightAudioSource?.PlayOneShot(fightClip);

        challenger.GetComponent<PlayerMovement>()?.PlayFight();
        opponent.GetComponent<PlayerMovement>()?.PlayFight();

        yield return new WaitForSeconds(2.5f);

        GameObject winner = Random.value > 0.5f ? challenger : opponent;
        GameObject loser  = winner == challenger ? opponent : challenger;

        PlayerMovement loserPM = loser.GetComponent<PlayerMovement>();

        RemoveFromTile(loser, tileIndex);
        ReapplySlotsForTile(tileIndex);

        loserPM.tileIndex = Mathf.Max(0, loserPM.tileIndex - 1);
        AddToTile(loser, loserPM.tileIndex);
        ReapplySlotsForTile(loserPM.tileIndex);

        challenger.GetComponent<PlayerMovement>()?.ResetToIdle();
        opponent.GetComponent<PlayerMovement>()?.ResetToIdle();

        yield return new WaitForSeconds(0.3f);
        fightInProgress = false;
    }

    // ==========================
    // INTRO FLOW (FIXED)
    // ==========================

    IEnumerator GameIntroFlow()
    {
        introPlaying = true;
        waitingForDice = true;

        foreach (var p in playersList)
        {
            cameraController.FocusPlayerCloseUp(p.transform);
            yield return new WaitForSeconds(2f);
        }

        // 🔥 ŠIS BIJA PAZUDIS
        cameraController.FocusDice(dice.transform);
        yield return new WaitForSeconds(0.5f);

        yield return countdownUI.PlayCountdown();

        introPlaying = false;
        waitingForDice = false;

        gameTimer = 0;
        timerRunning = true;
        gameMusic?.Play();
    }

    // ==========================
    // WIN FLOW (ZOOM + UI)
    // ==========================

 IEnumerator WinFlow(GameObject winner)
{
    gameEnded = true;
    timerRunning = false;
    dice.enabled = false;

    gameMusic?.Stop();
    winMusic?.Play();

    cameraController.FocusPlayerCloseUp(winner.transform);
    yield return new WaitForSeconds(1.5f);

    NameScript ns = winner.GetComponentInChildren<NameScript>();
    string playerName = ns != null ? ns.GetPlayerName() : "PLAYER";

    int score = CalculateScore();

    // ✅ VIENĪGAIS AddWin izsaukums
    LeaderboardManager.AddWin(playerName, score);

    winScreen.SetActive(true);

    winText.text =
        $"{playerName} WINS!\n" +
        $"TIME: {FormatTime(gameTimer)}\n" +
        $"ROLLS: {diceRollCount}\n" +
        $"SCORE: {score}";
}



    // ==========================
    // TILE HELPERS
    // ==========================

    void AddToTile(GameObject player, int tile)
    {
        if (!tileOccupants.ContainsKey(tile))
            tileOccupants[tile] = new List<GameObject>();

        tileOccupants[tile].Add(player);
    }

    void RemoveFromTile(GameObject player, int tile)
    {
        if (!tileOccupants.ContainsKey(tile)) return;
        tileOccupants[tile].Remove(player);
    }

    void ReapplySlotsForTile(int tile)
    {
        if (!tileOccupants.ContainsKey(tile)) return;

        var list = tileOccupants[tile];
        for (int i = 0; i < list.Count; i++)
        {
            int slot = Mathf.Clamp(i, 0, PlayerMovement.TileSlots.Length - 1);
            list[i].transform.position =
                tiles[tile].position + PlayerMovement.TileSlots[slot];
        }
    }

    // ==========================
    // UI
    // ==========================

    public void RestartGame() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void GoToMenu() =>
        SceneManager.LoadScene(0);


int CalculateScore()
{
    int baseScore = 10000;

    int rollPenalty = diceRollCount * 150;
    int timePenalty = Mathf.FloorToInt(gameTimer) * 5;

    int score = baseScore - rollPenalty - timePenalty;
    return Mathf.Max(score, 0);
}

    // ==========================
    // TIME
    // ==========================

    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60);
        int s = Mathf.FloorToInt(t % 60);
        int ms = Mathf.FloorToInt((t * 1000) % 1000);
        return $"{m:00}:{s:00}.{ms:000}";
    }
    public void TogglePause()
{
    if (gameEnded) return;

    if (isPaused)
        ResumeGame();
    else
        PauseGame();
}

public void PauseGame()
{
    isPaused = true;
    Time.timeScale = 0f;
    pauseCanvas.SetActive(true);


    gameMusic?.Pause();
}

public void ResumeGame()
{
    isPaused = false;
    Time.timeScale = 1f;

    if (pauseCanvas != null)
        pauseCanvas.SetActive(false);

    gameMusic?.UnPause();
}

public void GoToMenuFromPause()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(0);
}

public void OpenLeaderboard()
{
    if (leaderboardCanvas != null)
    {
        leaderboardCanvas.SetActive(true);

        var ui = leaderboardCanvas.GetComponent<LeaderboardUI>();
        if (ui != null)
            ui.Show();
        else
            Debug.LogError("❌ LeaderboardUI component missing on leaderboardCanvas");
    }

    if (pauseCanvas != null)
        pauseCanvas.SetActive(false);
}


public void CloseLeaderboard()
{
    if (leaderboardCanvas != null)
        leaderboardCanvas.SetActive(false);

    if (pauseCanvas != null)
        pauseCanvas.SetActive(true);
}

public void OpenSettings()
{
    settingsCanvas.SetActive(true);
    pauseCanvas.SetActive(false);
}

public void CloseSettings()
{
    settingsCanvas.SetActive(false);
    pauseCanvas.SetActive(true);
}



}
