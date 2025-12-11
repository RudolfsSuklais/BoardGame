using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public int tileIndex = 0;
    public float moveSpeed = 3f;

    public IEnumerator MoveSteps(int steps, Transform[] tiles)
    {
        for (int i = 0; i < steps; i++)
        {
            tileIndex++;

            if (tileIndex >= tiles.Length)
                tileIndex = 0;

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

            yield return new WaitForSeconds(0.2f);
        }
    }
}
