using UnityEngine;
using System.Collections.Generic;
using System;

public class Unit : MonoBehaviour
{
    public bool isPlayer;
    public Vector2 movement;
    public float distance;
    private float lastMoved;
    private bool isMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(new Vector2(movement.x, 0) * Time.fixedDeltaTime);
        Move(new Vector2(0, movement.y) * Time.fixedDeltaTime);
        distance += movement.magnitude*Time.fixedDeltaTime;
        if (isPlayer)
        {
            bool movingThisFrame = movement.magnitude > 0.1f;
            if (movingThisFrame && !isMoving)
            {
                isMoving = true;
                EventBus.Instance.Broadcast_OnMove();
                //Debug.Log("started moving at " + Time.time);
            }
            if (isMoving && !movingThisFrame)
            {
                isMoving = false;
                lastMoved = Time.time;
                //Debug.Log("stopped moving at " + Time.time);
            }
            if (!isMoving && !movingThisFrame && Time.time - lastMoved > 3)
            {
                EventBus.Instance.Broadcast_StandingStill();
                //Debug.Log("hasn't been moving for 3 seconds");
            }
        }
    }

    public void Move(Vector2 ds)
    {
        List<RaycastHit2D> hits = new();
        int n = GetComponent<Rigidbody2D>().Cast(ds, hits, ds.magnitude * 2);
        if (n == 0)
        {
            transform.Translate(ds);
        }
    }


}
