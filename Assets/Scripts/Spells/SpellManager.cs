using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private Dictionary<string,Spell> spells;
    void Start()
    {
        this.spells = new Dictionary<string, Spell>();
        loadSpellsFromJSON();
        Debug.Log(this.spells);
        Debug.Log("hello world!");
    }

    private void loadSpellsFromJSON()
    {
        var spellJSON = Resources.Load<TextAsset>("spells");   // this loads the SPELLS from the spell JSON file
        JToken rawText = JToken.Parse(spellJSON.text);
        foreach (var spell in rawText)
        {
            Spell sp = spell.ToObject<Spell>();
            this.spells[sp.GetName()] = sp;
        }
    }
}