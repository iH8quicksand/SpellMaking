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
using UnityEngine.InputSystem.Controls;
using static RPNEvaluator.RPNEvaluator;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public PlayerHealthBar healthui;
    public ManaBar manaui;
    //public SpriteRenderer spriteRenderer;

    public SpellCaster spellcaster;
    public SpellUI spellui;

    public int speed;

    public Unit unit;

    public bool dead = false;

    public GameObject cam;
    public float sensitivity = 0.1f;
    public InputActionReference SetSpell;
    public PauseMenu pauseMenu;


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

        SetSpell.action.started += TrySetSpell;
    }

    void OnDestroy()
    {
        EventBus.Instance.GainMana -= spellcaster.GainMana;
        EventBus.Instance.GainSpellPower -= spellcaster.GainSpellPower;
        EventBus.Instance.GainHealth -= hp.GainHP;
        spellcaster?.Dispose();
    }

    public void StartLevel()
    {
        UpdatePlayerStats(1);
    }

    public void UpdatePlayerClass(PlayerClass pc)
    {
        playerClass = pc;
        //GameManager.Instance.playerSpriteManager.PlaceSprite(pc.Sprite, spriteRenderer);
    }

    public void UpdatePlayerStats(int wave)
    {
        Dictionary<string, int> RPNDict = new() { { "wave", wave } };
        hp.SetMaxHP(Evaluate(playerClass.Health, RPNDict));
        spellcaster.max_mana = Evaluate(playerClass.Mana, RPNDict);
        spellcaster.mana_reg = Evaluate(playerClass.Mana_Regeneration, RPNDict);
        spellcaster.spell_power = Evaluate(playerClass.Spellpower, RPNDict);
        speed = Evaluate(playerClass.Speed, RPNDict);
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE) return;
        //if (EventSystem.current.IsPointerOverGameObject()) return; //<-- Doesn't matter in 3D since cursor is always centered
        StartCoroutine(spellcaster.Cast(cam.transform));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE) return;
        unit.movement = value.Get<Vector2>() * speed;
    }

    void OnLook(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE && GameManager.Instance.state != GameManager.GameState.COUNTDOWN)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector2 lookVector = value.Get<Vector2>();
        Vector3 rotation = cam.transform.rotation.eulerAngles;
        float rotationX = rotation.x - lookVector.y * sensitivity;
        if (rotationX > 90f && rotationX <= 270f) rotationX = (180f - rotationX >= 0) ? 90f : 270f;
        cam.transform.rotation = Quaternion.Euler(rotationX, rotation.y, rotation.z);
        transform.Rotate(lookVector.x * sensitivity * Vector3.up);
    }

    void OnJump(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE) return;
        unit.Jump();
    }

    void OnPause(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.INWAVE) pauseMenu.TogglePaused();
    }

    void TrySetSpell(InputAction.CallbackContext context)
    {
        int index = int.Parse(context.control.name) - 1;
        if (index < spellcaster.spells.Count) EventBus.Instance.Broadcast_SetSpell(index);
    }

    void Die()
    {
        //Debug.Log("You Lost");
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }

}
