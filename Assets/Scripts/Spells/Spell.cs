using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Projectile;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// Represents a basic Spell. It stores a spell's Properties, allows setting properties from a <see cref="JObject"/>, stores and compiles all <see cref="ValueModifier"/>s Modifying it, handles being Cast, and handles Hitting an enemy.
/// </summary>
public class Spell
{
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;

    private string name = "Bolt";
    private string description = "Placeholder description";
    private int icon = 0;
    private Damage.Type damageType = Damage.Type.ARCANE;
    private string baseMana = "10f";
    private string baseDamage = "100f";
    private string baseCooldown = "0.75f";
    private string baseSpeed = "15f";
    private Projectile.TrajectoryMod trajectory = Projectile.TrajectoryMod.STRAIGHT;
    private int projectileSprite;
    private string lifetime = "0";

    // Create the lists to hold the modifiers; each base spell holds a list of all the modifiers applied to it.
    /// <summary>
    /// Stores all <see cref="ValueModifier"/>s modifying the Base <see cref="Spell"/>'s damage to later sequentially calculate the final value using <see cref="ValueModifier.Apply(string, List{ValueModifier}, Dictionary{string, int})"/>.
    /// </summary>
    public List<ValueModifier> damageModifiers = new();
    /// <summary>
    /// List of <see cref="ValueModifier"/>s modifying the <see cref="Spell"/>'s base mana cost.
    /// </summary>
    public List<ValueModifier> manaModifiers = new();
    /// <summary>
    /// List of <see cref="ValueModifier"/>s modifying the <see cref="Spell"/>'s base cooldown.
    /// </summary>
    public List<ValueModifier> cooldownModifiers = new();
    /// <summary>
    /// List of <see cref="ValueModifier"/>s modifying the <see cref="Spell"/>'s base projectile speed.
    /// </summary>
    public List<ValueModifier> speedModifiers = new();
    /// <summary>
    /// List of <see cref="ValueModifier"/>s modifying the <see cref="Spell"/>'s base projectile lifetime.
    /// </summary>
    public List<ValueModifier> lifetimeModifiers = new();
    /// <summary>
    /// List of <see cref="Projectile.TrajectoryMod"/>s representing trajectories to apply to the <see cref="Spell"/> when it's Cast.
    /// </summary>
    public List<Projectile.TrajectoryMod> trajectoryModifiers = new();
    /// <summary>
    /// List of <see cref="Projectile.ImpactMod"/>s representing behaviors to exhibit when the <see cref="ProjectileController"/> collides with something.
    /// </summary>
    public List<Projectile.ImpactMod> impactModifiers = new();

