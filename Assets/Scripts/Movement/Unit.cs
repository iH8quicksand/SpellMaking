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
    private float velocityY = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(new Vector3(movement.x, 0, movement.y) * Time.fixedDeltaTime);
        //Move(new Vector3(0, 0, movement.y) * Time.fixedDeltaTime);
        distance += movement.magnitude * Time.fixedDeltaTime;
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
        if (velocityY > 0f)
        {
            transform.Translate(new Vector3(0f, velocityY, 0f));
            velocityY -= 0.01f;
        }
        if (velocityY < 0f) velocityY = 0f;

    }

    public void Move(Vector3 movementVector)
    {
        //bool isHit = GetComponent<Rigidbody>().SweepTest(movementVector, out RaycastHit hit, movementVector.magnitude * 2);
        //if (!isHit)
        //{
            transform.Translate(movementVector);
        //}
    }

    public void Jump()
    {
        velocityY = 0.2f;
    }


}
