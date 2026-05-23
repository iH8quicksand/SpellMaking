using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class RelicUI : MonoBehaviour
{
    public PlayerController player;
    public int index;
    private Relic relic;

    public Image icon;
    public GameObject highlight;
    public TextMeshProUGUI label;
    public GameObject description;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if a player has relics, this is how you *could* show them
        Relic r = player.relics[index];
        GameManager.Instance.relicIconManager.PlaceSprite(r.sprite, icon);
    }

    // Update is called once per frame
    void Update()
    {
        // Relics could have labels and/or an active-status
        Relic r = player.relics[index];
        label.text = r.GetName();
        highlight.SetActive(r.IsActive());
    }

    public void SetRelic(Relic relic)
    {
        this.relic = relic;
        UpdateStatLabel();
        EventBus.Instance.WaveEnd += UpdateStatLabel;
        description.GetComponent<TextMeshProUGUI>().text = relic.trigger.description + " " + relic.effect.description;
        switch(relic.effect.type)
        {
            case "gain-mana":
                label.color = new Color32(0x00, 0x03, 0xFF, 0xFF);
                break;
            case "gain-spellpower":
                label.color = new Color32(0xA7, 0x00, 0xFF, 0xFF);
                break;
            case "gain-health":
                label.color = new Color32(0xFF, 0x00, 0x00, 0xFF);
                break;
        }
        GameManager.Instance.relicIconManager.PlaceSprite(relic.sprite, icon);
        relic.effect.OnEffectDone += OnTrigger;
    }
    public void UpdateStatLabel()
    {
        Dictionary<string, int> rpnDict = new Dictionary<string, int> { {"wave", GameManager.Instance.GetWave()}, {"power", player.spellcaster.spell_power} };
        label.text = RPNEvaluator.RPNEvaluator.Evaluate(relic.effect.amount, rpnDict).ToString();
    }
    public void OnTrigger()
    {
        StartCoroutine(ShowHighlight());
    }

    public void ToggleDescription()
    {
        if (!description.activeSelf) description.SetActive(true);
        else description.SetActive(false);
    }

    private IEnumerator ShowHighlight()
    {
        highlight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        highlight.SetActive(false);
    }
}
