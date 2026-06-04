using UnityEngine;

public class SpiralingHomingTrajectory : Trajectory
{
    public float start;
    float angle;
    readonly float turn_rate;
    public SpiralingHomingTrajectory(float speed) : base(speed)
    {
        start = Time.time;
        angle = float.NaN;
        turn_rate = 0.25f;
    }

    public override void Movement(Transform transform)
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        transform.Rotate(0, speed * Mathf.Sqrt(speed) * Time.deltaTime * 20.0f / (1 + Random.value + Time.time - start), 0);

        if (float.IsNaN(angle))
        {
            Vector3 direction = transform.rotation * new Vector3(1, 0, 0);
            angle = Mathf.Atan2(direction.y, direction.x);
        }
        GameObject closest = GameManager.Instance.GetClosestEnemy(transform.position);
        if (closest == null)
        {
            Vector3 direction = transform.rotation * new Vector3(1, 0, 0);
            angle = Mathf.Atan2(direction.y, direction.x);
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
        }
        else
        {
            Vector3 new_direction = (closest.transform.position - transform.position).normalized;
            float new_angle = Mathf.Atan2(new_direction.y, new_direction.x);
            if (Mathf.Abs(angle - new_angle) > Mathf.Epsilon)
            {
                float da = new_angle - angle;
                if (da > Mathf.PI)
                {
                    da -= 2 * Mathf.PI;
                }
                if (da < -Mathf.PI)
                {
                    da += 2 * Mathf.PI;
                }
                angle += Mathf.Clamp(da, -turn_rate * Mathf.Deg2Rad, turn_rate * Mathf.Deg2Rad);

            }
            Vector3 direction = new(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            transform.Translate(speed * Time.deltaTime * direction.normalized, Space.World);
        }
    }
}

