using UnityEngine;

public class HomingTrajectory : Trajectory
{
    float angle;
    readonly float turn_rate;
    public HomingTrajectory(float speed) : base(speed)
    {
        angle = float.NaN;
        turn_rate = 0.25f;//max degrees turn per Update() frame
    }

    public override void Movement(Transform transform)
    {
        if (float.IsNaN(angle))
        {
            Vector3 direction = transform.rotation * new Vector3(0, 0, 1);
            angle = Mathf.Atan2(direction.z, direction.x);
        }
        GameObject closest = GameManager.Instance.GetClosestEnemy(transform.position);
        if (closest == null)
        {
            Vector3 direction = transform.rotation * new Vector3(0, 0, 1);
            angle = Mathf.Atan2(direction.z, direction.x);
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        }
        else //we have the angle (in the xz plane) of where the projectile is currently heading, and we have the closest enemy
        {
            Vector3 new_direction = (closest.transform.position - transform.position).normalized;
            float new_angle = Mathf.Atan2(new_direction.z, new_direction.x);
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
