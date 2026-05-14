using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI damageText;

    //New Spell Attributes
    public GameObject icon;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI mana;
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI description;

    private SpellCaster spellCaster;
    private Spell spell;

    void Start()
    {
        GameManager.Instance.rewardScreenManager = this;
    }
    private Spell getNewSpell()
    {
        //return spellCaster.spellBuilder.Build(spellCaster);
        return new SpellBuilder().Build(null);
    }
    public void Show()
    {
        spellCaster = GameManager.Instance.playerController.spellcaster;
        damageText.text = $"Damage Dealt: {GameManager.Instance.total_damage_dealt}";
        spell = getNewSpell();
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
        mana.text = spell.GetManaCost().ToString();
        damage.text = spell.GetDamage().ToString();
        spellName.text = spell.GetName();
        description.text = spell.GetDescription();

        if (spellCaster.spells.Count == 4)
        {
            GameManager.Instance.spellUIContainer.showDropButtons();
        }

        rewardUI.SetActive(true);
    }
    public void NextRound()
    {
        rewardUI.SetActive(false);
        GameManager.Instance.StartCountdown();
    }
    public void GetSpell()
    {
        if (spellCaster.spells.Count < 4)
        {
            spellCaster.spells.Add(spell);
            NextRound();
        }
    }
}
