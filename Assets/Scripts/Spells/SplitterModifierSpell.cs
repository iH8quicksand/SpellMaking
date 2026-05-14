using UnityEngine;
using System.Collections;

public class SplitterModifierSpell : ModifierSpell
{
    private string prefix;
    private float splitAngle;

    // The constructor takes in the angle from the JSON
    public SplitterModifierSpell(SpellCaster owner, Spell innerSpell, string prefix, float splitAngle) 
        : base(owner, innerSpell)
    {
        this.prefix = prefix;
        this.splitAngle = splitAngle;
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // 1. Get the original straight direction
        Vector3 direction = target - where;
        
        // 2. We want to split the angle from the JSON in half so it's perfectly symmetrical
        float halfAngle = this.splitAngle / 2f; 
        
        // 3. Calculate the two new directions by rotating the original direction around the Z-axis
        Vector3 leftDir = Quaternion.Euler(0, 0, halfAngle) * direction;
        Vector3 rightDir = Quaternion.Euler(0, 0, -halfAngle) * direction;
        
        // 4. Get the live position of the caster to avoid the "outrunning the gun" bug!
        Vector3 currentPos = owner.transform.position;
        
        // 5. Calculate the two new target coordinates using the rotated directions
        Vector3 targetOne = currentPos + leftDir;
        Vector3 targetTwo = currentPos + rightDir;
        
        // 6. Fire both spells! 
        // We use GameManager to start the first one independently so they fire at the EXACT same time,
        // rather than waiting for the first coroutine to entirely finish before shooting the second.
        GameManager.Instance.projectileManager.StartCoroutine(innerSpell.Cast(currentPos, targetOne, team));
        yield return innerSpell.Cast(currentPos, targetTwo, team);
    }

    public override string GetName()
    {
        return prefix + " " + innerSpell.GetName();
    }
}