using UnityEngine;
using System.Collections;

public class HomingModifierSpell : ModifierSpell
{
    private string prefix;

    public HomingModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, float damageMult, float manaAdder) 
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        // Apply stat changes from JSON: decreased damage, increased mana cost
        this.GetBaseSpell().damageModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, damageMult));
        this.GetBaseSpell().manaModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Add, manaAdder));
    }

    // Override trajectory to homing — this is the behavior change
    public override string GetTrajectory()
    {
        return "homing";
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}
