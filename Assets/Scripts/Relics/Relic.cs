using JetBrains.Annotations;
using System.Diagnostics;

public class Relic
{
    public string name { get; set; }
    public int sprite { get; set; }
    public Trigger trigger { get; set; }
    public Effect effect { get; set; }

    public Relic()
    {

    }

    public void OnTrigger()
    {
        effect.DoEffect();
    }
}