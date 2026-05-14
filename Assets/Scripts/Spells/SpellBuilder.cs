using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public Spell Build(SpellCaster owner)
    {
        // --- 1. GRAB THE DICTIONARIES ---
        var baseDict = GameManager.Instance.spellManager.baseSpells;
        // Assuming your teammate named the second dictionary "modifierSpells"
        var modDict = GameManager.Instance.spellManager.modifierSpells; 

        // --- 2. PICK A RANDOM BASE SPELL ---
        // Convert the dictionary keys into a list so we can pick a random index
        List<string> baseKeys = new List<string>(baseDict.Keys);
        string randomBaseId = baseKeys[Random.Range(0, baseKeys.Count)];
        
        // Build the innermost doll using the logic we already perfected!
        Spell mySpell = new Spell(owner);
        mySpell.SetAttributes(baseDict[randomBaseId]);

        // --- 3. DECIDE HOW MANY MODIFIERS TO ADD ---
        // Let's say every spell gets between 1 and 3 random modifiers
        int numModifiers = Random.Range(1, 4); 

        // --- 4. APPLY THE RANDOM MODIFIERS ---
        List<string> modKeys = new List<string>(modDict.Keys);

        for (int i = 0; i < numModifiers; i++)
        {
            // Pick a random modifier ID (like "doubler", "damage_amp", "splitter")
            string randomModId = modKeys[Random.Range(0, modKeys.Count)];
            JObject modJson = modDict[randomModId];
            
            // Extract the name/prefix from the JSON so we can use it in GetName()
            string prefix = (string)modJson["name"];

            // Look at which modifier we rolled, and wrap the spell in the correct class!
            switch (randomModId)
            {
                case "doubler":
                    // Parse the delay from the JSON, then wrap it!
                    float delay = (float)modJson["delay"];
                    mySpell = new DoublerModifier(owner, mySpell, prefix, delay);
                    break;

                case "splitter":
                    // Parse the angle from the JSON, then wrap it!
                    float angle = (float)modJson["angle"];
                    // Assuming you named your class SplitterModifierSpell
                    mySpell = new SplitterModifierSpell(owner, mySpell, prefix, angle); 
                    break;

                case "damage_amp":
                    // Parse the multiplier, then wrap it!
                    float dmgMult = (float)modJson["damage_multiplier"];
                    mySpell = new DamageModifierSpell(owner, mySpell, prefix, ValueModifier.ModifierType.Multiply, dmgMult);
                    
                    // The assignment says damage_amp ALSO increases mana cost, so we wrap it again!
                    float manaMult = (float)modJson["mana_multiplier"];
                    mySpell = new ManaModifierSpell(owner, mySpell, prefix, ValueModifier.ModifierType.Multiply, manaMult);
                    break;

                // Add more cases here for speed_amp, chaos, homing, etc.!
            }
        }

        // Return the final, crazy nested doll!
        return mySpell;
    }

    // this is what should get called at the start of the game to give the player
    // Arcane Bolt (or whatever spell is first in the JSON)
    public Spell BuildSpecificSpell(SpellCaster owner , string spellName)
    {
        Spell baseSpell = new Spell(owner); 
        // 2. Fetch "arcane_bolt" (or whatever spellID is passed in) from the Base Spells dictionary
        JObject spellData = GameManager.Instance.spellManager.baseSpells[spellName];

        // 3. The blank slate reads the JSON and "becomes" the Arcane Bolt
        baseSpell.SetAttributes(spellData);
        return baseSpell;
    }

   
    public SpellBuilder()
    {        
    }

}
