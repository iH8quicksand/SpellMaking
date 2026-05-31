using System;
using System.Collections.Generic;
using UnityEngine;

public class Hittable
{

    public enum Team { PLAYER, MONSTERS }
    public Team team;

    public int hp;
    public int max_hp;

    public GameObject owner;

    public void Damage(Damage damage)
    {
        EventBus.Instance.DoDamage(owner.transform.position, damage, this);
        hp -= damage.amount;
        GameManager.Instance.RegisterDamage(damage.amount);
        if (hp <= 0)
        {
            hp = 0;
            OnDeath();
        }
    }

    public event Action OnDeath;

    public Hittable(int hp, Team team, GameObject owner)
    {
        this.hp = hp;
        this.max_hp = hp;
        this.team = team;
        this.owner = owner;
    }

    public void SetMaxHP(int max_hp)
    {
        float perc = this.hp * 1.0f / this.max_hp;
        this.max_hp = max_hp;
        this.hp = Mathf.RoundToInt(perc * max_hp);
    }
    public void IncreaseMaxHealth(string amount)
    {
        Dictionary<string, int> rpnDict = new Dictionary<string, int> { { "wave", GameManager.Instance.GetWave() } };
        this.max_hp += RPNEvaluator.RPNEvaluator.Evaluate(amount, rpnDict);
    }
    public void GainHP(string rpn_hpGained)
    {
        Dictionary<string, int> rpnDict = new Dictionary<string, int> { { "wave", GameManager.Instance.GetWave() } };
        hp += RPNEvaluator.RPNEvaluator.Evaluate(rpn_hpGained, rpnDict);
        hp = Math.Min(hp, max_hp);
    }
}
