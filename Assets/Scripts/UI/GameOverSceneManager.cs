using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverSceneManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public TextMeshProUGUI damageText;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

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