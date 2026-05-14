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

    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;

    public Spell(SpellCaster owner)
    {
        this.owner = owner;
    }

    //instructions said to have a method that gets passed the json data
    // then the spell can configure itself
    public virtual void SetAttributes(JObject attributes)
    {
        // we will parse the attributes here

    }

    public virtual string GetName()
    {
        return "Bolt";
    }

    // STATS THAT GET CHANGED BY MODIFIERS ---------------------------------------
    public virtual int GetManaCost()
    {
        float baseMana = 10f; // these base values should come from the JSON!!!
        // Apply all the modifiers to the base stat
        float modifiedMana = ValueModifier.Apply(baseMana, manaModifiers);
        return Mathf.RoundToInt(modifiedMana);
    }

    public virtual int GetDamage()
    {
        float baseDamage = 100f;// these base values should come from the JSON!!!
        float modifiedDamage = ValueModifier.Apply(baseDamage, damageModifiers);
        return Mathf.RoundToInt(modifiedDamage);
    }

    public virtual float GetCooldown()
    {
        float baseCoolDown = 0.75f;// these base values should come from the JSON!!!
        float modifiedCoolDown = ValueModifier.Apply(baseCoolDown, cooldownModifiers);
        return Mathf.RoundToInt(modifiedCoolDown);
    }
    // ------------------------------------------------------------------------------

    // If I am asked for the base spell, it's just me!
    public virtual Spell GetBaseSpell()
    {
        return this; 
    }

    public virtual int GetIcon()
    {
        return 0;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        yield return new WaitForEndOfFrame();
    }

    protected virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
        }

    }

}
