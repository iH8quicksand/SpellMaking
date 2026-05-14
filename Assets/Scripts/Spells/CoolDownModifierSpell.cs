public class CoolDownModifierSpell : ModifierSpell
{
    private string prefix;

    public CoolDownModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, ValueModifier.ModifierType type, float amount) 
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        // Reach straight into the inner doll's mana list and slap the sticky note on!
        this.GetBaseSpell().cooldownModifiers.Add(new ValueModifier(type, amount));
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}