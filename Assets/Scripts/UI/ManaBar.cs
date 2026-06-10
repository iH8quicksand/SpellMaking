using TMPro;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    public GameObject slider;
    public TextMeshProUGUI label;

    private SpellCaster sc;
    float old_perc;

    // Update is called once per frame
    void Update()
    {
        if (sc == null) return;
        float perc = sc.mana * 1.0f / sc.max_mana;
        if (Mathf.Abs(perc - old_perc) > 0.001f)
        {
            slider.GetComponent<RectTransform>().offsetMax = new Vector2(-Mathf.Lerp(450f, 150f, perc), 25f);
            label.text = "Mana: " + sc.mana + "/" + sc.max_mana;
            old_perc = perc;
        }
    }

    public void SetSpellCaster(SpellCaster sc)
    {
        this.sc = sc;
        old_perc = sc.mana * 1.0f / sc.max_mana;
        label.text = "Mana: " + sc.mana + "/" + sc.max_mana;
    }
}
