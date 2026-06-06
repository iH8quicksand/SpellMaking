using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster 
{
    public Transform transform; // so doublerModifier can get players location
    public int mana;
    public int max_mana;
    public int mana_reg;
    public int spell_power;
    public Hittable.Team team;
    public List<Spell> spells;
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
        spells = new List<Spell>
        {
            SpellBuilder.Build(this, "arcane_bolt", specificModCount:0)
        };
        equippedSpellIndex = 0;
        EventBus.Instance.AddSpell += AddSpell;
        EventBus.Instance.SetSpell += SetSpell;
        EventBus.Instance.RemoveSpell += RemoveSpell;
    }

    public Spell GenerateRandomSpell()
    {
        return SpellBuilder.Build(this);
    }

    public void AddSpell(Spell newSpell)
    {
        spells.Add(newSpell);
    }
    public void RemoveSpell(int index)
    {
        spells.RemoveAt(index);
        if (equippedSpellIndex >= index) equippedSpellIndex--;
    }

    public IEnumerator Cast(Transform cameraTransform)
    {
        if (mana >= spells[equippedSpellIndex].GetManaCost() && spells[equippedSpellIndex].IsReady())
        {
            mana -= spells[equippedSpellIndex].GetManaCost();
            Vector3 targetOffset = cameraTransform.rotation * Vector3.forward;
            Vector3 cameraPosition = cameraTransform.position;
            yield return spells[equippedSpellIndex].Cast(cameraPosition + new Vector3(0f, -1f, 0f), cameraPosition + targetOffset * 2f + new Vector3(0f,-1f,0f), team);
        }
        yield break;
    }

    private void SetSpell(int index)
    {
        equippedSpellIndex = index;
    }

    public void GainSpellPower(string rpn_gainedeSpellPower)
    {
        Dictionary<string, int> rpnDict = new() { { "wave", GameManager.Instance.GetWave() } };
        spell_power += RPNEvaluator.RPNEvaluator.Evaluate(rpn_gainedeSpellPower, rpnDict);
    }
    public void GainMana(string rpn_gainedeMana)
    {
        Dictionary<string, int> rpnDict = new() { { "wave", GameManager.Instance.GetWave() } };
        mana += RPNEvaluator.RPNEvaluator.Evaluate(rpn_gainedeMana, rpnDict);
    }

}
