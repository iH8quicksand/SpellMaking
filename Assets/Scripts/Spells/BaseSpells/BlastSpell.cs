using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Projectile;

/// <summary>
/// Base spell with specific behavior to explode on impact into smaller projectiles.
/// </summary>
public class BlastSpell : Spell
{
    public BlastSpell(SpellCaster owner) : base(owner) { }

    //N, secondary damage, secondary projectile
    private string N;
    private string secondaryDamage;
    private Projectile.TrajectoryMod secondaryTrajectory;
    private string secondarySpeed;
    private string secondaryLifetime;
    private int secondarySprite;
    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        if (attributes["N"] != null) N = attributes["N"].ToString();
        if (attributes["secondary_damage"] != null) secondaryDamage = attributes["secondary_damage"].ToString();
        if (attributes.SelectToken("secondary_projectile.trajectory") != null) secondaryTrajectory = GetTrajectoryFromJSON(attributes.SelectToken("secondary_projectile.trajectory"));
        if (attributes.SelectToken("secondary_projectile.speed") != null) secondarySpeed = attributes.SelectToken("secondary_projectile.speed").ToString();
        if (attributes.SelectToken("secondary_projectile.lifetime") != null) secondaryLifetime = attributes.SelectToken("secondary_projectile.lifetime").ToString();
        if (attributes.SelectToken("secondary_projectile.sprite") != null) secondarySprite = (int)attributes.SelectToken("secondary_projectile.sprite");
    }

    public int GetDamage2()
    {
        float modifiedDamage2 = ValueModifier.Apply(secondaryDamage, damageModifiers, GetRPNDict());
        return Mathf.RoundToInt(modifiedDamage2);
    }
    public List<Projectile.TrajectoryMod> GetTrajectory2()
    {
        List<Projectile.TrajectoryMod> trajectories = new(trajectoryModifiers);
        trajectories.Insert(0, secondaryTrajectory);
        return trajectories;
    }
    public float GetSpeed2()
    {
        float modifiedSpeed = ValueModifier.Apply(secondarySpeed, speedModifiers, GetRPNDict());
        return modifiedSpeed;
    }
    public int GetProjectileSprite2()
    {
        return secondarySprite;
    }
    public float GetLifetime2()
    {
        return RPNEvaluator.RPNEvaluator.Evaluatef(secondaryLifetime, GetRPNDict());
    }

    protected override void OnProjectileCollision(ProjectileController pc)
    {
        if (pc.collisions > impactModifiers.Count) pc.DestroyProjectile();
        else if (pc.collisions == 0)
        {
            int N = RPNEvaluator.RPNEvaluator.Evaluate(this.N, GetRPNDict());
            double angle;
            Vector3 target;
            for (int i = 0; i < N; i++)
            {
                angle = i * 360d / N * Math.PI/180d;
                target = new Vector3((float)(pc.transform.position.x + Math.Cos(angle)), pc.transform.position.y, (float)(pc.transform.position.z + Math.Sin(angle)));
                GameManager.Instance.projectileManager.CreateProjectile(GetProjectileSprite2(), GetTrajectory2(), pc.transform.position, target - pc.transform.position, GetSpeed2(), OnHit2, OnProjectileCollision, GetLifetime2(), collisions:1);
            }
            pc.DestroyProjectile();
        }
        else
        {
            switch (impactModifiers[pc.collisions-1])
            {
                case Projectile.ImpactMod.REVERSE:
                    pc.movement.speed *= -1;
                    break;
            }
        }
        pc.collisions++;
    }
    protected void OnHit2(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage2(), GetDamageType()));
        }
    }
}