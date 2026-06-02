using UnityEngine;

public class RelicUIManager : MonoBehaviour
{
    public GameObject relicUIPrefab;
    public PlayerController player;

    public void OnRelicPickup(Relic r)
    {
        // make a new Relic UI representation
        GameObject rui = Instantiate(relicUIPrefab, transform);
        rui.transform.localPosition = new Vector3(-450 + 40 * (transform.childCount - 1), 0, 0);
        RelicUI ruic = rui.GetComponent<RelicUI>();
        ruic.player = player;
        ruic.index = transform.childCount - 1;
        ruic.SetRelic(r);
        
    }
}
