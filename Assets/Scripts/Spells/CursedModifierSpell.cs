using UnityEngine;
using System.Collections;

public class CursedModifierSpell : ModifierSpell
{
    private string prefix;

    public CursedModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, float damageMult, float cdMult)
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        // Apply stat changes from JSON: decreased damage, increased mana cost
        this.GetBaseSpell().damageModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, damageMult));
        this.GetBaseSpell().cooldownModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, cdMult));
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}