    /// <summary>
    /// Creates a new empty <see cref="Spell"/> able to reference the <see cref="SpellCaster"/>
    /// </summary>
    /// <param name="owner">The <see cref="SpellCaster"/> object used for calculating values based on <see cref="SpellCaster.spell_power"/> and special Cast behavior using <see cref="SpellCaster.transform"/> in certain <see cref="ModifierSpell"/>s.</param>
    public Spell(SpellCaster owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// Set the Base <see cref="Spell"/>'s attributes based on a <see cref="JObject"/> pulled from the spells.json.
    /// </summary>
    /// <param name="attributes">One specific <see cref="Spell"/>'s attributes from the JSON file.</param>
    public virtual void SetAttributes(JObject attributes)
    {
        if (attributes["name"] != null) name = attributes["name"].ToString();
        if (attributes["description"] != null) description = attributes["description"].ToString();
        if (attributes["icon"] != null) icon = (int)attributes["icon"];
        if (attributes.SelectToken("damage.type") != null)
        {
            switch(attributes.SelectToken("damage.type").ToString())
            {//PHYSICAL, ARCANE, NATURE, FIRE, ICE, DARK, LIGHT, EMOTIONAL
                case "physical":
                    damageType = Damage.Type.PHYSICAL; break;
                case "arcane":
                    damageType = Damage.Type.ARCANE; break;
                case "nature":
                    damageType = Damage.Type.NATURE; break;
                case "fire":
                    damageType = Damage.Type.FIRE; break;
                case "ice":
                    damageType = Damage.Type.ICE; break;
                case "dark":
                    damageType = Damage.Type.DARK; break;
                case "light":
                    damageType = Damage.Type.LIGHT; break;
                case "emotional":
                    damageType = Damage.Type.EMOTIONAL; break;
            }
        }
        if (attributes["mana_cost"] != null) baseMana = attributes["mana_cost"].ToString();
        if (attributes.SelectToken("damage.amount") != null) baseDamage = attributes.SelectToken("damage.amount").ToString();
        if (attributes["cooldown"] != null) baseCooldown = attributes["cooldown"].ToString();
        if (attributes.SelectToken("projectile.speed") != null) baseSpeed = attributes.SelectToken("projectile.speed").ToString();
        if (attributes.SelectToken("projectile.trajectory") != null) trajectory = GetTrajectoryFromJSON(attributes.SelectToken("projectile.trajectory"));
        if (attributes.SelectToken("projectile.sprite") != null) projectileSprite = (int)attributes.SelectToken("projectile.sprite");
        if (attributes.SelectToken("projectile.lifetime") != null) lifetime = attributes.SelectToken("projectile.lifetime").ToString();
    }

    // If I am asked for the base spell, it's just me!
    public virtual Spell GetBaseSpell()
    {
        return this;
    }

    public virtual string GetName()
    {
        return name;
    }
    public Dictionary<string,int> GetRPNDict()
    {
        return new Dictionary<string, int> { { "wave", GameManager.Instance.GetWave() }, { "power", owner.spell_power} };
    }

    // STATS THAT GET CHANGED BY MODIFIERS ---------------------------------------
    public virtual int GetManaCost()
    {
        // Apply all the modifiers to the base stat
        float modifiedMana = ValueModifier.Apply(baseMana, manaModifiers, GetRPNDict());
        return Mathf.RoundToInt(modifiedMana);
    }

    public virtual int GetDamage()
    {
        float modifiedDamage = ValueModifier.Apply(baseDamage, damageModifiers, GetRPNDict());
        return Mathf.RoundToInt(modifiedDamage);
    }

    public virtual float GetCooldown()
    {
        float modifiedCoolDown = ValueModifier.Apply(baseCooldown, cooldownModifiers, GetRPNDict());
        return Mathf.RoundToInt(modifiedCoolDown);
    }

    public virtual float GetSpeed()
    {
        float modifiedSpeed = ValueModifier.Apply(baseSpeed, speedModifiers, GetRPNDict());
        return modifiedSpeed;
    }

    public virtual float GetLifetime()
    {
        float modifiedLifetime = ValueModifier.Apply(lifetime, lifetimeModifiers, GetRPNDict());
        return modifiedLifetime;
    }
    // ------------------------------------------------------------------------------

    public virtual string GetDescription()
    {
        return name + ": " + description;
    }
    public virtual int GetIcon()
    {
        return icon;
    }
    public virtual Damage.Type GetDamageType()
    {
        return damageType;
    }
    public virtual List<Projectile.TrajectoryMod> GetTrajectory()
    {
        List<Projectile.TrajectoryMod> trajectories = new(trajectoryModifiers);
        trajectories.Insert(0, trajectory);
        return trajectories;
    }
    public virtual int GetProjectileSprite()
    {
        return projectileSprite;
    }

    public virtual bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }
    
    public virtual float GetLastCast()
    {
        return last_cast;
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        EventBus.Instance.Broadcast_OnCastSpell();
        last_cast = Time.time;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(GetProjectileSprite(), GetTrajectory(), where, target - where, GetSpeed(), OnHit, OnProjectileCollision);
        yield return new WaitForEndOfFrame();
    }

    protected virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), GetDamageType()));
        }

    }

    protected virtual void OnProjectileCollision(ProjectileController pc)
    {
        if (pc.collisions >= impactModifiers.Count)
        {
            pc.DestroyProjectile();
            return;
        }
        else
        {
            switch (impactModifiers[pc.collisions])
            {
                case Projectile.ImpactMod.REVERSE:
                    pc.movement.speed *= -1;
                    break;
            }
        }
        pc.collisions++;
    }

    public Projectile.TrajectoryMod GetTrajectoryFromJSON(JToken trajectory)
    {
        return trajectory.ToString() switch
        {
            "homing" => Projectile.TrajectoryMod.HOMING,
            "spiraling" => Projectile.TrajectoryMod.SPIRALING,
            _ => Projectile.TrajectoryMod.STRAIGHT,
        };
    }
    public Projectile.ImpactMod GetProjectileImpactFromJSON(JToken impact)
    {
        return impact.ToString() switch
        {
            "reverse" => Projectile.ImpactMod.REVERSE,
            "bounce" => Projectile.ImpactMod.BOUNCE,
            _ => Projectile.ImpactMod.DIE,
        };
    }
}
