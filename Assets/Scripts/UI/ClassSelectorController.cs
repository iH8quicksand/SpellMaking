using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClassSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public GameObject icon;
    public PlayerClass playerClass;
    public EnemySpawner spawner;

    public void SetClass(string name, PlayerClass pc)
    {
        playerClass = pc;
        label.text = name;
        GameManager.Instance.playerSpriteManager.PlaceSprite(pc.Sprite, icon.GetComponent<Image>());

    }

    public void StartLevel()
    {
        ButtonAudio.Instance.PlayClick();
        spawner.StartLevel(playerClass);
    }
}
