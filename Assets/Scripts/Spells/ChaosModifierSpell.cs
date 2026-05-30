using UnityEngine;
using System.Collections;

public class ChaosModifierSpell : ModifierSpell
{
    private string prefix;

    public ChaosModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, float damageMult)
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        // Apply stat changes from JSON: decreased damage, increased mana cost
        this.GetBaseSpell().damageModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, damageMult));    }

    // Override trajectory to homing — this is the behavior change
    public override string GetTrajectory()
    {
        return "spiraling";
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}
