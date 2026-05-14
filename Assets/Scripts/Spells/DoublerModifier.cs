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
        // 1. Fire the first shot normally
        yield return innerSpell.Cast(where, target, team);
        
        // 2. Wait for the delay
        yield return new WaitForSeconds(delay);
        
        // 3. Figure out the direction they originally aimed
        Vector3 direction = target - where; 
        
        // 4. Get the LIVE position of the player right now
        Vector3 newWhere = owner.transform.position; 
        
        // 5. Apply the original aim direction to the new position
        Vector3 newTarget = newWhere + direction;
        
        // 6. Fire the delayed shot from the updated position!
        yield return innerSpell.Cast(newWhere, newTarget, team);
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName(); // 3. Use the variable!
    }
}