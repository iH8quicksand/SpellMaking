using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/*
 * Currently supported triggers:
 *  take-damage, stand-still, move, cast-spell, on-kill
 * Currently supported effects:
 *  gain-mana, gain-spellpower, gain-health
 */
public class RelicManager
{
    private List<Relic> relicPool;
    private List<Relic> tempShownRelics;
    public void LoadRelics()
    {
        var relicsJSON = Resources.Load<TextAsset>("relics");
        relicPool = JsonConvert.DeserializeObject<List<Relic>>(relicsJSON.text);
        tempShownRelics = new List<Relic>();
    }
    public Relic GetRelic()
    {
        if (relicPool.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, relicPool.Count);
        Relic relic = relicPool[index];
        tempShownRelics.Add(relic);
        relicPool.RemoveAt(index);
        return relic;
    }
    public int relicsLeft()
    {
        return relicPool.Count;
    }
    public void PutUnusedRelicsBackInPool()
    {
        foreach (Relic relic in tempShownRelics)
        {
            relicPool.Add(relic);
        }
        tempShownRelics.Clear();
    }
    public Relic ConstructNewRandomRelic()
    {
        //Just a fun thing for future, not necessary to implement anytime soon.
        throw new NotImplementedException();
    }
    public void ActivateRelic(Relic relic)
    {
        PutUnusedRelicsBackInPool();
        relicPool.Remove(relic);
        EventBus eb = EventBus.Instance;
        //Register trigger as subscriber to its event
        switch(relic.trigger.type)
        {
            case "take-damage":
                eb.OnDamage += (_, _, target) => {
                    if (target.team == Hittable.Team.PLAYER)
                        relic.OnTrigger();
                };
                break;
            case "deal-damage":
                eb.OnDamage += (_, _, target) => {
                    if (target.team == Hittable.Team.MONSTERS)
                        relic.OnTrigger();
                };
                break;
            case "stand-still":
                eb.StandingStill += relic.OnTrigger;
                break;
            case "on-kill":
                eb.OnKill += relic.OnTrigger;
                break;
            case "wave-end":
                eb.WaveEnd += relic.OnTrigger;
                break;
        }
        //Register effect "until"s as subscribers to their events
        switch(relic.effect.until)
        {
            case "move":
                eb.OnMove += relic.effect.ReadyUp;
                break;
            case "cast-spell":
                eb.OnCastSpell += relic.effect.ReadyUp;
                break;
        }
    }
}