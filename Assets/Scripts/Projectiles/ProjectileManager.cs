using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectiles;//see data in Unity inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.projectileManager = this;
    }

    public void CreateProjectile(int which, List<Projectile.TrajectoryMod> trajectories, Vector3 where, Vector3 direction, float speed, Action<Hittable, Vector3> onHit, Action<ProjectileController> onProjectileCollision, float lifetime = 0, int collisions = 0)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectories, speed);
        new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
        new_projectile.GetComponent<ProjectileController>().OnProjectileCollision += onProjectileCollision;
        new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime);
        new_projectile.GetComponent<ProjectileController>().collisions = collisions;
    }

    public Trajectory MakeMovement(List<Projectile.TrajectoryMod> trajectories, float speed)
    {
        if (trajectories.Contains(Projectile.TrajectoryMod.HOMING) && trajectories.Contains(Projectile.TrajectoryMod.SPIRALING))
            return new SpiralingHomingTrajectory(speed);
        if (trajectories.Contains(Projectile.TrajectoryMod.HOMING))
            return new HomingTrajectory(speed);
        if (trajectories.Contains(Projectile.TrajectoryMod.SPIRALING))
            return new SpiralingTrajectory(speed);
        if (trajectories.Contains(Projectile.TrajectoryMod.STRAIGHT))
            return new StraightTrajectory(speed);
        return null;
    }

}
