using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we only have one spell (right now)
        spellUIs[0].SetActive(true);
        for(int i = 1; i< spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(false);
        }
        EventBus.Instance.AddSpell += AddSpell;
        EventBus.Instance.RemoveSpell += RemoveSpell;
        EventBus.Instance.WaveStart += hideDropButtons;
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
    public void AddSpell(Spell newSpell)
    {
        // First, get the index of the next free spell UI
        int freeIndex = 0;
        while (freeIndex < spellUIs.Length)
        {
            if (spellUIs[freeIndex].activeSelf == false) break; //if the slot at freeIndex is inactive, break (freeIndex found)
            freeIndex++;
        }
        //Next, activate the spell UI and set its spell
        Debug.Log("Free index found: " + freeIndex + ". (spellUIs.Length = " + spellUIs.Length);
        spellUIs[freeIndex].GetComponent<SpellUI>().SetSpell(newSpell);
        spellUIs[freeIndex].SetActive(true);
    }
    public void RemoveSpell(int index)
    {
        spellUIs[3].SetActive(false);
        for (int i=0; i<spellUIs.Length-1; i++)
        {
            spellUIs[i].GetComponent<SpellUI>().SetSpell(GameManager.Instance.player.GetComponent<PlayerController>().spellcaster.spells[i]);
        }
        hideDropButtons();
    }

}
