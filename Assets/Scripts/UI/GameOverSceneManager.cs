using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverSceneManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public TextMeshProUGUI damageText;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {  
            damageText.text = $"YOU DIED";
            gameOverUI.SetActive(true);
        }
        else
        {
            gameOverUI.SetActive(false);
        }
    }
}