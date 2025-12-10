using UnityEngine;

public class SideDetectScript : MonoBehaviour
{
    private DiceRollScript dice;
    private Rigidbody rb;

    void Awake()
    {
        dice = FindFirstObjectByType<DiceRollScript>();

        if (dice != null)
            rb = dice.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (dice == null || rb == null)
            return;

        // checking if dice almost stopped
        if (rb.linearVelocity.sqrMagnitude < 0.01f)
        {
            dice.isLanded = true;
            dice.diceFaceNum = gameObject.name;

        }
        else
        {
            dice.isLanded = false;
        }
    }
}
