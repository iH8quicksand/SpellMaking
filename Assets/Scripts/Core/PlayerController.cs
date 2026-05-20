using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static RPNEvaluator.RPNEvaluator;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;

    public int speed;

    public Unit unit;
    
    public bool dead = false;

    private PlayerClass playerClass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;

        spellcaster = new SpellCaster(125, 8, 10, Hittable.Team.PLAYER);
        spellcaster.transform = this.transform;
        StartCoroutine(spellcaster.ManaRegeneration());
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        spellui.SetSpell(spellcaster.spells[0]);
    }

    public void StartLevel()
    {
        UpdatePlayerStats(1);
    }

    public void UpdatePlayerClass(PlayerClass pc)
    {
        playerClass = pc;
    }

    public void UpdatePlayerStats(int wave)
    {
        Dictionary<string,int> RPNDict = new Dictionary<string, int> { { "wave", wave } };
        hp.SetMaxHP(Evaluate(playerClass.health, RPNDict));
        spellcaster.max_mana = Evaluate(playerClass.mana, RPNDict);
        spellcaster.mana_reg = Evaluate(playerClass.mana_regeneration, RPNDict);
        spellcaster.spell_power = Evaluate(playerClass.spellpower, RPNDict);
        speed = Evaluate(playerClass.speed, RPNDict);
    }

    // Update is called once per frame
    void Update()
    {
        //if (dead) Die();
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        unit.movement = value.Get<Vector2>()*speed;
    }

    void Die()
    {
        Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }

}
