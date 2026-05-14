using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public Spell Build(SpellCaster owner)
    {
        // this line below is what originally was here
        //return new Spell(owner);


        // Create the innermost doll
        Spell mySpell = new Spell(owner); 
        
        // add a valuemodifier to the list field in the spell
        mySpell = new ManaModifierSpell(owner, mySpell, "Cheap", ValueModifier.ModifierType.Multiply, 0.1f);
        mySpell = new DamageModifierSpell(owner, mySpell, "killer", ValueModifier.ModifierType.Multiply, 9f);
        mySpell = new DoublerModifier(owner, mySpell, "double ts", 0.5f);       
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
