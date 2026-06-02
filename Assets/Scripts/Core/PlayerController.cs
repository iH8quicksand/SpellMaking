using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static RPNEvaluator.RPNEvaluator;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;
    public SpriteRenderer spriteRenderer;

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

        spellcaster = new SpellCaster(125, 8, 10, Hittable.Team.PLAYER)
        {
            transform = this.transform
        };
        StartCoroutine(spellcaster.ManaRegeneration());
        EventBus.Instance.GainMana += spellcaster.GainMana;
        EventBus.Instance.GainSpellPower += spellcaster.GainSpellPower;
        
        hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;
        EventBus.Instance.GainHealth += hp.GainHP;

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
        GameManager.Instance.playerSpriteManager.PlaceSprite(pc.Sprite, spriteRenderer);
    }

    public void UpdatePlayerStats(int wave)
    {
        Dictionary<string,int> RPNDict = new() { { "wave", wave } };
        hp.SetMaxHP(Evaluate(playerClass.Health, RPNDict));
        spellcaster.max_mana = Evaluate(playerClass.Mana, RPNDict);
        spellcaster.mana_reg = Evaluate(playerClass.Mana_Regeneration, RPNDict);
        spellcaster.spell_power = Evaluate(playerClass.Spellpower, RPNDict);
        speed = Evaluate(playerClass.Speed, RPNDict);
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE) return;
        unit.movement = value.Get<Vector2>()*speed;
    }

    void Die()
    {
        Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }

}
