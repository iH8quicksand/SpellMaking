public class ManaModifierSpell : ModifierSpell
{
    private string prefix;

    public ManaModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, ValueModifier.ModifierType type, float amount) 
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        // Reach straight into the inner doll's mana list and slap the sticky note on!
        this.GetBaseSpell().manaModifiers.Add(new ValueModifier(type, amount));
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}