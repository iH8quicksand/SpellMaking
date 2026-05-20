using UnityEngine;
using TMPro;

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

    }

    public void StartLevel()
    {
        spawner.StartLevel(playerClass);
    }
}
