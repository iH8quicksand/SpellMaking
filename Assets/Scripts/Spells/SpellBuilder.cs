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
        
        // Wrap the base spell in our test modifier
        // we are calling the TestDoubleDamageModifier's constructor
        mySpell = new TestDoubleDamageModifier(owner, mySpell);
        
        // 3. BONUS TEST: Wrap it AGAIN to prove the nesting dolls can chain infinitely!
        mySpell = new TestDoubleDamageModifier(owner, mySpell); 

        return mySpell;
    }

   
    public SpellBuilder()
    {        
    }

}
