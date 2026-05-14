using UnityEngine;

public class TestDoubleDamageModifier : ModifierSpell
{
    public TestDoubleDamageModifier(SpellCaster owner, Spell innerSpell) : base(owner, innerSpell)
    {
        // Reach into the inner doll and add a * 2 multiplier to its list!
        this.innerSpell.damageModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, 2f));
    }

    public override string GetName()
    {
        return "Doubled " + innerSpell.GetName();
    }
    
    // deleted GetDamage()
}