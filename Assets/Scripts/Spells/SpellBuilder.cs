using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// Class to construct spells based on JObjects pulled from the <see cref="SpellManager"/>. Can build random spells or a specific spell.
/// </summary>
public class SpellBuilder 
{
    /// <summary>
    /// Build a random or specific spell.
    /// </summary>
    /// <param name="owner">The <see cref="SpellCaster"/> that would be casting the spell. Important for referencing player SpellPower.</param>
    /// <param name="specificSpellKey">If you want to make a specific base spell, its key would go here.</param>
    /// <param name="specificModCount">If you want a certain number of Modifiers on the spell, you can specify that here.</param>
    /// <returns>The new <see cref="Spell"/>!</returns>
    public static Spell Build(SpellCaster owner, string specificSpellKey=null, int specificModCount=-1)
    {
        // --- 1. GRAB THE DICTIONARIES ---
        var baseDict = GameManager.Instance.spellManager.baseSpells; //base spell data
        var modDict = GameManager.Instance.spellManager.modifierSpells; //modifier spell data

        // --- 2. PICK A RANDOM BASE SPELL ---
        // Convert the dictionary keys into a list so we can pick a random index (or the pre-set key)
        List<string> baseKeys = new(baseDict.Keys);
        string randomBaseId = specificSpellKey ?? baseKeys[Random.Range(0, baseKeys.Count)];
        //Get that base spell's data
        JObject spellData = baseDict[randomBaseId];
        
        // Build the innermost spell, like a Matryoshka Doll
        Spell mySpell = randomBaseId switch
        {
            "arcane_blast" => new BlastSpell(owner),
            "arcane_spray" => new SpraySpell(owner),
            _ => new Spell(owner),
        };
        mySpell.SetAttributes(spellData);

        // --- 3. DECIDE HOW MANY MODIFIERS TO ADD ---
        // Every spell gets between 1 and 3 random modifiers (or the pre-set number)
        int numModifiers = (specificModCount > -1) ? specificModCount : Random.Range(1, 4);
        if (numModifiers == 0) return mySpell;
        
        // --- 4. APPLY THE RANDOM MODIFIERS ---
        // Get a List of all the Modifier Spell keys
        List<string> modKeys = new(modDict.Keys);
        for (int i = 0; i < numModifiers; i++)
        {
            // Pick a random modifier ID (like "doubler", "damage_amp", "splitter")
            string randomModId = modKeys[Random.Range(0, modKeys.Count)];
            // Get that modifier spell's data
            JObject modSpellData = modDict[randomModId];

            // Look at which modifier we rolled, and wrap the spell in the correct class, like adding a new Matryoshka Doll layer!
            mySpell = randomModId switch
            {
                "doubler" => new DoublerModifier(owner, mySpell),
                "splitter" => new SplitterModifierSpell(owner, mySpell),
                _ => new ModifierSpell(owner, mySpell),
            };
            mySpell.SetAttributes(modSpellData);
        }

        // Return the final, crazy nested spell, like the outermost Matryoshka Doll!
        return mySpell;
    }
}
