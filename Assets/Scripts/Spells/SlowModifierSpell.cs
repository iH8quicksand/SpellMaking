public class SlowModifierSpell : ModifierSpell
{
    private string prefix;

    public SlowModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, ValueModifier.ModifierType type, float amount, float damageMult)
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        this.GetBaseSpell().damageModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, damageMult));
        this.GetBaseSpell().speedModifiers.Add(new ValueModifier(type, amount));
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}
