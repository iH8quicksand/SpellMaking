using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class EventBus 
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    public event Action WaveEnd;
    public event Action WaveStart;
    public event Action<Spell> AddSpell;
    public event Action<int> SetSpell;
    
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
        SetSpell?.Invoke(index);
    }

}
