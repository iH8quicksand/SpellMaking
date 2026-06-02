using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpriteView : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image image;

    public void Apply(string label, Sprite sprite)
    {
        this.label.text = label;
        this.image.sprite = sprite;
    }
}
