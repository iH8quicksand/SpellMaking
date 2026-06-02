using UnityEngine;

public class HomingTrajectory : Trajectory
{
    float angle;
    readonly float turn_rate;
    public HomingTrajectory(float speed) : base(speed)
    {
        angle = float.NaN;
        turn_rate = 0.25f;
    }

    public override void Movement(Transform transform)
    {
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
            Vector3 direction = new(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            transform.Translate(speed * Time.deltaTime * direction.normalized, Space.World);
        }
    }
}
