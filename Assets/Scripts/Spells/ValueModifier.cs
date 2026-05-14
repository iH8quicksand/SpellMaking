using System.Collections.Generic;
using UnityEngine;

public class ValueModifier
{
    public enum ModifierType { Add, Multiply }
    
    public ModifierType type;
    public float value;

    public ValueModifier(ModifierType type, float value)
    {
        this.type = type;
        this.value = value;
    }

    // professor explicitly asked for a static method to process lists:
    public static float Apply(float baseValue, List<ValueModifier> modifiers)
    {
        float finalValue = baseValue;
        
        foreach (var mod in modifiers)
        {
            if (mod.type == ModifierType.Add)
                finalValue += mod.value;
            else if (mod.type == ModifierType.Multiply)
                finalValue *= mod.value;
        }
        
        return finalValue;
    }
}