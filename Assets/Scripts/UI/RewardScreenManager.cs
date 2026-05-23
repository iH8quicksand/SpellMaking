using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI wavesClearedText;
    public GameObject spellsUI;
    public GameObject relicUIPrefab;
    public RelicManager relicManager;
    public GameObject relicUI;

    //New Spell Attributes
    public GameObject icon;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI mana;
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI description;
    public GameObject spellPanel;
    public GameObject getSpellButton;
    private Spell offeredSpell;

    private List<GameObject> relicSelectors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.WaveEnd += Show;
        EventBus.Instance.WaveStart += Hide;
        relicManager = new RelicManager();
        relicManager.LoadRelics();
        AcceptRelic += relicManager.ActivateRelic;
        AcceptRelic += LockOtherRelics;
        AcceptRelic += relicUI.GetComponent<RelicUIManager>().OnRelicPickup;
    }

    public void Show()
    {
        offeredSpell = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.GenerateRandomSpell();
        damageText.text = $"Damage Dealt: {GameManager.Instance.total_damage_dealt}";
        wavesClearedText.text = $"Waves Cleared: {GameManager.Instance.GetWave()}";
        GameManager.Instance.spellIconManager.PlaceSprite(offeredSpell.GetIcon(), icon.GetComponent<Image>());
        mana.text = offeredSpell.GetManaCost().ToString();
        damage.text = offeredSpell.GetDamage().ToString();
        spellName.text = offeredSpell.GetName();
        //description.text = offeredSpell.GetDescription();
        getSpellButton.SetActive(true);


        if (GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spells.Count == 4)
        {
            spellsUI.GetComponent<SpellUIContainer>().showDropButtons();
        }

        if (GameManager.Instance.GetWave() % 3 == 0) ShowRelics();

        spellPanel.GetComponent<Image>().color = new Color32(0x00, 0x00, 0x00, 0x22);
        rewardUI.SetActive(true);
    }

    public void Hide()
    {
        ClearRelics();
        rewardUI.SetActive(false);
    }

    public void ShowRelics()
    {
        int availableRelics = Math.Min(3, relicManager.relicsLeft());
        float spacingX = 300f;
        float startX = -((availableRelics - 1) * spacingX) / 2f;
        float currentX = startX;
        relicSelectors = new List<GameObject>();
        for (int i = 0; i < availableRelics; i++)
        {
            GameObject selector = Instantiate(relicUIPrefab, rewardUI.transform);
            selector.transform.localPosition = new Vector3(currentX, -135, 0);
            Relic relic = relicManager.GetRelic();
            selector.GetComponent<RelicSelectorUI>().SetRelic(relic, AcceptRelic);
            relicSelectors.Add(selector);
            currentX += spacingX;
        }
    }
    public void ClearRelics()
    {
        if (relicSelectors == null) return;
        foreach (GameObject relicSelector in  relicSelectors)
        {
            Destroy(relicSelector);
        }
    }

    public void AcceptSpell()
    {
        if (GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spells.Count < 4)
        {
            EventBus.Instance.Broadcast_AddSpell(offeredSpell);
            spellPanel.GetComponent<Image>().color = new Color32(0x1A, 0xFF, 0x00, 0x76);
            getSpellButton.SetActive(false);
            
        }
    }
    public Action<Relic> AcceptRelic;
    public void LockOtherRelics(Relic _)
    {
        foreach (GameObject relicSelector in relicSelectors)
        {
            relicSelector.GetComponent<RelicSelectorUI>().Lock();
        }
    }

    public void DEBUG_GetRandomSpell()
    {
        offeredSpell = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.GenerateRandomSpell();
        if (GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spells.Count < 4)
        {
            EventBus.Instance.Broadcast_AddSpell(offeredSpell);
        }
    }
}
