using System;

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
        }
        if (until != null) ready = false;
        OnEffectDone?.Invoke();
    }
    public void ReadyUp()
    {
        ready = true;
    }
}