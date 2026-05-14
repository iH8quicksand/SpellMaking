public class SpeedModifierSpell : ModifierSpell
{
    private string prefix;

    public SpeedModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, ValueModifier.ModifierType type, float amount) 
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        this.GetBaseSpell().speedModifiers.Add(new ValueModifier(type, amount));
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}
