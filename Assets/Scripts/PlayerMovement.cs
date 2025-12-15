using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public int tileIndex = 0;
    public float moveSpeed = 3f;

    // Slotu pozīcijas uz viena tile (max 4 spēlētāji)
    public static readonly Vector3[] TileSlots =
    {
        new Vector3( 0.25f, 0f,  0.25f),
        new Vector3(-0.25f, 0f,  0.25f),
        new Vector3( 0.25f, 0f, -0.25f),
        new Vector3(-0.25f, 0f, -0.25f),
    };

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator MoveSteps(int steps, Transform[] tiles)
    {
        if (animator != null)
            animator.SetBool("walk", true);

        for (int i = 0; i < steps; i++)
        {
            tileIndex++;

    tileIndex = Mathf.Min(tileIndex, tiles.Length - 1);


            Vector3 targetPos = tiles[tileIndex].position;

            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            yield return new WaitForSeconds(0.15f);
        }

        if (animator != null)
            animator.SetBool("walk", false);
    }

    // Izsauc cīņas animāciju
    public void PlayFight()
    {
        animator?.SetTrigger("Fight");
    }

    // Drošībai
    public void ResetToIdle()
    {
        animator?.SetBool("walk", false);
    }
}
