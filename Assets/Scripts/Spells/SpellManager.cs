using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

/*
 * Purpose: Initialize all the spells from spells.json at the start of the game
 *          so that SpellBuilder can access the base spells and modifier spells
 *          needed to give the player new spells at the end of each wave.
 *          
 *          Also, bridge the spell buttons in unity to the codebase. For example:
 *          Setting active spell, dropping a spell.
 *          
 * Usage: GameManager.Instance.spellManager.baseSpells/modifierSpells...
*/
public class SpellManager : MonoBehaviour
{
    public Dictionary<string,JObject> baseSpells;
    public Dictionary<string,JObject> modifierSpells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.Instance.spellManager = this;
        loadSpellsFromJSON();
    }

    /*
     * Purpose: Translate JSON file into a dictionary with all the JSON's attributes.
     *          Filter the resultant dictionary into two dictionaries for each type
     *          of spell: base spells and modifier spells.
     *          
     * Parameters: None
     * 
     * Returns: None
    */
    private void loadSpellsFromJSON()
    {
        baseSpells = new Dictionary<string, JObject>();
        modifierSpells = new Dictionary<string, JObject>();
        var spellJSON = Resources.Load<TextAsset>("spells");   // this loads the SPELLS from the spell JSON file
        Dictionary<string, JObject> allSpells = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(spellJSON.text); // this deserializes the JSON into the spells dictionary as JObjects
        foreach (var spell in allSpells)
        {
            if (spell.Value.ContainsKey("icon")) // only base spells have icons, modifier spells don't
            {
                baseSpells.Add(spell.Key, spell.Value);
            } else
            {
                modifierSpells.Add(spell.Key, spell.Value);
            }
        }
    }

    public void setSpell(int index)
    {
        EventBus.Instance.Broadcast_SetSpell(index);
    }
    public void removeSpell(int index)
    {
        EventBus.Instance.Broadcast_RemoveSpell(index);
    }
}