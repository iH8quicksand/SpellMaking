using System.Collections.Generic;

/// <summary>
/// Represents a modification, a calculation, that can be applied to some base value.
/// </summary>
public class ValueModifier
{
    public enum ModifierType { Multiply, Divide, Add, Subtract, Overwrite }
    
    public ModifierType type;
    public string value;

    /// <summary>
    /// Creates a new <see cref="ValueModifier"/>.
    /// </summary>
    /// <param name="type">What operation to use (+,-,*,/).</param>
    /// <param name="value">How much to modify the base value using the specified operator.</param>
    public ValueModifier(ModifierType type, string value)
    {
        this.type = type;
        this.value = value;
    }

    // professor explicitly asked for a static method to process lists:
    /// <summary>
    /// Applies a list of <see cref="ValueModifier"/>s to a base value to produce a final, modified value.
    /// </summary>
    /// <param name="baseValue">The base value that all the <see cref="ValueModifier"/>s are applied to.</param>
    /// <param name="modifiers">All of the <see cref="ValueModifier"/>s to apply to the base value.</param>
    /// <param name="rpnDict">The variable lookup <see cref="Dictionary{TKey, TValue"/> used by <see cref="RPNEvaluator.RPNEvaluator"/> to evaluate a Reverse Polish Notation <see cref="string"/> that uses variables (ex. "wave" or "power").</param>
    /// <returns>The final value after all <see cref="ValueModifier"/>s have been applied.</returns>
    public static float Apply(string baseValue, List<ValueModifier> modifiers, Dictionary<string,int> rpnDict)
    {
        float finalValue = RPNEvaluator.RPNEvaluator.Evaluatef(baseValue,rpnDict);
        
        foreach (var mod in modifiers)
        {
            float modValue = RPNEvaluator.RPNEvaluator.Evaluatef(mod.value,rpnDict);
            switch(mod.type)
            {
                case ModifierType.Multiply:
                    finalValue *= modValue; break;
                case ModifierType.Divide:
                    finalValue /= modValue; break;
                case ModifierType.Add:
                    finalValue += modValue; break;
                case ModifierType.Subtract:
                    finalValue -= modValue; break;
                case ModifierType.Overwrite:
                    finalValue = modValue; break;
            }
        }
        
        return finalValue;
    }
}