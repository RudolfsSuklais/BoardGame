using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public DiceRollScript dice;
    public Transform[] tiles;

    List<GameObject> playersList = new List<GameObject>();
    int currentPlayer = 0;
    bool waitingForDice = true;

    void Start()
    {
        waitingForDice = false; 
    }

    public void RegisterPlayer(GameObject player)
    {
        playersList.Add(player);
    }

    void Update()
    {
        if (playersList.Count == 0) return;

        if (!waitingForDice && dice.isLanded)
        {
            waitingForDice = true;

            int steps = int.Parse(dice.diceFaceNum);
            StartCoroutine(MoveCurrentPlayer(steps));
        }
    }

    IEnumerator MoveCurrentPlayer(int steps)
    {
        var pm = playersList[currentPlayer].GetComponent<PlayerMovement>();
        yield return StartCoroutine(pm.MoveSteps(steps, tiles));
        NextTurn();
    }

    void NextTurn()
    {
        currentPlayer++;

        if (currentPlayer >= playersList.Count)
            currentPlayer = 0;

        dice.ResetDice();
        waitingForDice = false;
    }
}
