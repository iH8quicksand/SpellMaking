using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;
    
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;
    public SpellManager spellManager;
    public RewardScreenManager rewardScreenManager;
    public EnemySpawner enemySpawner;
    public SpellUIContainer spellUIContainer;
    public PlayerController playerController;

    public int wave = 1;
    public int total_damage_dealt = 0;
    
    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void ResetEnemyCount()
    {
        enemies.Clear();
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }
    
    

    public void RegisterDamage(int amount)
    {
        total_damage_dealt += amount;
    }

    private GameManager()
    {
        enemies = new List<GameObject>();
    }

    public void RoundOver()
    {
        state = GameState.WAVEEND;
        wave++;
        player.GetComponent<PlayerController>().updatePlayerStats(wave);
        rewardScreenManager.Show();
    }
    public void StartCountdown()
    {
        CoroutineManager.Instance.Run(Countdown());
    }
    public void NextRound()
    {
        enemySpawner.NextWave();
    }
    IEnumerator Countdown()
    {
        state = GameState.COUNTDOWN; // This is for countdown till the next wave
        countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            countdown--;
        }
        state = GameState.INWAVE;
        NextRound();
    }
}
