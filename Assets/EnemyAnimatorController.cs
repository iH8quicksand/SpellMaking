using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetWalking(bool walking)
    {
        if (anim != null)
            anim.SetBool("IsWalking", walking);
    }

    public void Attack()
    {
        if (anim != null)
            anim.SetTrigger("Attack");
    }

    public void TakeDamage()
    {
        if (anim != null)
            anim.SetTrigger("TakeDamage");
    }

    public void Die()
    {
        if (anim != null)
            anim.SetBool("IsDead", true);
    }
}