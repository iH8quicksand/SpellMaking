using System;
using UnityEngine;

public class Effect
{
    public string Description { get; set; }
    public string Type { get; set; }
    public string Amount { get; set; }
    public string Until { get; set; }
    public Effect() { }

    public Action OnEffectDone;

    private bool ready = true;
    public void DoEffect()
    {
        if (!ready) return;
        EventBus eb = EventBus.Instance;
        switch(Type)
        {
            case "gain-mana":
                eb.Broadcast_GainMana(Amount);
                break;
            case "gain-spellpower":
                eb.Broadcast_GainSpellPower(Amount);
                break;
            case "gain-health":
                eb.Broadcast_GainHealth(Amount);
                break;
            case "deal-damage-random-enemy":
                Damage damageDealt = new(int.Parse(Amount), Damage.Type.DARK);
                Hittable enemy = GameManager.Instance.GetClosestEnemy(GameManager.Instance.player.transform.position).GetComponent<EnemyController>().hp;
                eb.DoDamage(Vector3.zero, damageDealt, enemy);
                break;
            case "increase-max-health":
                GameManager.Instance.player.GetComponent<PlayerController>().hp.IncreaseMaxHealth(Amount);
                break;
                
        }
        if (Until != null) ready = false;
        OnEffectDone?.Invoke();
    }
    public void ReadyUp()
    {
        ready = true;
    }
}