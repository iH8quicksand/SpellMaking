using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject slider;
    public TextMeshProUGUI label;
    
    private Hittable hp;
    private float old_perc;

    void Update()
    {
        if (hp == null) return;
        float perc = hp.hp * 1.0f / hp.max_hp;
        if (Mathf.Abs(perc - old_perc) > 0.1f)
        {
            slider.GetComponent<RectTransform>().offsetMax = new Vector2(-Mathf.Lerp(550f, 150f, perc), 25f);
            label.text = "HP: " + hp.hp + "/" + hp.max_hp;
            old_perc = perc;
        }
    }

    public void SetHealth(Hittable hp)
    {
        this.hp = hp;
        old_perc = hp.hp * 1.0f / hp.max_hp;
        label.text = "HP: " + hp.hp + "/" + hp.max_hp;
    }
}
