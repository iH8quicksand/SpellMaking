using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI damageText;
    public GameObject spellsUI;

    //New Spell Attributes
    public GameObject icon;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI mana;
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI description;
    private Spell offeredSpell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.WaveEnd += Show;
        EventBus.Instance.WaveStart += Hide;
    }

    public void Show()
    {
        offeredSpell = GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.GenerateRandomSpell();
        damageText.text = $"Damage Dealt: {GameManager.Instance.total_damage_dealt}";
        GameManager.Instance.spellIconManager.PlaceSprite(offeredSpell.GetIcon(), icon.GetComponent<Image>());
        mana.text = offeredSpell.GetManaCost().ToString();
        damage.text = offeredSpell.GetDamage().ToString();
        spellName.text = offeredSpell.GetName();
        //description.text = offeredSpell.GetDescription();

        if (GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spells.Count == 4)
        {
            spellsUI.GetComponent<SpellUIContainer>().showDropButtons();
        }

        rewardUI.SetActive(true);
    }

    public void Hide()
    {
        rewardUI.SetActive(false);
    }

    public void AcceptSpell()
    {
        if (GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spells.Count < 4)
        {
            EventBus.Instance.Broadcast_AddSpell(offeredSpell);
            GameManager.Instance.player.GetComponentInChildren<EnemySpawner>().NextWave();
        }
    }
}
