using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.spellUIContainer = this;
        spellUIs[0].SetActive(true);
        for (int i = 1; i < spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(false);
        }
    }
    public void UpdateUIs()
    {
        SpellCaster spellCaster = GameManager.Instance.playerController.spellcaster;
        for (int i = 0; i < spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(false);
        }
        for (int i = 0; i < Mathf.Min(spellCaster.spells.Count, spellUIs.Length); ++i)
        {
            spellUIs[i].SetActive(true);
        }
    }

    public void showDropButtons()
    {
        foreach (GameObject spellUI in spellUIs)
        {
            spellUI.GetComponent<SpellUI>().dropbutton.SetActive(true);
        }
    }
    public void hideDropButtons()
    {
        foreach (GameObject spellUI in spellUIs)
        {
            spellUI.GetComponent<SpellUI>().dropbutton.SetActive(false);
        }
    }

}
