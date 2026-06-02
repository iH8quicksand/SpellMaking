using JetBrains.Annotations;
using System.Diagnostics;

public class Relic
{
    public string Name { get; set; }
    public int Sprite { get; set; }
    public Trigger Trigger { get; set; }
    public Effect Effect { get; set; }

    public Relic()
    {

    }

    public void OnTrigger()
    {
        Effect.DoEffect();
    }
}