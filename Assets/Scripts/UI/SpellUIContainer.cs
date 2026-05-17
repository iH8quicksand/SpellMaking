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
    }

    // Update is called once per frame
    void Update()
    {
        
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
            if (spellUIs[freeIndex].activeSelf == false) break;
            freeIndex++;
        }
        //Next, activate the spell UI and set its spell
        spellUIs[freeIndex].GetComponent<SpellUI>().SetSpell(newSpell);
        spellUIs[freeIndex].SetActive(true);
    }

}
