using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public Dictionary<string,JObject> spells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.spellManager = this;
        loadSpellsFromJSON();
    }

    private void loadSpellsFromJSON()
    {
        var spellJSON = Resources.Load<TextAsset>("spells");   // this loads the SPELLS from the spell JSON file
        spells = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(spellJSON.text); // this deserializes the JSON into the spells dictionary as JObjects
    }
}