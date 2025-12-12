using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetWalking(bool value)
    {
        anim.SetBool("walk", value);
    }

    public void PlayFight()
    {
        anim.SetTrigger("Fight");
    }
}
