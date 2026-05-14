using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpellCaster 
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public int spell_power;
    public Hittable.Team team;
    public List<Spell> spell;
    private int equippedSpellIndex;

    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }

    public SpellCaster(int mana, int mana_reg, int spell_power, Hittable.Team team)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.spell_power = spell_power;
        this.team = team;
        spell.Append(new SpellBuilder().Build(this));
        equippedSpellIndex = 0;
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {        
        if (mana >= spell[equippedSpellIndex].GetManaCost() && spell[equippedSpellIndex].IsReady())
        {
            mana -= spell[equippedSpellIndex].GetManaCost();
            yield return spell[equippedSpellIndex].Cast(where, target, team);
        }
        yield break;
    }

}
