using UnityEngine;

public class AntController : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetWalking(bool walking)
    {
        anim.SetBool("IsWalking", walking);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void TakeDamage()
    {
        anim.SetTrigger("TakeDamage");
    }

    public void Stun()
    {
        anim.SetTrigger("Stun");
    }

    public void Die()
    {
        anim.SetBool("IsDead", true);
    }
}