using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using Unity.VisualScripting;

public class Spell
{
    // Create the lists to hold the modifiers
    // each base spell holds a list of all the modifiers applied to it
    public List<ValueModifier> damageModifiers = new List<ValueModifier>();
    public List<ValueModifier> manaModifiers = new List<ValueModifier>();
    public List<ValueModifier> cooldownModifiers = new List<ValueModifier>();
    public List<ValueModifier> speedModifiers = new List<ValueModifier>();

    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;

    public string name { get; set; }
    public string description { get; set; }
    public int icon {  get; set; }
    public string N {  get; set; }
    public string spray { get; set; }
    public SpellDamage damage { get; set; }
    public string manaCost { get; set; }
    public string cooldown {  get; set; }
    public Projectile projectile { get; set; }
    public Projectile secondary_projectile { get; set; }

    public Spell(SpellCaster owner)
    {
        this.owner = owner;
    }

    //instructions said to have a method that gets passed the json data
    // then the spell can configure itself.
    // THIS IS FOR BASE SPELLS
    public virtual void SetAttributes(JObject attributes)
    {

    }

    public virtual string GetName()
    {
        return name ?? "Bolt";
    }

    private Dictionary<string, int> getRPNDict()
    {
        return new Dictionary<string, int> { { "power", owner.spell_power }, { "wave", GameManager.Instance.GetWave() } };
    }

    // STATS THAT GET CHANGED BY MODIFIERS ---------------------------------------
    public virtual int GetManaCost()
    {
        float baseMana = RPNEvaluator.RPNEvaluator.Evaluatef(manaCost ?? "10", getRPNDict());
        // Apply all the modifiers to the base stat
        float modifiedMana = ValueModifier.Apply(baseMana, manaModifiers);
        return Mathf.RoundToInt(modifiedMana);
    }

    public virtual int GetDamage()
    {
        float baseDamage = RPNEvaluator.RPNEvaluator.Evaluatef(damage.amount ?? "100", getRPNDict());
        float modifiedDamage = ValueModifier.Apply(baseDamage, damageModifiers);
        return Mathf.RoundToInt(modifiedDamage);
    }

    public virtual float GetCooldown()
    {
        float baseCoolDown = RPNEvaluator.RPNEvaluator.Evaluatef(cooldown ?? "0.75", getRPNDict());
        float modifiedCoolDown = ValueModifier.Apply(baseCoolDown, cooldownModifiers);
        return Mathf.RoundToInt(modifiedCoolDown);
    }

    public virtual float GetSpeed(int projectileNumber)
    {
        Projectile p = (projectileNumber == 1) ? projectile : secondary_projectile;
        if (p == null) return 0;
        float baseSpeed = RPNEvaluator.RPNEvaluator.Evaluatef(p.speed ?? "15", getRPNDict());
        float modifiedSpeed = ValueModifier.Apply(baseSpeed, speedModifiers);
        return modifiedSpeed;
    }
    // ------------------------------------------------------------------------------

    // If I am asked for the base spell, it's just me!
    public virtual Spell GetBaseSpell()
    {
        return this; 
    }

    public virtual int GetIcon()
    {
        return icon;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        EventBus.Instance.Broadcast_OnCastSpell();
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, GetTrajectory(1), where, target - where, GetSpeed(1), OnHit);
        if (secondary_projectile != null) GameManager.Instance.projectileManager.CreateProjectile(0, GetTrajectory(2), where, target - where, GetSpeed(2), OnHit);
        yield return new WaitForEndOfFrame();
    }

    public virtual string GetTrajectory(int projectileNumber)
    {
        Projectile p = (projectileNumber == 1) ? projectile : secondary_projectile;
        if (p == null) return null;
        string trajectory = p.trajectory;
        return "straight";
    }

    protected virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
        }

    }

}
