using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initialize all the spells from spells.json at the start of the game
/// so that SpellBuilder can access the base spells and modifier spells
/// needed to give the player new spells at the end of each wave.
/// Also, bridge the spell buttons in Unity to the codebase. For example:
/// Setting active spell, dropping a spell.
/// <example>
/// Usage: <code>GameManager.Instance.spellManager.baseSpells</code> or <code>GameManager.Instance.spellManager.modifierSpells</code>
/// </example>
/// </summary>
public class SpellManager : MonoBehaviour
{
    public Dictionary<string,JObject> baseSpells;
    public Dictionary<string,JObject> modifierSpells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.Instance.spellManager = this;
        LoadSpellsFromJSON();
    }

    /// <summary>
    /// Translates JSON file into a dictionary with all the JSON's attributes.
    /// Filters the resultant dictionary into two dictionaries for each type
    /// of spell: base spells and modifier spells.
    /// </summary>
    private void LoadSpellsFromJSON()
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

    /// <summary>
    /// Called when a spell hotbar slot is clicked on. Used to change the player's active spell.
    /// </summary>
    /// <param name="index">0-3 depending on which slot was clicked.</param>
    public void SetSpell(int index)
    {
        EventBus.Instance.Broadcast_SetSpell(index);
    }
    /// <summary>
    /// Called when a spell hotbar slot's drop button is clicked. Used to remove a spell from the player's "inventory".
    /// </summary>
    /// <param name="index">0-3 depending on which slot's drop button was clicked.</param>
    public void RemoveSpell(int index)
    {
        EventBus.Instance.Broadcast_RemoveSpell(index);
    }
}