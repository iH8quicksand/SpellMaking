using UnityEngine;

public class SpiralingTrajectory : Trajectory
{
    public float start;
    public SpiralingTrajectory(float speed) : base(speed)
    {
        start = Time.time;
    }

    public override void Movement(Transform transform)
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        transform.Rotate(0, speed * Mathf.Sqrt(speed) * Time.deltaTime * 20.0f / (1 + Random.value + Time.time - start), 0);
    }
}
