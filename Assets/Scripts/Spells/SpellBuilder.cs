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
        
        return mySpell;
    }

   
    public SpellBuilder()
    {        
    }

}
