using UnityEngine;

public class EnemySpriteManager : IconManager
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.enemySpriteManager = this;
    }
}
