using UnityEngine;

public class Trajectory
{
    public float speed;

    public Trajectory(float speed)
    {
        this.speed = speed;
    }

    public virtual void Movement(Transform transform)
    {
        
    }
}
