using System;
using UnityEngine;

public class Effect
{
    public string description { get; set; }
    public string type { get; set; }
    public string amount { get; set; }
    public string until { get; set; }
    public Effect() { }

    public Action OnEffectDone;

    private bool ready = true;
    public void DoEffect()
    {
        if (!ready) return;
        EventBus eb = EventBus.Instance;
        switch(type)
        {
            case "gain-mana":
                eb.Broadcast_GainMana(amount);
                break;
            case "gain-spellpower":
                eb.Broadcast_GainSpellPower(amount);
                break;
            case "gain-health":
                eb.Broadcast_GainHealth(amount);
                break;
            case "deal-damage-random-enemy":
                Damage damageDealt = new Damage(int.Parse(amount), Damage.Type.DARK);
                Hittable enemy = GameManager.Instance.GetClosestEnemy(GameManager.Instance.player.transform.position).GetComponent<Hittable>();
                eb.DoDamage(Vector3.zero, damageDealt, enemy);
                break;
        }
        if (until != null) ready = false;
        OnEffectDone?.Invoke();
    }
    public void ReadyUp()
    {
        ready = true;
    }
}