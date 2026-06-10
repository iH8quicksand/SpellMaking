using UnityEngine;
using TMPro;

public class MenuSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public string level;
    public EnemySpawner spawner;

    public void SetLevel(string text)
    {
        level = text;
        label.text = text;
    }

    public void StartLevel()
    {
        ButtonAudio.Instance.PlayClick();
        spawner.SelectLevel(level);
    }
}
