using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Core")]
    public DiceRollScript dice;
    public Transform[] tiles;
    public CameraController cameraController;
    public CountdownUI countdownUI;

    [Header("Fight Audio")]
    public AudioSource fightAudioSource;
    public AudioClip fightClip;

    List<GameObject> playersList = new List<GameObject>();
    Dictionary<int, List<GameObject>> tileOccupants = new Dictionary<int, List<GameObject>>();

    int currentPlayer = 0;
    bool waitingForDice = true;
    bool fightInProgress = false;
    public bool introPlaying = true;

    // ==========================
    // PLAYER REGISTER
    // ==========================

    public void RegisterPlayer(GameObject player)
    {
        playersList.Add(player);

        if (!tileOccupants.ContainsKey(0))
            tileOccupants[0] = new List<GameObject>();

        tileOccupants[0].Add(player);
        ApplySlotPosition(player, 0);

        // Sākam intro tikai tad, kad visi jau ir spawn
        if (playersList.Count >= 2 && introPlaying)
        {
            StartCoroutine(GameIntroFlow());
        }
    }

    // ==========================
    // UPDATE
    // ==========================

    void Update()
    {
        if (playersList.Count == 0) return;
        if (introPlaying || fightInProgress) return;

        if (!waitingForDice && dice.isLanded)
        {
            waitingForDice = true;
            int steps = int.Parse(dice.diceFaceNum);
            StartCoroutine(MoveCurrentPlayer(steps));
        }
    }

    // ==========================
    // PLAYER MOVE
    // ==========================

    IEnumerator MoveCurrentPlayer(int steps)
    {
        GameObject player = playersList[currentPlayer];
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        cameraController?.FocusPlayer(player.transform);

        RemoveFromTile(player, pm.tileIndex);

        yield return StartCoroutine(pm.MoveSteps(steps, tiles));

        AddToTile(player, pm.tileIndex);
        ApplySlotPosition(player, pm.tileIndex);

        // CĪŅA, ja uz tile ir vairāki
        if (tileOccupants[pm.tileIndex].Count > 1)
        {
            yield return StartCoroutine(HandleFight(pm.tileIndex));
        }

        NextTurn();
    }

    // ==========================
    // TURN
    // ==========================

    void NextTurn()
    {
        currentPlayer++;
        if (currentPlayer >= playersList.Count)
            currentPlayer = 0;

        dice.ResetDice();
        waitingForDice = false;

        cameraController?.FocusDice(dice.transform);
    }

    // ==========================
    // FIGHT SYSTEM
    // ==========================

    IEnumerator HandleFight(int tileIndex)
    {
        fightInProgress = true;
        waitingForDice = true;

        List<GameObject> fighters = tileOccupants[tileIndex];
        if (fighters.Count < 2)
        {
            fightInProgress = false;
            yield break;
        }

        GameObject a = fighters[0];
        GameObject b = fighters[1];

        cameraController?.FocusPlayer(a.transform);

        // 🔊 skaņa
        if (fightAudioSource && fightClip)
            fightAudioSource.PlayOneShot(fightClip);

        // 🥊 animācijas
        a.GetComponent<PlayerMovement>()?.PlayFight();
        b.GetComponent<PlayerMovement>()?.PlayFight();

        yield return new WaitForSeconds(2.5f);

        // Random uzvarētājs
        GameObject winner = Random.value > 0.5f ? a : b;
        GameObject loser  = winner == a ? b : a;

        PlayerMovement loserPM = loser.GetComponent<PlayerMovement>();

        RemoveFromTile(loser, tileIndex);

        loserPM.tileIndex = Mathf.Max(0, loserPM.tileIndex - 1);

        AddToTile(loser, loserPM.tileIndex);
        ApplySlotPosition(loser, loserPM.tileIndex);

        ApplySlotPosition(winner, tileIndex);

        // reset animācijas
        a.GetComponent<PlayerMovement>()?.ResetToIdle();
        b.GetComponent<PlayerMovement>()?.ResetToIdle();

        yield return new WaitForSeconds(0.4f);

        fightInProgress = false;
    }

    // ==========================
    // TILE SLOT SYSTEM
    // ==========================

    void AddToTile(GameObject player, int tileIndex)
    {
        if (!tileOccupants.ContainsKey(tileIndex))
            tileOccupants[tileIndex] = new List<GameObject>();

        tileOccupants[tileIndex].Add(player);
    }

    void RemoveFromTile(GameObject player, int tileIndex)
    {
        if (!tileOccupants.ContainsKey(tileIndex)) return;
        tileOccupants[tileIndex].Remove(player);
    }

    void ApplySlotPosition(GameObject player, int tileIndex)
    {
        int slotIndex = tileOccupants[tileIndex].IndexOf(player);
        slotIndex = Mathf.Clamp(slotIndex, 0, PlayerMovement.TileSlots.Length - 1);

        Vector3 finalPos =
            tiles[tileIndex].position +
            PlayerMovement.TileSlots[slotIndex];

        player.transform.position = finalPos;
    }

    // ==========================
    // INTRO (SHOWCASE + 3..2..1)
    // ==========================

    IEnumerator GameIntroFlow()
    {
        introPlaying = true;
        waitingForDice = true;

        foreach (var player in playersList)
        {
            cameraController?.FocusPlayerCloseUp(player.transform);
            yield return new WaitForSeconds(2f);
        }

        cameraController?.FocusDice(dice.transform);
        yield return new WaitForSeconds(0.5f);

        if (countdownUI != null)
            yield return StartCoroutine(countdownUI.PlayCountdown());

        introPlaying = false;
        waitingForDice = false;
    }
}
