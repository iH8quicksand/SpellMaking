using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class ModifierSpell : Spell
{
    protected Spell innerSpell;

    // I'm just a wrapper, so ask the doll inside me!
    public override Spell GetBaseSpell()
    {
        return innerSpell.GetBaseSpell();
    }

    // The constructor takes the spell it is wrapping
    public ModifierSpell(SpellCaster owner, Spell innerSpell) : base(owner)
    {
        this.innerSpell = innerSpell;
    }

    public override string GetName() => innerSpell.GetName();
    public override string GetDescription() => innerSpell.GetDescription();
    public override int GetManaCost() => innerSpell.GetManaCost();
    public override int GetDamage() => innerSpell.GetDamage();
    public override float GetCooldown() => innerSpell.GetCooldown();
    public override int GetIcon() => innerSpell.GetIcon();

    // Pass the cast down to the inner spell by default
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        yield return innerSpell.Cast(where, target, team);
    }
    
    public override void SetAttributes(JObject attributes)
    {
        // Modifiers will read their specific multipliers/adders here
    }
}