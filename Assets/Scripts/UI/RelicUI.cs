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

    private void OnDestroy()
    {
        EventBus.Instance.WaveEnd -= UpdateStatLabel;
        relic.Effect.OnEffectDone -= OnTrigger;
    }

    public void SetRelic(Relic relic)
    {
        this.relic = relic;
        UpdateStatLabel();
        EventBus.Instance.WaveEnd += UpdateStatLabel;
        description.GetComponent<TextMeshProUGUI>().text = relic.Trigger.Description + " " + relic.Effect.Description;
        switch(relic.Effect.Type)
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
        GameManager.Instance.relicIconManager.PlaceSprite(relic.Sprite, icon);
        relic.Effect.OnEffectDone += OnTrigger;
    }
    public void UpdateStatLabel()
    {
        Dictionary<string, int> rpnDict = new() { {"wave", GameManager.Instance.GetWave()}, {"power", player.spellcaster.spell_power} };
        label.text = RPNEvaluator.RPNEvaluator.Evaluate(relic.Effect.Amount, rpnDict).ToString();
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
