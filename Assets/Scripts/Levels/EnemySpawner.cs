using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Unity.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.VFX;
using static RPNEvaluator.RPNEvaluator;
using UnityEngine.SceneManagement;


public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public Image class_selector;
    public GameObject button;
    public GameObject playerClassPrefab;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints; 
    Dictionary<string, Enemy> enemy_types = new Dictionary<string, Enemy>(); 
    Dictionary<string, Level> level_types = new Dictionary<string, Level>(); 
    Dictionary<string, PlayerClass> class_types = new Dictionary<string, PlayerClass>(); 
    public string currentLevelname;
    public int wave_count;
    public int delay = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadEnemyType();
        LoadLevelType(); 
        // loop through levels and add a button for each difficulty
        
        int totalLevels = level_types.Count;
        float spacing = 50f;
        float startY = ((totalLevels - 1) * spacing) / 2f;
        float currentY = startY;
        foreach (var kvp in level_types)
        {
            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, currentY, 0);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(kvp.Key);
            currentY -= spacing;
        }

        
        LoadClassTypes();
        int totalClasses = class_types.Count;
        float spacingX = 200f;
        float startX = -((totalClasses - 1) * spacingX) / 2f;
        float currentX = startX;
        foreach (var kvp in class_types)
        {
            GameObject selector = Instantiate(playerClassPrefab, class_selector.transform);
            selector.transform.localPosition = new Vector3(currentX, -145, 0);
            selector.GetComponent<ClassSelectorController>().spawner = this;
            selector.GetComponent<ClassSelectorController>().SetClass(kvp.Key, kvp.Value);
            currentX += spacingX;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel(string levelname)
    {
        wave_count = 1;
        currentLevelname = levelname;
        
        level_selector.gameObject.SetActive(false);
        class_selector.gameObject.SetActive(true);
    }

    public void StartLevel(PlayerClass playerClass)
    {
        class_selector.gameObject.SetActive(false);
        GameManager.Instance.player.GetComponent<PlayerController>().UpdatePlayerClass(playerClass);
        GameManager.Instance.player.GetComponent<PlayerController>().UpdatePlayerStats(1);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        Debug.Log($"Starting level: {currentLevelname}");

        StartCoroutine(SpawnWave()); // I feel like we should pass the levelname to SpawnWave()
    }

    public void NextWave() // Executed when Next Wave button pressed
    {
        wave_count++;
        GameManager.Instance.player.GetComponent<PlayerController>().UpdatePlayerStats(wave_count);
        EventBus.Instance.Broadcast_WaveStart(); //Currently used just to close reward screen, but should be used more!
        StartCoroutine(SpawnWave());
    }
    
    IEnumerator SpawnWave()
    {
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN; // This is for countdown till the next wave
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        
        Level currentLevel = level_types[currentLevelname];         // sets the current level type
        for (int i = 0; i < currentLevel.spawns.Count; i++)                // this spawns the stuff 
        {
            
            Spawn spawn = currentLevel.spawns[i];
            Enemy enemy_data = enemy_types[spawn.enemy];
            
            SetPerameters parameters =  new SetPerameters // saves the parameters to the builder class to get later.
            {
                type = spawn.enemy,
                hp = Evaluate(spawn.hp, new Dictionary<string, int> {{ "base", enemy_data.hp }, { "wave", wave_count }}),
                damage = Evaluate(spawn.damage ?? "base" , new Dictionary<string, int> {{ "base", enemy_data.damage }, { "wave", wave_count }}),
                speed = Evaluate(enemy_data.speed.ToString(), new Dictionary<string, int>{{ "base", enemy_data.speed }, { "wave", wave_count }}),
                delay = currentLevel.spawns[i].delay,
                location = currentLevel.spawns[i].location,
                
            };
            int count = Evaluate(spawn.count, new Dictionary<string, int> { { "wave", wave_count } });
            if (count <= 0) count = 1;

            // fallback to spawning 1 at a time
            int[] sequence = (spawn.sequence != null && spawn.sequence.Length > 0) ? spawn.sequence : new int[] { 1 };
            
            int sequenceIndex = 0;
            int spawnedSoFar = 0;

            while (spawnedSoFar < count)
            {
                int batchSize = sequence[sequenceIndex % sequence.Length];
                
                batchSize = Mathf.Min(batchSize, count - spawnedSoFar);

                for (int index = 0; index < batchSize; index++)
                {
                    Debug.unityLogger.Log(spawn.enemy);
                    SpawnEnemy(parameters); 
                }

                spawnedSoFar += batchSize;
                sequenceIndex++;

                // wait before triggering the next batch 
                if (spawnedSoFar < count)
                {
                    float waitTime = parameters.delay == 0 ? 2f : parameters.delay;
                    yield return new WaitForSeconds(waitTime);
                }
            }

        }
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
        EventBus.Instance.Broadcast_WaveEnd();
    }

    void SpawnEnemy(SetPerameters parameters)                                // going to need to add the other perimeters like 
    {

        SpawnPoint spawn_point = null;
        if (!string.IsNullOrEmpty(parameters.location))
        {
            SpawnPoint[] matchingSpawns = System.Array.FindAll(SpawnPoints, sp => 
                parameters.location.ToUpper().Contains(sp.kind.ToString().ToUpper())
            );
            if (matchingSpawns.Length > 0)
            {
                spawn_point = matchingSpawns[Random.Range(0, matchingSpawns.Length)];
            }
            else
            {
                // fallback: Check if the JSON used the exact GameObject name instead (e.g., "RedSpawnSouthWing")
                spawn_point = System.Array.Find(SpawnPoints, sp => sp.name == parameters.location);
            }
        }
        
        if (spawn_point == null)
        {
            spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }

        Debug.Log($"Spawning {parameters.type} at {spawn_point.name} | position: {spawn_point.transform.position}");

        Vector3 initial_position = spawn_point.GetRandomPosition();

        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);
        
        Enemy data = enemy_types[parameters.type];                                   // get the name of the enemy to are makeing
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance
                                     .enemySpriteManager.Get(data.sprite);           // assign the sprite of the name
        new_enemy.GetComponent<EnemyController>().SetParameters(parameters);         // assign the contoller to the name and parameters
                                                        // function in enemycontroller
        GameManager.Instance.AddEnemy(new_enemy);                                    // creat the enemy in the game
    }
    
    
    public void LoadEnemyType()
    {
        var enemytext = Resources.Load<TextAsset>("enemies");   // this loads the enemies files
        JToken jo = JToken.Parse(enemytext.text);
        foreach (var enemy in jo)
        {
            Enemy en = enemy.ToObject<Enemy>();
            enemy_types[en.name] = en;
        }
    }

    public void LoadLevelType()
    {
        var levelstext = Resources.Load<TextAsset>("levels");
        JToken jo = JToken.Parse(levelstext.text);
        foreach (var levelIterator in jo)
        {
            Level level = levelIterator.ToObject<Level>();
            level_types[level.name] = level;
        }
        
        foreach (var kvp in level_types)
        {
            Level level = kvp.Value;
            Debug.Log($"=== LEVEL: {level.name} | Waves: {level.waves} | Total Spawns: {level.spawns.Count} ===");
            
        }
    }

    private void LoadClassTypes()
    {
        var classesJSON = Resources.Load<TextAsset>("classes");
        class_types = JsonConvert.DeserializeObject<Dictionary<string, PlayerClass>>(classesJSON.text);
        //foreach (var (key, value) in class_types) //Just prints the contents of class_types
        //{
        //    Debug.Log("name: " + key);
        //    Debug.Log("sprite: " + value.sprite);
        //    Debug.Log("health: " + value.health);
        //    Debug.Log("mana: " + value.mana);
        //    Debug.Log("mana_regeneration: " + value.mana_regeneration);
        //    Debug.Log("spellpower: " + value.spellpower);
        //    Debug.Log("speed: " + value.speed);
        //}
    }

    public void RestartLevel()
    {
        GameManager.Instance.state = GameManager.GameState.PREGAME;
        StopAllCoroutines(); // stop SpawnWave from finishing
        GameManager.Instance.ResetEnemyCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DEBUG_FinishWave()
    {
        EventBus.Instance.Broadcast_WaveEnd();
    }
}