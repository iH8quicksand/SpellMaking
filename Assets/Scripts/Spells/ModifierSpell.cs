using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

public class ModifierSpell : Spell
{
    protected Spell innerSpell;
    /// <summary>Represents the Modifier Spell's name, which becomes a prefix to the Base Spell's name.</summary>
    private string prefix;
    private string description;

    // I'm just a wrapper, so ask the doll inside me!
    public override Spell GetBaseSpell()
    {
        return innerSpell.GetBaseSpell();
    }

    // The constructor takes the spell it is wrapping
    public ModifierSpell(SpellCaster owner, Spell innerSpell) : base(owner)
    {
        this.innerSpell = innerSpell;
    }

    /// <summary>
    /// Recursively chains together the names of all <see cref="ModifierSpell"/>s to the Base <see cref="Spell"/>'s.
    /// </summary>
    /// <returns><see cref="string"/> representing the completely (or partially if called via recursion) compiled name of the <see cref="Spell"/>.</returns>
    public override string GetName() => prefix + " " + innerSpell.GetName();
    public override string GetDescription() => prefix + ": " + description + "\n" + innerSpell.GetDescription();
    public override int GetManaCost() => innerSpell.GetManaCost();
    public override int GetDamage() => innerSpell.GetDamage();
    public override float GetCooldown() => innerSpell.GetCooldown();
    public override int GetIcon() => innerSpell.GetIcon();
    public override float GetSpeed() => innerSpell.GetSpeed();
    public override List<Projectile.TrajectoryMod> GetTrajectory() => innerSpell.GetTrajectory();
    public override bool IsReady() => innerSpell.IsReady();
    public override Damage.Type GetDamageType() => innerSpell.GetDamageType();
    public override float GetLastCast() => innerSpell.GetLastCast();

    // Pass the cast down to the inner spell by default
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        yield return innerSpell.Cast(where, target, team);
    }

    /// <summary>
    /// Set the <see cref="ModifierSpell"/>'s attributes based on a <see cref="JObject"/> pulled from the spells.json.
    /// </summary>
    /// <param name="attributes">One specific <see cref="ModifierSpell"/>'s attributes from the JSON file.</param>
    public override void SetAttributes(JObject attributes)
    {
        if (attributes["name"] != null) prefix = attributes["name"].ToString();
        if (attributes["description"] != null) description = attributes["description"].ToString();
        if (attributes["projectile_trajectory"] != null) innerSpell.trajectoryModifiers.Add(GetTrajectoryFromJSON(attributes["projectile_trajectory"]));
        if (attributes["projectile_impact"] != null) innerSpell.impactModifiers.Add(GetProjectileImpactFromJSON(attributes["projectile_impact"]));
        string[] properties = { "damage", "mana", "speed", "cooldown" };
        foreach (string property in properties)
        {
            if (attributes[property] != null)
            {
                AddModifier(property, attributes[property]);
            }
        }
    }

    private void AddModifier(string property, JToken modifier)
    {
        ValueModifier.ModifierType modType = GetModTypeFromJSON(modifier["modifier"]);
        string modAmountRPN = modifier["amount"].ToString();
        switch (property)
        {
            case "damage":
                innerSpell.damageModifiers.Add(new ValueModifier(modType, modAmountRPN)); break;
            case "mana":
                innerSpell.manaModifiers.Add(new ValueModifier(modType, modAmountRPN)); break;
            case "speed":
                innerSpell.speedModifiers.Add(new ValueModifier(modType, modAmountRPN)); break;
            case "cooldown":
                innerSpell.cooldownModifiers.Add(new ValueModifier(modType, modAmountRPN)); break;
        }
    }

    private ValueModifier.ModifierType GetModTypeFromJSON(JToken modToken)
    {
        string modString = modToken.ToString();
        return modString switch
        {
            "-" => ValueModifier.ModifierType.Subtract,
            "/" => ValueModifier.ModifierType.Divide,
            "+" => ValueModifier.ModifierType.Add,
            _ => ValueModifier.ModifierType.Multiply,
        };
    }
}