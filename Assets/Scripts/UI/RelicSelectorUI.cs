using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RelicSelectorUI : MonoBehaviour
{
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI descriptionLabel;
    public GameObject icon;
    public GameObject panel;
    public GameObject button;
    private Relic relic;
    private Action<Relic> acceptRelic;
    public RelicSelectorUI() { }
    public void SetRelic(Relic relic, Action<Relic> acceptRelic)
    {
        this.relic = relic;
        this.acceptRelic = acceptRelic;
        nameLabel.text = relic.name;
        descriptionLabel.text = relic.trigger.description + " " + relic.effect.description;
        //change relic icon here
    }
    public void Take()
    {
        acceptRelic?.Invoke(relic);
        panel.GetComponent<Image>().color = new Color32(0x1A, 0xFF, 0x00, 0x76);//"#1AFF0076"
        button.SetActive(false);
    }
    public void Lock()
    {
        button.SetActive(false);
    }
}