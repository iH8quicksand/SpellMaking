using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

/// <summary>
/// A <c>ModifierSpell</c> that makes a spell fire a second projectile after a <c>delay</c>.
/// </summary>
public class DoublerModifier : ModifierSpell
{
    /// <summary>
    /// Represents how many seconds after the first shot that the second fires.
    /// </summary>
    /// <value>Uses Reverse Polish Notation in <see cref="string"/> form for dynamic evaluation with <see cref="RPNEvaluator.RPNEvaluator"/></value>
    private string delay;

    /// <summary>
    /// Wraps itself around an <c>innerSpell</c> to apply itself as a modifier.
    /// </summary>
    /// <param name="owner">The <see cref="SpellCaster"/> object used for calculating values based on <see cref="SpellCaster.spell_power"/> and special Cast behavior using <see cref="SpellCaster.transform"/> in certain <see cref="ModifierSpell"/>s.</param>
    /// <param name="innerSpell">The <see cref="Spell"/> being modified.</param>
    /// <param name="prefix">The name of the <see cref="ModifierSpell"/>.</param>
    /// <param name="delay">The seconds between the first and second shot in Reverse Polish Notation (for evaluation with <see cref="RPNEvaluator.RPNEvaluator"/>).</param>
    public DoublerModifier(SpellCaster owner, Spell innerSpell) : base(owner, innerSpell) { }

    /// <summary>
    /// Overwrites <c>innerSpell</c>'s Cast function to make it fire a second projectile after a delay.
    /// </summary>
    /// <param name="where">Where the player is when they cast the spell.</param>
    /// <param name="target">Where the player clicked when they cast the spell.</param>
    /// <param name="team">The team of the spellcaster (always <see cref="Hittable.Team.PLAYER"/>).</param>
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // 1. Fire the first shot normally
        yield return innerSpell.Cast(where, target, team);
        
        // 2. Wait for the delay
        yield return new WaitForSeconds(RPNEvaluator.RPNEvaluator.Evaluatef(delay,GetRPNDict()));
        
        // 3. Figure out the direction they originally aimed
        Vector3 direction = target - where; 
        
        // 4. Get the LIVE position of the player right now
        Vector3 newWhere = owner.transform.position; 
        
        // 5. Apply the original aim direction to the new position
        Vector3 newTarget = newWhere + direction;
        
        // 6. Fire the delayed shot from the updated position!
        yield return innerSpell.Cast(newWhere, newTarget, team);
    }

    /// <summary>
    /// Set the <see cref="DoublerModifier"/>'s specific attributes based on a <see cref="JObject"/> pulled from the spells.json.
    /// </summary>
    /// <param name="attributes">The <see cref="DoublerModifier"/>'s attributes from the JSON file.</param>
    public override void SetAttributes(JObject attributes)
    {
        base.SetAttributes(attributes);
        if (attributes["delay"] != null)
            delay = attributes["delay"].ToString();
    }
}