using UnityEngine;
using System.Collections;

public class ReverseModifierSpell : ModifierSpell
{
    private string prefix;

    public ReverseModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, float manaAdder)
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        // Apply stat changes from JSON: increased mana cost
        this.GetBaseSpell().cooldownModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, 1.5f));
        this.GetBaseSpell().manaModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Add, manaAdder));
    }

    // Override trajectory to reverse — this is the behavior change
    public override string GetTrajectory()
    {
        return "reverse";
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}
