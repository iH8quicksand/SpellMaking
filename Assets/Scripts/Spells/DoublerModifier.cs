using UnityEngine;
using System.Collections;

public class DoublerModifier : ModifierSpell
{
    private string prefix;
    private float delay; // Better to store it so we don't hardcode it!

    // 1. We need the constructor to satisfy C#, set our variables, and add our stat modifiers!
    public DoublerModifier(SpellCaster owner, Spell innerSpell, string prefix, float delay) 
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        this.delay = delay;

        // Add the sticky notes required by the JSON!
        this.GetBaseSpell().manaModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, 1.5f));
        this.GetBaseSpell().cooldownModifiers.Add(new ValueModifier(ValueModifier.ModifierType.Multiply, 1.5f));
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        yield return innerSpell.Cast(where, target, team);
        yield return new WaitForSeconds(delay); // 2. Fixed the float by using the variable!
        yield return innerSpell.Cast(where, target, team);
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName(); // 3. Use the variable!
    }
}