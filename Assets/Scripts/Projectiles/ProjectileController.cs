using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

public class ProjectileController : MonoBehaviour
{
    public int collisions = 0;
    public float startTime;
    public float lifetime;
    public event Action<Hittable,Vector3> OnHit;
    public event Action<ProjectileController> OnProjectileCollision;
    public Trajectory movement;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        movement.Movement(transform);
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        Transform playerPosition = GameManager.Instance.player.transform;
        transform.Find("sprite").LookAt(playerPosition);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("projectile")) return;
        if (collision.gameObject.CompareTag("unit"))
        {
            if (collision.gameObject.TryGetComponent<EnemyController>(out var ec))
            {
                OnHit(ec.hp, transform.position);
            }
            else if (collision.gameObject.TryGetComponent<PlayerController>(out var pc))
            {
                OnHit(pc.hp, transform.position);
            }
        }
        //Debug.Log(collision.gameObject.name);
        OnProjectileCollision(this);
    }

    public void SetLifetime(float lifetime)
    {
        if (lifetime>0) StartCoroutine(Expire(lifetime));
    }

    IEnumerator Expire(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
