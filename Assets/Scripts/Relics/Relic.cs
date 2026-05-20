using JetBrains.Annotations;

public class Relic
{
    private string trigger; // I know these should actually be triggers but this is placeholder
    private string effect;

    // constructor
    public Relic (string trigger, string effect)
    {
        this.trigger = trigger;
        //SUBSCRIBE TO EVENTBUS FOR THAT TRIGGER HERE
        this.effect = effect;
    }

    public void UseEffect ()
    {
        // something like: this.effect.use
    }
}