using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject slider;
    
    public Hittable hp;
    float old_perc;

    void Update()
    {
        if (hp == null) return;
        float perc = hp.hp * 1.0f / hp.max_hp;
        if (Mathf.Abs(old_perc - perc) > 0.01f)
        {
            slider.GetComponent<RectTransform>().offsetMax = new Vector2(-Mathf.Lerp(450f, 150f, perc), 25f);
            old_perc = perc;
        }
    }

    public void SetHealth(Hittable hp)
    {
        this.hp = hp;
        float perc = hp.hp * 1.0f / hp.max_hp;
        
        slider.transform.localScale = new Vector3(perc, 1, 1);
        slider.transform.localPosition = new Vector3(-(1-perc)/2, 0, 0);
        old_perc = perc;
    }

    
}
