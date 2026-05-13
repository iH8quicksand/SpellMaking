using UnityEngine;

public class TestDoubleDamageModifier : ModifierSpell
{
    // The constructor just passes the variables up to the ModifierSpell base class
    public TestDoubleDamageModifier(SpellCaster owner, Spell innerSpell) : base(owner, innerSpell)
    {
    }

    // We override GetDamage to intercept the calculation
    public override int GetDamage()
    {
        // 1. Ask the inner doll what its damage is
        int originalDamage = innerSpell.GetDamage();
        
        // 2. Do our specific modifier math
        int newDamage = originalDamage * 2;
        
        // 3. Print it to the Unity Console so we can see the chain working!
        Debug.Log($"[TestDoubleDamageModifier] Inner damage was {originalDamage}. Doubling it to {newDamage}!");
        
        // 4. Pass the new value back to whoever asked
        return newDamage;
    }

    public override string GetName()
    {
        return "Doubled " + innerSpell.GetName();
    }
}