using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class EventBus 
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            theInstance ??= new EventBus(); //SINGLETON YES
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    public event Action WaveEnd;
    public event Action WaveStart;
    public event Action<Spell> AddSpell;
    public event Action<int> SetSpell;
    public event Action<int> RemoveSpell;
    public event Action<string> GainMana;
    public event Action StandingStill;
    public event Action<string> GainSpellPower;
    public event Action OnKill;
    public event Action OnMove;
    public event Action OnCastSpell;
    public event Action<string> GainHealth;
    public event Action<string> MaxHealthIncrease;
    
    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }
    public void Broadcast_WaveEnd()
    {
        WaveEnd?.Invoke();
    }
    public void Broadcast_WaveStart()
    {
        WaveStart?.Invoke();
    }
    public void Broadcast_AddSpell(Spell newSpell)
    {
        AddSpell?.Invoke(newSpell);
    }
    public void Broadcast_SetSpell(int index)
    {
        ButtonAudioManager.Instance.PlayClick();
        SetSpell?.Invoke(index);
    }
    public void Broadcast_RemoveSpell(int index)
    {
        RemoveSpell?.Invoke(index);
    }
    public void Broadcast_GainMana(string rpn_manaGained)//effect to receive
    {
        GainMana?.Invoke(rpn_manaGained);
    }
    public void Broadcast_StandingStill()//trigger to send
    {
        StandingStill?.Invoke();
    }
    public void Broadcast_GainSpellPower(string rpn_spellPower)//effect to receive
    {
        GainSpellPower?.Invoke(rpn_spellPower);
    }
    public void Broadcast_OnKill()//trigger to send
    {
        OnKill?.Invoke();
    }
    public void Broadcast_OnMove()//trigger to send
    {
        OnMove?.Invoke();
    }
    public void Broadcast_OnCastSpell()//trigger to send
    {
        OnCastSpell?.Invoke();
    }
    public void Broadcast_GainHealth(string rpn_healthGained)//effect to receive
    {
        GainHealth?.Invoke(rpn_healthGained);
    }
    public void Broadcast_IncreaseMaxHealth(string rpn_maxhealthincrease)
    {
        MaxHealthIncrease?.Invoke(rpn_maxhealthincrease);
    }

}
