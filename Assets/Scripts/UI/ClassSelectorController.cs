using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClassSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public GameObject icon;
    public TextMeshProUGUI description;
    public PlayerClass playerClass;
    public EnemySpawner spawner;

    public void SetClass(string name, PlayerClass pc)
    {
        playerClass = pc;
        label.text = name;
        GameManager.Instance.playerSpriteManager.PlaceSprite(pc.Sprite, icon.GetComponent<Image>());
        description.text = "HP: " + pc.Health + "\nMana: " + pc.Mana + "\nMana Regen: " + pc.Mana_Regeneration + "\nSpellpower: " + pc.Spellpower + "\nSpeed: " + pc.Speed;
    }

    public void StartLevel()
    {
        ButtonAudioManager.Instance.PlayClick();
        spawner.StartLevel(playerClass);
    }
}
