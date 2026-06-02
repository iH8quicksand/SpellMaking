using UnityEngine;

public class StraightTrajectory : Trajectory
{
    public StraightTrajectory(float speed) : base(speed)
    {

    }

    public override void Movement(Transform transform)
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
    }
}
