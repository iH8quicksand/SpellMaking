using UnityEngine;

// Attach this to an invisible GameObject with a box collider to destroy spells that touch it
public class BoundaryDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ProjectileController>(out var projectile))
        {
            projectile.DestroyProjectile();
        }
    }
}
